
using BankManagement.Enums;
using BankManagement.Models;
using BankManagement.Services;

namespace BankCustomerManagement
{
    class Program
    {
        private static CustomerService _customerService = new CustomerService();

        static void Main(string[] args)
        {
            Console.WriteLine("🏦 Bank Customer Management System 🏦");
            BankManage();
        }

        static void BankManage()
        {
            while (true)
            {
                Console.WriteLine("1) View All Customers");
                Console.WriteLine("2) Find Customer by ID");
                Console.WriteLine("3) Add New Customer");
                Console.WriteLine("4) Update Customer");
                Console.WriteLine("5) Delete Customer");
                Console.WriteLine("6) Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ViewAllCustomers();
                            break;
                        case "2":
                            FindCustomerById();
                            break;
                        case "3":
                            AddNewCustomer();
                            break;
                        case "4":
                            UpdateCustomer();
                            break;
                        case "5":
                            DeleteCustomer();
                            break;
                        case "6":
                            return;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void ViewAllCustomers()
        {
            var customers = _customerService.GetAllCustomers();

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            foreach (var customer in customers)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine($"\nTotal customers: {customers.Count}");
        }

        static void FindCustomerById()
        {
            Console.Write("\nEnter Customer ID: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = _customerService.GetSingleCustomer(customerId);
                if (customer != null)
                {
                    Console.WriteLine(customer);
                }
                else
                {
                    Console.WriteLine($"Customer with ID {customerId} not found.");
                }
            }
            else
            {
                Console.WriteLine(" Invalid ID format.");
            }
        }

        static void AddNewCustomer()
        {

            var customer = new Customer();

            Console.Write("Enter Name: ");
            customer.Name = Console.ReadLine() ?? "";

            Console.Write("Enter Identity Number: ");
            customer.IdentityNumber = Console.ReadLine() ?? "";

            Console.Write("Enter Phone Number: ");
            customer.PhoneNumber = Console.ReadLine() ?? "";

            Console.Write("Enter Email: ");
            customer.Email = Console.ReadLine() ?? "";

            Console.Write("Enter Type - 0 for Individual, 1 for Company: ");
            if (Enum.TryParse<CustomerType>(Console.ReadLine(), out var type))
            {
                customer.Type = type;
            }
            else
            {
                Console.WriteLine(" Invalid type");
                customer.Type = CustomerType.Individual;
            }

            try
            {
                var newId = _customerService.AddCustomer(customer);
                Console.WriteLine($" Customer added successfully with ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Failed to add customer: {ex.Message}");
            }
        }

        static void UpdateCustomer()
        {
            Console.Write("\nEnter Customer ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine(" Invalid ID format.");
                return;
            }

            var existingCustomer = _customerService.GetSingleCustomer(customerId);
            if (existingCustomer == null)
            {
                Console.WriteLine($"Customer with ID {customerId} not found.");
                return;
            }

            Console.WriteLine($"\nCurrent data: {existingCustomer}");

            var updatedCustomer = new Customer { Id = customerId };

            Console.Write($"Enter Name ({existingCustomer.Name}): ");
            updatedCustomer.Name = Console.ReadLine() ?? existingCustomer.Name;

            Console.Write($"Enter Identity Number ({existingCustomer.IdentityNumber}): ");
            updatedCustomer.IdentityNumber = Console.ReadLine() ?? existingCustomer.IdentityNumber;

            Console.Write($"Enter Phone Number ({existingCustomer.PhoneNumber}): ");
            updatedCustomer.PhoneNumber = Console.ReadLine() ?? existingCustomer.PhoneNumber;

            Console.Write($"Enter Email ({existingCustomer.Email}): ");
            updatedCustomer.Email = Console.ReadLine() ?? existingCustomer.Email;

            Console.Write($"Enter Type - 0 for Individual, 1 for Company (current: {(int)existingCustomer.Type}): ");
            if (Enum.TryParse<CustomerType>(Console.ReadLine(), out var type))
            {
                updatedCustomer.Type = type;
            }
            else
            {
                updatedCustomer.Type = existingCustomer.Type;
            }

            try
            {
                var updatedId = _customerService.UpdateCustomer(updatedCustomer);
                Console.WriteLine($" Customer with ID {updatedId} updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Failed to update customer: {ex.Message}");
            }
        }

        static void DeleteCustomer()
        {
            Console.Write("\nEnter Customer ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine(" Invalid ID format.");
                return;
            }

            var customer = _customerService.GetSingleCustomer(customerId);
            if (customer == null)
            {
                Console.WriteLine($"Customer with ID {customerId} not found.");
                return;
            }

            Console.WriteLine($"\nCustomer to delete: {customer}");
            Console.Write("Are you sure you want to delete this customer? (y/N): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "y" || confirmation == "yes")
            {
                try
                {
                    var deletedId = _customerService.DeleteCustomer(customerId);
                    Console.WriteLine($" Customer with ID {deletedId} deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Failed to delete customer: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Deletion stopped.");
            }
        }
    }
}