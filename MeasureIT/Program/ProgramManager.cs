using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureIT
{
    public class ProgramManager
    {
        private static string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public static List<Program> GetListOfInstalledPrograms()
        {
            List<Program> installedPrograms = new List<Program>();

            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        string name = (string) subkey.GetValue("DisplayName");

                        installedPrograms.Add(new Program());
                    }
                }
            }
            return installedPrograms;
            


        }


    }
}
