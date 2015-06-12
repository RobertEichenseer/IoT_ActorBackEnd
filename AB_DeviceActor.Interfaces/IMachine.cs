using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using AB_DeviceActor.Interfaces.Dto;

namespace AB.Interfaces
{
    public interface IMachine : IActor
    {
        Task<bool> IsErrorOrAbnormalityIngest(MachineTelemetry deviceTelemetry);
        Task<MachineHealth> GetCurrentHealthAsync();
    }
}
