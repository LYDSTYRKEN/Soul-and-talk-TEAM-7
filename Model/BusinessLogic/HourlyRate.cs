using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_and_talk.Model.BusinessLogic
{
    internal class HourlyRate : HourlyRateCalculator
    {
        public decimal GetRatePerHour(Customer Customer, bool IsPhysical)
        {
            if (Customer.Institution == null)
            {
                if (IsPhysical)
                {
                    {
                        return 450m;
                    }
                    else
                    {
                        return 350m;
                    }
                }
            }
            
            if (Customer.Institution.Type == InstitutionType.Puplic)
            {
                if (IsPhysical)
                {
                    {
                        return 550m;
                    }
                    else
                    {
                        return 550m;
                    }
                }
            }

            if (Customer.Institution.Type == InstitutionType.Private)
            {
                if (IsPhysical)
                {
                    {
                        return 450m;
                    }
                    else
                    {
                        return 350m;
                    }
                }
            }
            if (Customer.Institution.Type == InstitutionType.Private)
            {
                if (IsPhysical)
                {
                    {
                        return 450m;
                    }
                    else
                    {
                        return 350m;
                    }
                }

                return 0;

            }
        }
    }
}
