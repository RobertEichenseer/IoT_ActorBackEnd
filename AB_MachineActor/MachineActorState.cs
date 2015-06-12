using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace AB_MachineActor
{
    [DataContract]
    public class MachineActorState
    {
        [DataMember]
        public List<string> SuspiciousIngests { get; set; }

        [DataMember]
        public long ingestCount { get; set; }


        public MachineActorState()
        {
            SuspiciousIngests = new List<string>();
            ingestCount = 0; 
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Suspicious Devices: {0}", String.Join("-",SuspiciousIngests));
        }
    }
}