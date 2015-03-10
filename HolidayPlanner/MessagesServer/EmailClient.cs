using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolidayPlanner.Domain;
using HolidayPlanner.EventsSystem;

namespace HolidayPlanner.MessagesServer
{
    public class EmailClient : IEmailClient
    {
        private Dictionary<string, Action<HolidayRequest>> subscriptions;
        private IEventSystem eventSystem;

        public EmailClient() : this(new EventSystem())
        {
        }

        public EmailClient(IEventSystem eventSystem)
        {
            this.eventSystem = eventSystem;
            subscriptions = new Dictionary<string, Action<HolidayRequest>>();
        }

        public async virtual Task SubscribeAsync(Employee employee, Action<HolidayRequest> callback)
        {
            subscriptions.Add(employee.Name, callback);
            await eventSystem.SubscribeAsync<HolidayRequest>(GetChannel(employee.Name), ChannelPattern.Literal, HandleNewRequest);
        }

        public async Task UnsubscribeAsync(Employee employee)
        {
            subscriptions.Remove(employee.Name);
            await eventSystem.UnsubscribeAsync(GetChannel(employee.Name), ChannelPattern.Literal);
        }       

        public async virtual Task SendEmailAsync(HolidayRequest request)
        {
            await eventSystem.PublishAsync<HolidayRequest>(GetChannel(request.ToEmployeeName), ChannelPattern.Literal, request);
        }        

        internal void HandleNewRequest(string channel, HolidayRequest holidayRequest )
        {
            foreach (var subscription in subscriptions)
            {
                if (subscription.Key.Equals(holidayRequest.ToEmployeeName))
                {
                    subscription.Value.Invoke(holidayRequest);
                }
            }
        }

        private string GetChannel(string employeeName)
        {
            return string.Format("Channel: {0}", employeeName);
        }      
        
    }
}
