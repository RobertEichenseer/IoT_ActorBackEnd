using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB_DeviceActor.Interfaces.Dto
{
    public class MachineHealth
    {
        public long MachineId { get; set; }
        public List<string> SuspiciousIngests { get; set; }
    }
}
