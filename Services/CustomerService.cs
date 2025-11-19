using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankManagement.Models;
using BankManagement.Enums;

namespace BankManagement.Services
{


        public class CustomerService
        {
            private readonly string filePath = "Data/customers.csv";

            public List<Customer> GetAllCustomers()
            {
                var customers = new List<Customer>();

                if (!File.Exists(filePath))
                    return customers;

                var lines = File.ReadAllLines(filePath);


                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var customer = ParseCustomerFromCsv(line);
                        if (customer != null)
                            customers.Add(customer);
                    }
                }

                return customers;
            }

            public Customer? GetSingleCustomer(int customerId)
            {
                var customers = GetAllCustomers();
                return customers.FirstOrDefault(c => c.Id == customerId);
            }

            public int AddCustomer(Customer model)
            {
                if (string.IsNullOrWhiteSpace(model.Name) ||
                    string.IsNullOrWhiteSpace(model.IdentityNumber) ||
                    string.IsNullOrWhiteSpace(model.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(model.Email))
                {
                    throw new ArgumentException("All customer fields are required");
                }

                var customers = GetAllCustomers();

                model.Id = customers.Count > 0 ? customers.Max(c => c.Id) + 1 : 1;

                customers.Add(model);

                SaveAllCustomers(customers);

                return model.Id;
            }

            public int UpdateCustomer(Customer model)
            {
                if (model.Id <= 0)
                    throw new ArgumentException("Invalid customer ID");

                var customers = GetAllCustomers();
                var existingCustomer = customers.FirstOrDefault(c => c.Id == model.Id);

                if (existingCustomer == null)
                    throw new ArgumentException($"Customer with ID {model.Id} not found");

                if (string.IsNullOrWhiteSpace(model.Name) ||
                    string.IsNullOrWhiteSpace(model.IdentityNumber) ||
                    string.IsNullOrWhiteSpace(model.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(model.Email))
                {
                    throw new ArgumentException("All customer fields are required");
                }

                existingCustomer.Name = model.Name;
                existingCustomer.IdentityNumber = model.IdentityNumber;
                existingCustomer.PhoneNumber = model.PhoneNumber;
                existingCustomer.Email = model.Email;
                existingCustomer.Type = model.Type;

                SaveAllCustomers(customers);
                return model.Id;
            }

            public int DeleteCustomer(int customerId)
            {
                var customers = GetAllCustomers();
                var customerToDelete = customers.FirstOrDefault(c => c.Id == customerId);

                if (customerToDelete == null)
                    throw new ArgumentException($"Customer with ID {customerId} not found");

                customers.Remove(customerToDelete);
                SaveAllCustomers(customers);

                return customerId;
            }

            private Customer? ParseCustomerFromCsv(string csvLine)
            {
                try
                {
                    var values = csvLine.Split(',');

                    if (values.Length != 6) return null;

                    return new Customer
                    {
                        Id = int.Parse(values[0]),
                        Name = values[1],
                        IdentityNumber = values[2],
                        PhoneNumber = values[3],
                        Email = values[4],
                        Type = (CustomerType)int.Parse(values[5])
                    };
                }
                catch
                {
                    return null;
                }
            }

            private void SaveAllCustomers(List<Customer> customers)
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var lines = new List<string> { "Id,Name,IdentityNumber,PhoneNumber,Email,Type" };

                foreach (var customer in customers.OrderBy(c => c.Id))
                {
                    var line = $"{customer.Id},{customer.Name},{customer.IdentityNumber},{customer.PhoneNumber},{customer.Email},{(int)customer.Type}";
                    lines.Add(line);
                }

                File.WriteAllLines(filePath, lines);
            }
        
    }

}
