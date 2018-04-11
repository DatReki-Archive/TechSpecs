using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tech_Specs
{
    class Program
    {
        static void Main(string[] args)
        {
            //Name of the program, maker and creation date
            Console.Title = "Tech specs";
            Console.WriteLine("Made by TimoHalofan  13-3-2018\n\n");

            Stats();
        }

        static PerformanceCounter cpuCounter;
        private static ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        private static ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");
        
        public static async void Stats()
        {
            try
            {
                RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

                if (processor_name != null)
                {
                    if (processor_name.GetValue("ProcessorNameString") != null)
                    {
                        //   Console.WriteLine(processor_name.GetValue("ProcessorNameString"));   //Display processor ingo.
                        // Could do stuff
                    }
                }
                PerformanceCounter pageCounter = new PerformanceCounter
                ("Paging File", "% Usage", "_Total");
                {
                    ManagementClass mc = new ManagementClass("win32_processor");
                    ManagementObjectCollection moc = mc.GetInstances();
                    String info = String.Empty;
                    foreach (ManagementObject mo in moc)
                    {
                        string name = (string)mo["Name"];
                        name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");
                        info = name; //+ ", " + (string)mo["Caption"] + ", " + (string)mo["SocketDesignation"];
                                     //mo.Properties["Name"].Value.ToString();
                    }
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                    String osinfo = String.Empty;
                    foreach (ManagementObject wmi in searcher.Get())
                    {
                        try
                        {
                            string os = ((string)wmi["Caption"]).Trim() + ", " + (string)wmi["Version"] + ", " + (string)wmi["OSArchitecture"];
                            osinfo = os;
                        }
                        catch { }
                    }
                    Process process = Process.GetCurrentProcess();

                    int MemSlots = 0;
                    ManagementScope oMs = new ManagementScope();
                    ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
                    ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
                    ManagementObjectCollection oCollection2 = oSearcher2.Get();
                    foreach (ManagementObject obj in oCollection2)
                    {
                        MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

                    }
                    string mem = MemSlots.ToString();

                    ManagementScope oMs2 = new ManagementScope();
                    ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
                    ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs2, oQuery);
                    ManagementObjectCollection oCollection = oSearcher.Get();

                    long MemSize = 0;
                    long mCap = 0;

                    // In case more than one Memory sticks are installed
                    foreach (ManagementObject obj in oCollection)
                    {
                        mCap = Convert.ToInt64(obj["Capacity"]);
                        MemSize += mCap;
                    }
                    MemSize = (MemSize / 1024) / 1024;
                    string ram = MemSize.ToString() + "MB";

                    //Getting some CPU details
                    var currProcess = Process.GetCurrentProcess();
                    cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    Console.WriteLine($"OS: {osinfo}");

                    foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration").Get())
                    {
                        foreach (PropertyData property in mo.Properties)
                        {
                            if (property.Name == "Description")
                            {
                                Console.WriteLine($"CPU: {processor_name.GetValue("ProcessorNameString")}");
                                GetCpuDetails();
                            }
                        }
                    }
                    #region Motherboard

                    Console.WriteLine($"\nRam: {ram}");
                    GetRamSlots();
                    Console.WriteLine("");
                    Console.WriteLine("Motherboard Properties:");
                    Console.WriteLine("Availability: " + MotherboardInfo.Availability);
                    Console.WriteLine("InstallDate: " + MotherboardInfo.InstallDate);
                    Console.WriteLine("Manufacturer: " + MotherboardInfo.Manufacturer);
                    Console.WriteLine("Model: " + MotherboardInfo.Model);
                    Console.WriteLine("PrimaryBusType: " + MotherboardInfo.PrimaryBusType);
                    Console.WriteLine("Product: " + MotherboardInfo.Product);
                    Console.WriteLine("SecondaryBusType: " + MotherboardInfo.SecondaryBusType);
                    Console.WriteLine("SerialNumber: " + MotherboardInfo.SerialNumber);
                    Console.WriteLine("Status: " + MotherboardInfo.Status);
                    Console.WriteLine("SystemName: " + MotherboardInfo.SystemName);
                    Console.WriteLine("Version: " + MotherboardInfo.Version);

                    #endregion
                    Console.WriteLine("");
                    //Code for getting GPU('s)
                    ManagementObjectSearcher objvide = new ManagementObjectSearcher("select * from Win32_VideoController");
                    foreach (ManagementObject obj in objvide.Get())
                    {
                        {
                            Console.WriteLine("GPU");
                            List<string> GPU = new List<string>();
                            //GPU.Add("Name  -  " + obj["Name"]);
                            Console.WriteLine($"Amount of GPUs: {objvide.Get().Count}");
                            if (objvide.Get().Count == 1)
                            {
                                //Nothing needs to be done when there is only 1 GPU
                            }
                            if (objvide.Get().Count == 2)
                            {
                                GetSecondGPUName();
                            }
                            if (objvide.Get().Count == 3)
                            {
                                GetSecondGPUName();
                            }
                            if (objvide.Get().Count == 4)
                            {
                                GetSecondGPUName();
                            }
                            if (objvide.Get().Count == 5)
                            {
                                GetSecondGPUName();
                            }
                            if (objvide.Get().Count == 6)
                            {
                                GetSecondGPUName();
                            }
                            if (objvide.Get().Count == 7)
                            {
                                GetSecondGPUName();
                            }
                            GPU.Add("DeviceID: " + obj["DeviceID"]);
                            GPU.Add("AdapterDACType: " + obj["AdapterDACType"]);
                            GPU.Add("DriverVersion: " + obj["DriverVersion"]);
                            GPU.Add("VideoProcessor: " + obj["VideoProcessor"]);
                            GPU.Add("VideoArchitecture: " + obj["VideoArchitecture"]);
                            GPU.Add("VideoMemoryType: " + obj["VideoMemoryType"]);
                            
                            // Loop over strings.
                            foreach (string s in GPU)
                            {
                                Console.WriteLine(s);
                            };
                            Console.WriteLine("\n\n");
                        }
                        Console.WriteLine("Do you want too see the hard drives too? [yes/no]");
                        string option = Console.ReadLine();

                        if (option == "yes")
                        {
                            // Hard drives
                            DriveInfo[] allDrives = DriveInfo.GetDrives();
                            foreach (DriveInfo d in allDrives)
                            {
                                if (d.IsReady == true)
                                {
                                    Console.WriteLine("Hard Drives");
                                    Console.WriteLine("Total available space:          {0, 15}", SizeSuffix(d.TotalFreeSpace));
                                    Console.WriteLine("Total size of drive:            {0, 15} ", SizeSuffix(d.TotalSize));
                                    //Console.Write("Root directory:                 {0, 12}", d.RootDirectory);
                                    //Decided not to use root directories but if you want to have it feel free to remove the //'s
                                }
                            }
                        }
                        Console.WriteLine("Do you want too see extra specs too? [yes/no]");
                        string option2 = Console.ReadLine();
                        if(option2 == "yes")
                        {
                            //Shows the amount of monitors connected to the PC
                            String queryString = "Select * from Win32_DesktopMonitor";
                            ManagementObjectSearcher monitor = new ManagementObjectSearcher(queryString);
                            Console.WriteLine("Monitors: " + monitor.Get().Count.ToString());
                        }
                        Console.WriteLine("\n\nDone - Press any button to exit");
                        Console.ReadKey();
                        await Task.Delay(-1);
                    }
                }
            }
            catch (Exception er)
            {
                Console.WriteLine("Something went wrong!" + er);
                //Catching stuff just to make sure
            }
        }
        private static string ConvertToDateTime(string unconvertedTime)
        {
            string convertedTime = "";
            int year = int.Parse(unconvertedTime.Substring(0, 4));
            int month = int.Parse(unconvertedTime.Substring(4, 2));
            int date = int.Parse(unconvertedTime.Substring(6, 2));
            int hours = int.Parse(unconvertedTime.Substring(8, 2));
            int minutes = int.Parse(unconvertedTime.Substring(10, 2));
            int seconds = int.Parse(unconvertedTime.Substring(12, 2));
            string meridian = "AM";
            if (hours > 12)
            {
                hours -= 12;
                meridian = "PM";
            }
            convertedTime = date.ToString() + "/" + month.ToString() + "/" + year.ToString() + " " +
            hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString() + " " + meridian;
            return convertedTime;
        }
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        private static void GetCpuDetails()
        {
            //Getting even more CPU specs like the amount of cores and the architecture
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                Console.WriteLine("Number Of Physical Processors: {0} ", item["NumberOfProcessors"]);
                Console.WriteLine("Number Of Logical Processors: {0} ", item["NumberOfLogicalProcessors"]);

                var numberOfCores = 0;
                foreach (var item2 in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                {
                    numberOfCores += int.Parse(item2["NumberOfCores"].ToString());
                    Console.WriteLine("Number Of Cores: {0}", numberOfCores);
                    Console.WriteLine("Architecture: {0}", GetArchitectureDetail(int.Parse(item2["Architecture"].ToString())));
                }
            }
        }
        private static void GetSecondGPUName()
        {
            try
            {
                //Idk why but for some reason this allowes you too see more than one GPU which is a known issue with using win32_VideoController that you cannot easily get mutiple GPU's
                ManagementObjectSearcher objvide = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (ManagementObject obj in objvide.Get())
                {
                    {
                        List<string> GPU = new List<string>();
                        GPU.Add("Name: " + obj["Name"]);

                        // Loop over strings.
                        foreach (string s in GPU)
                        {
                            Console.WriteLine(s);
                        };
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //Gotta catch them all
            }
        }
        private static void GetRamSlots()
        {
            //Btw if you have a dual socket motherboard and only have 1 CPU in it it will not detect all ram slots
            int MemSlots = 0;
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
            ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
            ManagementObjectCollection oCollection2 = oSearcher2.Get();
            foreach (ManagementObject obj in oCollection2)
            {
                MemSlots = Convert.ToInt32(obj["MemoryDevices"]);
            }
            string mem = MemSlots.ToString();
            Console.WriteLine($"Ram slots {mem}");
        }
        private static string GetArchitectureDetail(int architectureNumber)
        {
            switch (architectureNumber)
            {
                case 0: return "x86";
                case 1: return "MIPS";
                case 2: return "Alpha";
                case 3: return "PowerPC";
                case 6: return "Itanium-based systems";
                case 9: return "x64";
                default:
                    return "Unkown";
            }
        }
    }
}
