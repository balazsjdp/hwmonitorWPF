using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWMonitorWPF
{
    class MonitorProcess
    {
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }


        public struct SensorInfo
        {
            public string sensorName { get; set; }
            public float? sensorValue { get; set; }
        }

        public static SensorInfo[] CPUTemp(Computer computer, UpdateVisitor updateVisitor)
        {
            computer.Open();
            SensorInfo[] sensorInfos = new SensorInfo[4];
            computer.Accept(updateVisitor);


            foreach (IHardware hardware in computer.Hardware)
            {

                if (hardware.HardwareType == HardwareType.CPU)
                {

                    int coreCounter = 0;
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (coreCounter == sensorInfos.Length)
                                return sensorInfos;


                            SensorInfo coreInfo = new SensorInfo();
                            coreInfo.sensorName = sensor.Name.ToString();
                            coreInfo.sensorValue = sensor.Value;

                            sensorInfos[coreCounter] = coreInfo;

                            coreCounter++;
                        }
                    }
                }

            }
            computer.Close();
            return sensorInfos;
        }
    }
}
