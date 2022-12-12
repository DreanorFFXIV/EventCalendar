using System;
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
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBgvc495CAKUdRpocz33pttO_q2JigeC6M",
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
    }
}