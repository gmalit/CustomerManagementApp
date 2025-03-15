using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementApp
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
