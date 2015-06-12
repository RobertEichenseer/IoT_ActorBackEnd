using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace AB_DeviceActor
{
    [DataContract]
    public class DeviceState
    {
        [DataMember]
        public string FailureState { get; set; }
        [DataMember]
        public int IngestCount { get; set; }

        [DataMember]
        public DateTime? LastIngest { get; set; }

        [DataMember]
        public long MachineId { get; set; }

        public DeviceState()
        {
            MachineId = -1;
            LastIngest = null;
            IngestCount = 0;
            FailureState = String.Empty; 
        }

        public override string ToString()
        {
            string format = "DeviceState: {0} - {1} - {2}";
            return string.Format(CultureInfo.InvariantCulture, format, FailureState, IngestCount, LastIngest);
        }
    }
}