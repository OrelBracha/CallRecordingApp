using System;
using System.ServiceModel;
using CallRecordingClient.ServiceReference;

namespace CallRecordingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new CallRecordingServiceClient();

            while (true)
            {
                Console.WriteLine("1. Record a call");
                Console.WriteLine("2. Get all calls");
                Console.WriteLine("3. View call details");
                Console.WriteLine("4. Update a call");
                Console.WriteLine("5. Delete a call");
                Console.WriteLine("6. Save calls to file");
                Console.WriteLine("7. Load calls from file");
                Console.WriteLine("8. Search calls by customer name");
                Console.WriteLine("9. Exit");
                Console.Write("Select an option: ");
                var option = Console.ReadLine();

                try
                {
                    switch (option)
                    {
                        case "1":
                            RecordCall(client);
                            break;
                        case "2":
                            GetAllCalls(client);
                            break;
                        case "3":
                            ViewCallDetails(client);
                            break;
                        case "4":
                            UpdateCall(client);
                            break;
                        case "5":
                            DeleteCall(client);
                            break;
                        case "6":
                            SaveCallsToFile(client);
                            break;
                        case "7":
                            LoadCallsFromFile(client);
                            break;
                        case "8":
                            SearchCallsByCustomerName(client);
                            break;
                        case "9":
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine();
            }
        }

        private static void RecordCall(CallRecordingServiceClient client)
        {
            try
            {
                Console.Write("Customer Name: ");
                var customerName = Console.ReadLine();
                Console.Write("Phone Number: ");
                var phoneNumber = Console.ReadLine();
                Console.Write("Reason: ");
                var reason = Console.ReadLine();
                Console.Write("Time of Call (yyyy-MM-dd HH:mm): ");
                var timeOfCall = DateTime.Parse(Console.ReadLine());
                Console.Write("Importance (low, medium, high): ");
                var importance = Console.ReadLine();

                var call = new CallRecord
                {
                    CustomerName = customerName,
                    PhoneNumber = phoneNumber,
                    Reason = reason,
                    TimeOfCall = timeOfCall,
                    Importance = importance
                };

                client.RecordCall(call);
                Console.WriteLine("Call recorded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while recording the call: {ex.Message}");
            }
        }

        private static void GetAllCalls(CallRecordingServiceClient client)
        {
            try
            {
                var calls = client.GetAllCalls();
                foreach (var call in calls)
                {
                    Console.WriteLine($"ID: {call.Id}, Customer: {call.CustomerName}, Phone: {call.PhoneNumber}, Reason: {call.Reason}, Time: {call.TimeOfCall}, Importance: {call.Importance}, Additional Details: {call.AdditionalDetails}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting all calls: {ex.Message}");
            }
        }

        private static void ViewCallDetails(CallRecordingServiceClient client)
        {
            try
            {
                Console.Write("Enter Call ID: ");
                var id = int.Parse(Console.ReadLine());
                var call = client.GetCallById(id);
                if (call != null)
                {
                    Console.WriteLine($"ID: {call.Id}, Customer: {call.CustomerName}, Phone: {call.PhoneNumber}, Reason: {call.Reason}, Time: {call.TimeOfCall}, Importance: {call.Importance}, Additional Details: {call.AdditionalDetails}");
                }
                else
                {
                    Console.WriteLine("Call not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while viewing the call details: {ex.Message}");
            }
        }

        private static void UpdateCall(CallRecordingServiceClient client)
        {
            try
            {
                Console.Write("Enter Call ID: ");
                var id = int.Parse(Console.ReadLine());
                Console.Write("Enter Additional Details: ");
                var additionalDetails = Console.ReadLine();
                client.UpdateCall(id, additionalDetails);
                Console.WriteLine("Call updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the call: {ex.Message}");
            }
        }

        private static void DeleteCall(CallRecordingServiceClient client)
        {
            try
            {
                Console.Write("Enter Call ID: ");
                var id = int.Parse(Console.ReadLine());
                client.DeleteCall(id);
                Console.WriteLine("Call deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the call: {ex.Message}");
            }
        }

        private static void SaveCallsToFile(CallRecordingServiceClient client)
        {
            try
            {
                client.SaveCallsToFile();
                Console.WriteLine("Calls saved to file successfully.");
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error from service: " + ex.Message);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving calls to file: {ex.Message}");
            }
        }

        private static void LoadCallsFromFile(CallRecordingServiceClient client)
        {
            try
            {
                client.LoadCallsFromFile();
                Console.WriteLine("Calls loaded from file successfully.");
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error from service: " + ex.Message);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading calls from file: {ex.Message}");
            }
        }

        private static void SearchCallsByCustomerName(CallRecordingServiceClient client)
        {
            try
            {
                Console.Write("Enter Customer Name: ");
                var customerName = Console.ReadLine();
                var calls = client.SearchCallsByCustomerName(customerName);
                foreach (var call in calls)
                {
                    Console.WriteLine($"ID: {call.Id}, Customer: {call.CustomerName}, Time: {call.TimeOfCall}, Importance: {call.Importance}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while searching calls by customer name: {ex.Message}");
            }
        }
    }
}
