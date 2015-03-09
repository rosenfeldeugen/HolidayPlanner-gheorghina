﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolidayPlanner.Domain;

namespace HolidayPlanner.MessagesServer
{
    public interface IEmailClient
    {
        Task SubscribeAsync(Employee employee, Action<HolidayRequest> callback);

        Task UnsubscribeAsync(Employee employee);

        Task SendEmailAsync(HolidayRequest request);
    }
}
