using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_and_talk.Model.BusinessLogic
{
    public interface HourlyRateCalculator
    {
        decimal GetRatePerHour(Customer Customer, bool IsPhysical);
    }
}
