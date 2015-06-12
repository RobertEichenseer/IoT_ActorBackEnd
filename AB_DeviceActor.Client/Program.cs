using AB.Interfaces;
using AB_DeviceActor.Interfaces.Dto;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB_DeviceActor.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BackEndCommunication backEndCommunication = new BackEndCommunication();

            var task = backEndCommunication.IngestTelemetry();
            task.Wait();

            Console.ReadLine();
        }
    }

    public class BackEndCommunication
    {
        public async Task IngestTelemetry()
        {
            IDevice deviceProxy = ActorProxy.Create<IDevice>(ActorId.NewId(), "fabric:/ActorBackendApplication");

            bool ingestResult;
            DeviceHealth deviceHealth;
            long machineId = -1;

            //Ingest telemetry
            for (int i = 0; i < 50; i++)
            {
                ingestResult = await deviceProxy.IngestTelemetryAsync(new DeviceTelemetry()
                {
                    DeviceId = 10,
                    Humidity = 80,
                    Pollution = 70,
                    Temperature = 20.5F + i,
                });
                Console.WriteLine(String.Format("Ingest Result: {0}", ingestResult));

                deviceHealth  = await deviceProxy.GetCurrentHealthAsync();
                machineId = deviceHealth.MachineId; 
                Console.WriteLine(String.Format("Failure State {0} - {1} - {2}", deviceHealth.FailureState, deviceHealth.LastIngest, deviceHealth.MachineId));
                Console.WriteLine();
            }

            //Suspicious Ingests
            IMachine machineProxy = ActorProxy.Create<IMachine>(new ActorId(machineId), "fabric:/ActorBackendApplication");
            MachineHealth machineHealth = await machineProxy.GetCurrentHealthAsync();
            Console.WriteLine(String.Format("{0}", String.Join('\n'.ToString(), machineHealth.SuspiciousIngests)));

        }
    }
}
