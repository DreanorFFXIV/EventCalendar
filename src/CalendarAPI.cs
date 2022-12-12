using System;
using System.Buffers.Text;
using System.IO;
using System.Reflection;
using System.Text;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace EventCalendar
{
    public class CalendarAPI
    {
        private EventsResource.ListRequest Request { get; }
        
        public CalendarAPI()
        {
            var base64EncodedBytes = Convert.FromBase64String(Read());
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                ApiKey = Encoding.UTF8.GetString(base64EncodedBytes),
                ApplicationName = "FFXIVEVENTS",
            });
            
            Request = service.Events.List("5de3023ee82a703ccd7c4b71f0e34418967b28f0001cdb722cb261e11d51dc8d@group.calendar.google.com");
            Request.TimeMin = DateTime.Now;
            Request.ShowDeleted = false;
            Request.SingleEvents = true;
            Request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        }

        public Events GetEvents()
        {
            return Request.Execute();
        }

        private string Read()
        {
            var rootType = typeof(CalendarAPI);  
            var resourceNames = rootType.Assembly.GetManifestResourceNames();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = resourceNames[0];
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}