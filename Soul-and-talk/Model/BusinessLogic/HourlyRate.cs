namespace Soul_and_talk.Model.BusinessLogic
{
    public class HourlyRate : IHourlyRateCalculator
    {
        public decimal GetRatePerHour(Customer customer, bool isPhysical)
        {
            if (customer == null)
            {
                return 0;
            }

            // Privat kunde (ingen institution)
            if (customer.Institution == null)
            {
                if (isPhysical)
                    return 450m;
                else
                    return 350m;
            }

            // Offentlig institution
            if (customer.Institution.Type == InstitutionType.Public)
            {
                // samme pris fysisk/online
                return 550m;
            }

            // Privat institution
            if (isPhysical)
                return 450m;
            else
                return 350m;
        }
    }
}