using Microsoft.Win32;
using System.IO;
using System;

namespace ByfronWoofer
{
    internal class IdfSpoof
    {
        public static bool Init()
        {
            try
            {
                SpoofProductID();
                Console.WriteLine("Spoofed ProdID");
                SpoofProfileGUID();
                Console.WriteLine("Spoofed PGUID");
                SpoofMachineID();
                Console.WriteLine("Spoofed MID");
                SpoofMachineGUID();
                Console.WriteLine("Spoofed MGUID");
                SpoofInstallTime();
                Console.WriteLine("Spoofed Installtime");
                HideSMBios();
                Console.WriteLine("SMBios hidden.");
                FlushDNS();
                Console.WriteLine("Flushed dns.");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static void SpoofProductID()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", true);
            registryKey.SetValue("ProductID", $"{utils.RandomNumberString(5)}-{utils.RandomNumberString(5)}-{utils.RandomNumberString(5)}-{utils.RandomString(5)}");
            registryKey.Close();
        }

        private static void HideSMBios()
        {
            utils.RunAsProcess("reg add HKLM\\SYSTEM\\CurrentControlSet\\Control\\WMI\\Restrictions /F");
            utils.RunAsProcess("reg add HKLM\\SYSTEM\\CurrentControlSet\\Control\\WMI\\Restrictions /v HideMachine /t REG_DWORD /d 1 /F");
            utils.RunAsProcess("taskkill /F /IM WmiPrvSE.exe");
        }

        private static void FlushDNS()
        {
            utils.RunAsProcess("ipconfig /release");
            utils.RunAsProcess("ipconfig /flushdns");
            utils.RunAsProcess("ipconfig /renew");
            utils.RunAsProcess("ipconfig /flushdns");
            utils.RunAsProcess("ping localhost -n 3 >nul");
        }

        private static void SpoofMachineID()
        {
            ;
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\SQMClient", true);
            registryKey.SetValue("MachineId", "{" + Guid.NewGuid().ToString().ToUpper() + "}");
        }

        private static void SpoofMachineGUID()
        {
            string value = Guid.NewGuid().ToString();
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Cryptography", true);
            registryKey.SetValue("MachineGuid", value);
        }

        private static void SpoofProfileGUID()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\IDConfigDB\\Hardware Profiles\\0001", true);
            registryKey.SetValue("HwProfileGUID", "{" + Guid.NewGuid().ToString() + "}");
        }

        private static void SpoofInstallTime()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", true);
            long unixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            registryKey.SetValue("InstallTime", unixTime);
            registryKey.SetValue("InstallDate", (int)unixTime);
            registryKey.Close();
        }
    }
}