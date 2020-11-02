using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenHardwareMonitor.Hardware;
using static HWMonitorWPF.MonitorProcess;

namespace HWMonitorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Show();

            Computer computer = new Computer();
            computer.CPUEnabled = true;

            RunMonitoring();

            async void RunMonitoring()
            {
                while (true)
                {
                    await Task.Run(() =>
                    {
                        UpdateVisitor updateVisitor = new UpdateVisitor();
                        MonitorProcess.SensorInfo[] cputTemps = MonitorProcess.CPUTemp(computer, updateVisitor);
                      
                        foreach (SensorInfo coreInfo in cputTemps)
                        {
                            Console.WriteLine(coreInfo.sensorName + " - " + coreInfo.sensorValue + "C°");
                            this.Dispatcher.Invoke(() => {

                                ProgressBar bar_1 = (ProgressBar)this.FindName("core_1_bar");
                                ProgressBar bar_2 = (ProgressBar)this.FindName("core_2_bar");
                                ProgressBar bar_3 = (ProgressBar)this.FindName("core_3_bar");
                                ProgressBar bar_4 = (ProgressBar)this.FindName("core_4_bar");
                                ProgressBar bar_5 = (ProgressBar)this.FindName("core_5_bar");
                                ProgressBar bar_6 = (ProgressBar)this.FindName("core_6_bar");

                                bar_5.Value = 0;
                                bar_6.Value = 0;


                                switch (coreInfo.sensorName)
                                {
                                    case "CPU Core #1":
                                        bar_1.Value = Convert.ToDouble(coreInfo.sensorValue);
                                        ColorBar(bar_1, coreInfo.sensorValue);
                                        break;
                                    case "CPU Core #2":
                                        bar_2.Value = Convert.ToDouble(coreInfo.sensorValue);
                                        ColorBar(bar_2, coreInfo.sensorValue);
                                        break;
                                    case "CPU Core #3":
                                        bar_3.Value = Convert.ToDouble(coreInfo.sensorValue);
                                        ColorBar(bar_3, coreInfo.sensorValue);
                                        break;
                                    case "CPU Core #4":
                                        bar_4.Value = Convert.ToDouble(coreInfo.sensorValue);
                                        ColorBar(bar_4, coreInfo.sensorValue);
                                        break;
                                    case "CPU Core #5":
                                        bar_5.Value = Convert.ToDouble(coreInfo.sensorValue);
                                        ColorBar(bar_5, coreInfo.sensorValue);
                                        break;
                                    case "CPU Core #6":
                                        bar_6.Value = Convert.ToDouble(coreInfo.sensorValue);
                                        ColorBar(bar_6, coreInfo.sensorValue);
                                        break;
                                }

                            });

                        }

                    });
                    //Thread.Sleep(100);
                }
            }

            

            void ColorBar(ProgressBar bar, float? value)
            {
                if(bar.Value >= 80)
                {
                    bar.Foreground = Brushes.Red;
                }else if(bar.Value >= 70)
                {
                    bar.Foreground = Brushes.Orange;
                }
                else
                {
                    bar.Foreground = Brushes.Green;
                }
            }
           


           
          

            

            
        }
    }
}
