using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CallRecordingService
{
    [ServiceContract]
    public interface ICallRecordingService
    {
        [OperationContract]
        void RecordCall(CallRecord call);

        [OperationContract]
        List<CallRecord> GetAllCalls();

        [OperationContract]
        CallRecord GetCallById(int id);

        [OperationContract]
        void UpdateCall(int id, string additionalDetails);

        [OperationContract]
        void DeleteCall(int id);

        [OperationContract]
        void SaveCallsToFile();

        [OperationContract]
        void LoadCallsFromFile();

        [OperationContract]
        List<CallRecord> SearchCallsByCustomerName(string customerName);
    }

    [Serializable]
    [DataContract]
    public class CallRecord
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public DateTime TimeOfCall { get; set; }

        [DataMember]
        public string Importance { get; set; }

        [DataMember]
        public string AdditionalDetails { get; set; }

        public override string ToString()
        {
            return $"{Id}, {CustomerName}, {PhoneNumber}, {Reason}, {TimeOfCall}, {Importance}, {AdditionalDetails}";
        }
    }
}
