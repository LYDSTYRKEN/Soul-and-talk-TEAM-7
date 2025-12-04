using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_and_talk.Model.Customer
{
    internal class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Institution Institution { get; set; }
        public bool IsPrivateCustomer
        {
            get
            {
                return Institution == null;
            }
        }
    }
}
