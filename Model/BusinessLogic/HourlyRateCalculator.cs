using System;
using System.Collections.Generic;
using System.Text;
using Soul_and_talk.Model;

namespace Soul_and_talk.Model.BusinessLogic
{
    public interface IHourlyRateCalculator
    {
        decimal GetRatePerHour(Customer customer, bool isPhysical);
    }
}
