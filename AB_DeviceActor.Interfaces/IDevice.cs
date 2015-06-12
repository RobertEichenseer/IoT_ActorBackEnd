using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace AB.Interfaces
{
    public interface IDevice : IActor
    {
        Task<bool> IngestTelemetryAsync(DeviceTelemetry deviceTelemetry);
        Task<DeviceHealth> GetCurrentHealthAsync();
    }
}
