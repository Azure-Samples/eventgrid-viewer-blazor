using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Blazor.EventGridViewer.ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventGridController : ControllerBase
    {
        private readonly IEventGridService _eventGridService;
        private readonly IAdapter<string, List<EventGridEventModel>> _eventGridSchemaAdapter;
        private readonly ILogger<EventGridController> _logger;

        public EventGridController(
            IEventGridService eventGridService,
            IAdapter<string, List<EventGridEventModel>> eventGridSchemaAdapter,
            ILogger<EventGridController> logger)
        {
            _eventGridService = eventGridService ?? throw new ArgumentNullException(nameof(eventGridService));
            _eventGridSchemaAdapter = eventGridSchemaAdapter ?? throw new ArgumentNullException(nameof(eventGridSchemaAdapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.LogDebug("EventGridController initialized");
        }

        /// <summary>
        /// Test Endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation("GET request received on EventGridController");
            return "EventGridController is up and running...";
        }

        /// <summary>
        /// Webhook for the Azure EventGrid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var stopwatch = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();

            _logger.LogInformation("EventGrid POST request started. CorrelationId: {CorrelationId}", correlationId);

            try
            {
                // Log request headers for debugging
                _logger.LogDebug("Request headers: {@Headers}", Request.Headers.Select(h => new { Key = h.Key, Value = h.Value }));

                string json;
                // using StreamReader due to changes in .Net Core 3 serializer ie ValueKind
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    json = await reader.ReadToEndAsync();
                }

                _logger.LogInformation("Received EventGrid payload. Length: {Length}, CorrelationId: {CorrelationId}",
                    json?.Length ?? 0, correlationId);

                if (string.IsNullOrEmpty(json))
                {
                    _logger.LogWarning("Received empty payload. CorrelationId: {CorrelationId}", correlationId);
                    return BadRequest("Empty payload received");
                }

                // Log the raw payload in development for debugging
                if (Request.Headers.ContainsKey("aeg-event-type") && Request.Headers["aeg-event-type"] == "SubscriptionValidation")
                {
                    _logger.LogInformation("Subscription validation request detected. CorrelationId: {CorrelationId}", correlationId);
                }
                else
                {
                    _logger.LogDebug("Raw EventGrid payload: {Payload}", json);
                }

                List<EventGridEventModel> eventGridEventModels;
                try
                {
                    eventGridEventModels = _eventGridSchemaAdapter.Convert(json);
                    _logger.LogInformation("Successfully parsed {EventCount} events. CorrelationId: {CorrelationId}",
                        eventGridEventModels?.Count ?? 0, correlationId);
                }
                catch (Exception adapterEx)
                {
                    _logger.LogError(adapterEx, "Failed to convert EventGrid payload. CorrelationId: {CorrelationId}, Payload: {Payload}",
                        correlationId, json);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to parse EventGrid payload");
                }

                if (eventGridEventModels == null || eventGridEventModels.Count == 0)
                {
                    _logger.LogWarning("No events found in payload. CorrelationId: {CorrelationId}", correlationId);
                    return Ok();
                }

                foreach (var model in eventGridEventModels)
                {
                    _logger.LogInformation("Processing event: {EventType}, Subject: {Subject}, Id: {EventId}, CorrelationId: {CorrelationId}",
                        model.EventType, model.Subject, model.Id, correlationId);

                    // EventGrid validation message
                    if (model.EventType == EventTypes.EventGridSubscriptionValidationEvent)
                    {
                        try
                        {
                            var eventData = ((JObject)(model.EventData)).ToObject<SubscriptionValidationEventData>();
                            var responseData = new SubscriptionValidationResponse()
                            {
                                ValidationResponse = eventData.ValidationCode
                            };

                            _logger.LogInformation("Returning validation response. CorrelationId: {CorrelationId}", correlationId);
                            return Ok(responseData);
                        }
                        catch (Exception validationEx)
                        {
                            _logger.LogError(validationEx, "Failed to process validation event. CorrelationId: {CorrelationId}", correlationId);
                            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to process validation event");
                        }
                    }

                    // handle all other events
                    try
                    {
                        this.HandleEvent(model);
                        _logger.LogInformation("Successfully handled event: {EventType}. CorrelationId: {CorrelationId}",
                            model.EventType, correlationId);
                    }
                    catch (Exception handleEx)
                    {
                        _logger.LogError(handleEx, "Failed to handle event: {EventType}, Subject: {Subject}. CorrelationId: {CorrelationId}",
                            model.EventType, model.Subject, correlationId);
                        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to handle event");
                    }
                }

                stopwatch.Stop();
                _logger.LogInformation("EventGrid POST request completed successfully. Duration: {Duration}ms, CorrelationId: {CorrelationId}",
                    stopwatch.ElapsedMilliseconds, correlationId);

                return Ok();
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Unhandled exception in EventGrid POST endpoint. Duration: {Duration}ms, CorrelationId: {CorrelationId}",
                    stopwatch.ElapsedMilliseconds, correlationId);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Handle EventGrid Event
        /// </summary>
        /// <param name="model">EventGridEventModel</param>
        private void HandleEvent(EventGridEventModel model)
        {
            try
            {
                _logger.LogDebug("Handling event: {EventType}, Subject: {Subject}, Id: {EventId}",
                    model.EventType, model.Subject, model.Id);

                if (_eventGridService == null)
                {
                    _logger.LogError("EventGrid service is null - cannot handle event");
                    throw new InvalidOperationException("EventGrid service is not available");
                }

                var result = _eventGridService.RaiseEventReceivedEvent(model);

                _logger.LogDebug("Event handling result: {Result} for event: {EventId}", result, model.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling event: {EventType}, Subject: {Subject}, Id: {EventId}",
                    model.EventType, model.Subject, model.Id);
                throw; // Re-throw to be caught by the calling method
            }
        }
    }
}
