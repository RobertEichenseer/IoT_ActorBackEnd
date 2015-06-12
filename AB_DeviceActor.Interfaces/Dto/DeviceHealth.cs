using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB.Interfaces
{
    public class DeviceHealth
    {
        public string FailureState { get; set; }
        public int IngestCount { get; set; }
        public DateTime? LastIngest { get; set; }
        public long MachineId { get; set; }
    }
}
