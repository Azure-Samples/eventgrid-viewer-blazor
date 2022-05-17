using Newtonsoft.Json.Linq;
using System;

namespace Blazor.EventGridViewer.Unit.Tests
{
    /// <summary>
    /// Class used to create mock data
    /// </summary>
    public static class Data
    {
        /// <summary>
        /// Create a mock EventGridEvent json
        /// </summary>
        /// <returns></returns>
        public static string GetMockEventGridEventJson()
        {
            // Note: guids were generated with an online guid generator
            string mockEventGridEvent = "[{\"id\": \"efdae305-d50f-49f6-a6ec-bd632d52bb3b\",\"topic\":\"subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",\"subject\":\"EventGridSubscriptionValidation\",\"data\": {\"validationCode\": \"595c123c-0b91-4fa1-8b40-36f2e44194c9\"},\"eventType\": \"Microsoft.EventGrid.SubscriptionValidationEvent\",\"eventTime\": \"2018-01-25T22:12:19.4556811Z\",\"metadataVersion\": \"1\",\"dataVersion\": \"1\"}]";
            return mockEventGridEvent;
        }

        /// <summary>
        /// Create a mock CloudEvent json
        /// </summary>
        /// <returns></returns>
        public static string GetMockCloudEventJson()
        {
            // Note: guids were generated with an online guid generator
            string mockCloudEvent = "{\"specversion\":\"1.0\",\"type\":\"com.github.pull.create\",\"source\":\"https://github.com/cloudevents/spec/pull\",\"subject\": \"123\",\"id\": \"efdae305-d50f-49f6-a6ec-bd632d52bb3b\",\"time\": \"2018-01-25T22:12:19.4556811Z\",\"data\": {\"make\": \"Ducati\",\"model\":\"Monster\"}}";
            return mockCloudEvent;
        }

        /// <summary>
        /// Create a mock CloudEvent with extra properties json
        /// </summary>
        /// <returns></returns>
        public static string GetMockCloudEventExtraPropertiesJson()
        {
            // Note: guids were generated with an online guid generator
            string mockCloudEventExtraProperties = "[{\"specversion\":\"1.0\",\"type\":\"com.github.pull.create\",\"source\":\"https://github.com/cloudevents/spec/pull\",\"subject\":\"123\",\"id\":\"efdae305-d50f-49f6-a6ec-bd632d52bb3b\",\"time\": \"2018-01-25T22:12:19.4556811Z\",\"comexampleothervalue\":\"5\",\"data\": {\"make\": \"Ducati\",\"model\":\"Monster\"}}]";
            return mockCloudEventExtraProperties;
        }
    }
}
