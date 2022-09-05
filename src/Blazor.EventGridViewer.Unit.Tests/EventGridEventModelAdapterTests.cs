using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Adapters;
using Blazor.EventGridViewer.Services.Interfaces;
using Xunit;

namespace Blazor.EventGridViewer.Unit.Tests
{
    /// <summary>
    /// Test Class used to the the EventGridEventModelAdapter Class
    /// </summary>
    public class EventGridEventModelAdapterTests
    {
        /// <summary>
        /// Testing that the Convert method throws an exception if the EventGridEventModel is null
        /// </summary>
        [Fact]
        public void EventGridEventModelAdatperConvertNullThrowsExceptionTest()
        {
            // Arrange
            IAdapter<EventGridEventModel, EventGridViewerEventModel> adapter = new EventGridEventModelAdapter();

            // Act
            var exception = Record.Exception(() => adapter.Convert(null));

            // Assert
            Assert.NotNull(exception);
        }

        /// <summary>
        /// Testing that the Convert method can convert
        /// </summary>
        [Fact]
        public void EventGridEventModelAdapterConvertCanConvertEventGridEventModel()
        {
            // Arrange
            IAdapter<EventGridEventModel, EventGridViewerEventModel> adapter = new EventGridEventModelAdapter();
            var eventGridEventModel = new EventGridEventModel() { Data = "SomeJson", EventType = "SomeEventType", Id = "SomeId", Subject = "SomeSubject" };

            // Act
            var eventGridViewerEventModel = adapter.Convert(eventGridEventModel);

            // Assert
            Assert.True(eventGridEventModel.Id == eventGridViewerEventModel.Id && eventGridEventModel.Subject == eventGridViewerEventModel.Subject &&
                eventGridEventModel.EventType == eventGridViewerEventModel.EventType && eventGridEventModel.Data == eventGridViewerEventModel.Data &&
                eventGridEventModel.EventTime == eventGridViewerEventModel.EventTime);
        }
    }
}
