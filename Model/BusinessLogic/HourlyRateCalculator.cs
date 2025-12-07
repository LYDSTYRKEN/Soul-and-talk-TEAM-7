// Peter 

namespace Soul_and_talk.Model.BusinessLogic
{
    public interface IHourlyRateCalculator
    {
        decimal GetRatePerHour(Customer customer, bool isPhysical);
    }
}

// Peter Her Til