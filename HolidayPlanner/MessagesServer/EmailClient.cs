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
        private Dictionary<Employee, Action<HolidayRequest>> subscriptions;
        private IEventSystem eventSystem;

        public EmailClient() : this(new EventSystem())
        {
        }

        public EmailClient(IEventSystem eventSystem)
        {
            this.eventSystem = eventSystem;
            subscriptions = new Dictionary<Employee, Action<HolidayRequest>>();
        }

        public virtual void Subscribe(Employee employee, Action<HolidayRequest> callback)
        {
            subscriptions.Add(employee, callback);
            eventSystem.SubscribeAsync<HolidayRequest>(GetChannel(employee), ChannelPattern.Literal, HandleNewRequest);
        }

        public void Unsubscribe(Employee employee)
        {
            subscriptions.Remove(employee);
            eventSystem.UnsubscribeAsync(GetChannel(employee), ChannelPattern.Literal);
        }       

        public virtual void SendEmail(HolidayRequest request)
        {
            eventSystem.PublishAsync<HolidayRequest>(GetChannel(request.ToEmployee), ChannelPattern.Literal, request);
        }        

        private void HandleNewRequest(string channel, HolidayRequest holidayRequest )
        {
            foreach (var subscription in subscriptions)
            {
                if (subscription.Key.Equals(holidayRequest.ToEmployee))
                {
                    subscription.Value.Invoke(holidayRequest);
                }
            }
        }

        private string GetChannel(Employee employee)
        {
            return string.Format("Channel: {0}.{1}", employee.Name, employee.Role);
        }
       
        public object SendEmail(object p)
        {
            throw new NotImplementedException();
        }
    }
}
