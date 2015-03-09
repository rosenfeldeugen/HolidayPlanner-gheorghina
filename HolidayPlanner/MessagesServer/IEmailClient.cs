using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolidayPlanner.Domain;

namespace HolidayPlanner.MessagesServer
{
    public interface IEmailClient
    {
        void Subscribe(Employee employee, Action<HolidayRequest> callback);

        void Unsubscribe(Employee employee);

        void SendEmail(HolidayRequest request);
    }
}
