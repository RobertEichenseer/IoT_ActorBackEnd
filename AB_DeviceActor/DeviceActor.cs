using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;
using AB.Interfaces;

namespace AB_DeviceActor
{
    public class DeviceActor : Actor<DeviceState>, IDevice
    {
        public override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                this.State = new DeviceState() {
                    FailureState = "Initialized",
                    LastIngest = null,
                    IngestCount = 0
                };
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }
        
        public async Task<bool> IngestTelemetryAsync(DeviceTelemetry deviceTelemetry)
        {
            this.State.LastIngest = DateTime.Now;
            this.State.IngestCount++;

            if (this.State.MachineId == -1)
                this.State.MachineId = GetMachineId(deviceTelemetry.DeviceId);

            if (deviceTelemetry.Pollution > 50 && deviceTelemetry.Temperature >= 25)
            { 
                IMachine machineProxy = ActorProxy.Create<IMachine>(new ActorId(this.State.MachineId), "fabric:/ActorBackendApplication");
                bool reportTask = await machineProxy.IsErrorOrAbnormalityIngest(new MachineTelemetry() {
                    DeviceId = deviceTelemetry.DeviceId, 
                    Pollution = deviceTelemetry.Pollution,
                    Temperature = deviceTelemetry.Temperature,
                });
                if (reportTask)
                {
                    this.State.FailureState = "Error; Ingest > Threshold";
                    return false;
                }
            }

            this.State.FailureState = "No failure State; at the moment ;-)"; 
            return true;
        }

        [Readonly]
        public Task<DeviceHealth> GetCurrentHealthAsync()
        {
            return Task.FromResult<DeviceHealth>(new DeviceHealth()
            {
                FailureState = this.State.FailureState,
                IngestCount = this.State.IngestCount,
                LastIngest = this.State.LastIngest,
                MachineId = this.State.MachineId,
            }); 
        }

        private long GetMachineId(int deviceId)
        {
           
            if (deviceId < 100)
                return 1001;

            return 2002;
        }
    }
}
