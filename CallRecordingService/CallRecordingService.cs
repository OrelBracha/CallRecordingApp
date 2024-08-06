using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;

namespace CallRecordingService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CallRecordingService : ICallRecordingService
    {
        private readonly List<CallRecord> _callRecords = new List<CallRecord>();
        private int _nextId = 1;
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "calls.txt");

        public void RecordCall(CallRecord call)
        {
            if (call == null)
                throw new ArgumentNullException(nameof(call));

            lock (_callRecords)
            {
                call.Id = _nextId++;
                _callRecords.Add(call);
            }
        }

        public List<CallRecord> GetAllCalls()
        {
            lock (_callRecords)
            {
                return _callRecords.ToList();
            }
        }

        public CallRecord GetCallById(int id)
        {
            lock (_callRecords)
            {
                return _callRecords.FirstOrDefault(c => c.Id == id);
            }
        }

        public void UpdateCall(int id, string additionalDetails)
        {
            lock (_callRecords)
            {
                var call = _callRecords.FirstOrDefault(c => c.Id == id);
                if (call != null)
                {
                    call.AdditionalDetails = additionalDetails;
                }
            }
        }

        public void DeleteCall(int id)
        {
            lock (_callRecords)
            {
                var call = _callRecords.FirstOrDefault(c => c.Id == id);
                if (call != null)
                {
                    _callRecords.Remove(call);
                }
            }
        }

        public void SaveCallsToFile()
        {
            lock (_callRecords)
            {
                try
                {
                    // Create or overwrite the file at the specified path
                    using (var writer = new StreamWriter(_filePath))
                    {
                        Console.WriteLine($"Attempting to save file to: {_filePath}");

                        // Write each CallRecord as a formatted string
                        foreach (var record in _callRecords)
                        {
                            writer.WriteLine(record.ToString()); // Call ToString() method for human-readable format
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving file: {ex.Message}");
                }
            }
        }

        public void LoadCallsFromFile()
        {
            lock (_callRecords)
            {
                try
                {
                    using (var reader = new StreamReader(_filePath))
                    {
                        Console.WriteLine($"Attempting to load file from: {_filePath}");

                        _callRecords.Clear(); // Clear existing records to avoid duplicates

                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var record = ParseCallRecord(line);
                            if (record != null)
                            {
                                _callRecords.Add(record);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading file: {ex.Message}");
                }
            }
        }

        private CallRecord ParseCallRecord(string line)
        {
            var parts = line.Split(new[] { ',' }, 7);
            if (parts.Length == 7)
            {
                return new CallRecord
                {
                    Id = int.Parse(parts[0].Trim()),
                    CustomerName = parts[1].Trim(),
                    PhoneNumber = parts[2].Trim(),
                    Reason = parts[3].Trim(),
                    TimeOfCall = DateTime.Parse(parts[4].Trim()),
                    Importance = parts[5].Trim(),
                    AdditionalDetails = parts[6].Trim()
                };
            }
            return null;
        }

        public List<CallRecord> SearchCallsByCustomerName(string customerName)
        {
            lock (_callRecords)
            {
                return _callRecords
                .Where(c => c.CustomerName.IndexOf(customerName, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
            }
        }
    }
}
