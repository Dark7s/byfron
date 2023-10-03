using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByfronWoofer
{
    internal class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);
        [DllImport("user32.dll", EntryPoint = "BlockInput")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        static void Main(string[] args)
        {
        Main:
            Console.Title = "Byfron Spoofer";
            Console.WriteLine("Byfron spoofer");
            Console.WriteLine("[F1] Method 1 (TEMP) [IDF/DISK]");
            Console.WriteLine("[F2] Method 2 (PERM) [SIDCHG]");
            while (true)
            {
                var w = Console.ReadKey(true);
                if (w.Key == ConsoleKey.F1) {
                    IdfSpoof.Init();
                    utils.ByteArrayToFile("C:\\Windows\\IME\\DiskSpoof.exe", bytes.tmpdriver);
                    Process.Start("C:\\Windows\\IME\\DiskSpoof.exe");
                    Console.Clear();
                    goto Main;
                } else { if(w.Key == ConsoleKey.F2) {
                            Console.Write("This method will permanently change your SID, do you want to continue?(y/n)");
                            while (true)
                            {
                                var wyn = Console.ReadKey(true);
                                if (wyn.Key == ConsoleKey.Y)
                                {
                                    var path = @"SOFTWARE\Microsoft\Windows Defender\Real-Time Protection";
                                    var key = "DisableRealtimeMonitoring";
                                    var regkey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                                    if (regkey != null)
                                    {
                                        var subkey = regkey.OpenSubKey(path);
                                        var val = subkey.GetValue(key);
                                        if (val != null && val is Int32)
                                        {
                                            var value = (int)val;
                                            if (value == 1)
                                            {
                                                IdfSpoof.Init();
                                                SYSTEMTIME st = new SYSTEMTIME();
                                                st.wYear = 2023; // must be short
                                                st.wMonth = 9;
                                                st.wDay = 25;
                                                st.wHour = 1;
                                                st.wMinute = 23;
                                                st.wSecond = 50;
 
                                            SetSystemTime(ref st);
                                            //sidchg64-3.0h.exe /R /F /FS /KEY:7H5YE-bNEvJ-dXeFL-2J
                                            utils.ByteArrayToFile("C:\\Windows\\IME\\sidchg.exe", bytes.sidchg);
                                            Process.Start("C:\\Windows\\IME\\sidchg.exe", "/R /F /FS /KEY:7H5YE-bNEvJ-dXeFL-2J");
                                            BlockInput(true);
                                            Console.Clear();
                                            MessageBox.Show("DONT TOUCH YOUR PC UNTIL IT TURNED OFF.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Please make sure real time protection is off.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Please make sure real time protection is off.");
                                        }
                                    }
                                    else
                                    {
                                        Console.Write("CORRUPTED WINDOWS!");
                                    }
                                }
                                else
                                {
                                    if (wyn.Key == ConsoleKey.N)
                                    {
                                        Console.Clear();
                                        goto Main;
                                    }
                                }
                            } }
                  }
                }
            }
        }
    }
