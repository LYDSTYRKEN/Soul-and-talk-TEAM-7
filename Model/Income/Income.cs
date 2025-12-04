using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_and_talk.Model.Income
{
    internal class Income
    {
        public Customer Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public bool IsPhysical { get; set; }
        public decimal Kilometers { get; set; }
        public decimal Amount { get; set; }
        public Income()
        {
            Customer = new Customer();
        }


    }
}
