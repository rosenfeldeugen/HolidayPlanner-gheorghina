using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayPlanner.Domain.Exceptions
{
    public class UnsupportedActionException: Exception
    {
        public override string Message
        {
            get
            {
                return "Unsupported Action";
            }
        }
    }
}
