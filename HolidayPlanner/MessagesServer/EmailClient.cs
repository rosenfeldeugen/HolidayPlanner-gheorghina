using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolidayPlanner.Domain;
using HolidayPlanner.EventsSystem;

namespace HolidayPlanner.MessagesServer
{
    //CR: you can remove the dependency on Employee by receiving just its name
    public class EmailClient : IEmailClient
    {
        //CR: Dictionary is not thread safe. Because you are using async, I expect that the subscriptions to be accessed from many threads 
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

        //CR: SubscribeAsync(string name, Action<HolidayRequest> callback)
        public async virtual Task SubscribeAsync(Employee employee, Action<HolidayRequest> callback)
        {
            //CR: be careful name is not unique
            subscriptions.Add(employee.Name, callback);
            await eventSystem.SubscribeAsync<HolidayRequest>(GetChannel(employee.Name), ChannelPattern.Literal, HandleNewRequest);
        }

        //CR: UnsubscribeAsync(string name)
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
            //CR: how about subscriptions[holidayRequest.ToEmployeeName].Invoke(holidayRequest);

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
