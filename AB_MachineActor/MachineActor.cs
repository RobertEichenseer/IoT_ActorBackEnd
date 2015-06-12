using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;
using AB.Interfaces;
using AB_DeviceActor.Interfaces.Dto;

namespace AB_MachineActor
{
    public class MachineActor : Actor<MachineActorState>, IMachine
    {

        private IActorTimer _cleanSuspiciousIngestTimer;

        public override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                this.State = new MachineActorState() {   };
            }

            _cleanSuspiciousIngestTimer = RegisterTimer(
                CleanSuspiciousIngestList,
                null,
                TimeSpan.FromSeconds(30),
                TimeSpan.FromSeconds(30));
            
            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }

        public override Task OnDeactivateAsync()
        {
            if (_cleanSuspiciousIngestTimer != null)
                UnregisterTimer(_cleanSuspiciousIngestTimer);

            return base.OnDeactivateAsync();
        }

        private async Task CleanSuspiciousIngestList(object arg)
        {
            this.State.SuspiciousIngests.Clear();

            return ; 
        }

        public Task<MachineHealth> GetCurrentHealthAsync()
        {
            return Task.FromResult<MachineHealth>(new MachineHealth()
            {
                MachineId = this.GetActorId().GetLongId(),
                SuspiciousIngests = this.State.SuspiciousIngests,
                
            });
        }

        public Task<bool> IsErrorOrAbnormalityIngest(MachineTelemetry deviceTelemetry)
        {
            //Todo: Reach Out to Azure ML and check for outliers
            //In this case every fifth ingest or ingests with temperature > 45 are treated as suspicious/errors

            this.State.ingestCount++; 

            if (deviceTelemetry.Temperature > 45 || this.State.ingestCount % 5 == 0)
            {
                this.State.SuspiciousIngests.Add(String.Format("{0}-{1}", deviceTelemetry.DeviceId, deviceTelemetry.Temperature)); 
                return Task.FromResult<bool>(true);
            }
            
            return Task.FromResult<bool>(false);

        }
        
    }
}
