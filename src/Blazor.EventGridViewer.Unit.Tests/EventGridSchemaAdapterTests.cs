using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Adapters;
using Blazor.EventGridViewer.Services.Interfaces;
using Microsoft.Azure.EventGrid.Models;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blazor.EventGridViewer.Unit.Tests
{
    /// <summary>
    /// Test class used to test the EventGridSchemaAdapter
    /// </summary>
    public class EventGridSchemaAdapterTests
    {
        /// <summary>
        /// Testing that the Convert method throws an exception if the EventGridEvent is null
        /// </summary>
        [Fact]
        public void EventGridSchemaAdapterConvertNullThrowsExceptionTest()
        {
            // Arrange
            Mock<IEventGridIdentifySchemaService> mockEventGridIdentifySchemaService = new Mock<IEventGridIdentifySchemaService>();
            mockEventGridIdentifySchemaService.Setup(s => s.Identify(It.IsAny<string>())).Returns(Core.EventGridSchemaType.CloudEvent);
            IAdapter<string, List<EventGridEventModel>> adapter = new EventGridSchemaAdapter(mockEventGridIdentifySchemaService.Object);

            // Act
            var exception = Record.Exception(() => adapter.Convert(null));

            // Assert
            Assert.NotNull(exception);
        }

        /// <summary>
        /// Testing that the Convert method can convert a EventGridEvent to a EventGridEventModel
        /// </summary>
        [Fact]
        public void EventGridSchemaAdapterConvertEventGridTest()
        {
            // Arrange
            string json = Data.GetMockEventGridEventJson();
            Mock<IEventGridIdentifySchemaService> mockEventGridIdentifySchemaService = new Mock<IEventGridIdentifySchemaService>();
            mockEventGridIdentifySchemaService.Setup(s => s.Identify(json)).Returns(Core.EventGridSchemaType.EventGrid);
            IAdapter<string, List<EventGridEventModel>> adapter = new EventGridSchemaAdapter(mockEventGridIdentifySchemaService.Object);
            var mockModel = JsonConvert.DeserializeObject<List<EventGridEvent>>(json).FirstOrDefault();

            // Act
            var model = adapter.Convert(Data.GetMockEventGridEventJson()).FirstOrDefault();

            // Assert
            Assert.True(model.Id == mockModel.Id && model.Subject == mockModel.Subject &&
                model.EventType == mockModel.EventType && model.EventTime == mockModel.EventTime.ToString("o"));

            var data = JsonConvert.SerializeObject(mockModel, Formatting.Indented);
            Assert.Equal(data, model.Data);
        }

        /// <summary>
        /// Testing that the Convert method can convert a CloudEvent to a EventGridEventModel
        /// </summary>
        [Fact]
        public void EventGridSchemaAdapterConvertCloudEventTest()
        {
            // Arrange
            string json = Data.GetMockCloudEventJson();
            Mock<IEventGridIdentifySchemaService> mockEventGridIdentifySchemaService = new Mock<IEventGridIdentifySchemaService>();
            mockEventGridIdentifySchemaService.Setup(s => s.Identify(json)).Returns(Core.EventGridSchemaType.CloudEvent);
            IAdapter<string, List<EventGridEventModel>> adapter = new EventGridSchemaAdapter(mockEventGridIdentifySchemaService.Object);
            var mockModel = JsonConvert.DeserializeObject<CloudEvent>(json);

            // Act
            var model = adapter.Convert(json).FirstOrDefault();

            // Assert
            Assert.True(model.Id == mockModel.Id && model.Subject == mockModel.Subject &&
                model.EventType == mockModel.Type && model.EventTime == mockModel.Time);

            var data = JsonConvert.SerializeObject(mockModel, Formatting.Indented);
            Assert.Equal(data, model.Data);
        }
    }
}
