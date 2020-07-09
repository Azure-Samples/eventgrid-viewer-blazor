using Blazor.EventGridViewer.Core;
using Blazor.EventGridViewer.Services;
using Blazor.EventGridViewer.Services.Interfaces;
using Xunit;

namespace Blazor.EventGridViewer.Unit.Tests
{
    /// <summary>
    /// Test class used to test the EventGridIdentifySchemaService class
    /// </summary>
    public class EventGridIdentifySchemaServiceTests
    {
        /// <summary>
        /// Testing the Identify method to identify a CloudEvent
        /// </summary>
        [Fact]
        public void EventGridIdentifySchemaServiceCanIdentifyCloudEvent()
        {
            // Arrange
            IEventGridIdentifySchemaService service = new EventGridIdentifySchemaService();

            // Act
            EventGridSchemaType type = service.Identify(Data.GetMockCloudEventJson());

            // Assert
            Assert.True(type == EventGridSchemaType.CloudEvent);
        }

        /// <summary>
        /// Testing the Identify method to identify a CloudEvent with extra properties
        /// </summary>
        [Fact]
        public void EventGridIdentifySchemaServiceCanIdentifyCloudEventExtraProperties()
        {
            // Arrange
            IEventGridIdentifySchemaService service = new EventGridIdentifySchemaService();

            // Act
            EventGridSchemaType type = service.Identify(Data.GetMockCloudEventJson());

            // Assert
            Assert.True(type == EventGridSchemaType.CloudEvent);
        }

        /// <summary>
        /// Testing the Identify method to identify a EventGridEvent
        /// </summary>
        [Fact]
        public void EventGridIdentifySchemaServiceCanIdentifyEventGridEvent()
        {
            // Arrange
            IEventGridIdentifySchemaService service = new EventGridIdentifySchemaService();

            // Act
            EventGridSchemaType type = service.Identify(Data.GetMockEventGridEventJson());

            // Assert
            Assert.True(type == EventGridSchemaType.EventGrid);
        }
    }
}
