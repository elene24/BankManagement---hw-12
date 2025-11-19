using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankManagement.Enums;

namespace BankManagement.Models
{


        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string IdentityNumber { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public CustomerType Type { get; set; }

            public override string ToString()
            {
                return $"{Id}: {Name} ({Type}) - {PhoneNumber} | {Email} | ID: {IdentityNumber}";
            }
        }
    
}
