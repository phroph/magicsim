using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace magicsim
{
    public class SimC
    {
        /// <summary>
        /// The function determines whether the current operating system is a 
        /// 64-bit operating system.
        /// </summary>
        /// <returns>
        /// The function returns true if the operating system is 64-bit; 
        /// otherwise, it returns false.
        /// </returns>
        public static bool Is64BitOperatingSystem()
        {
            if (IntPtr.Size == 8)  // 64-bit programs run only on Win64
            {
                return true;
            }
            else  // 32-bit programs run on both 32-bit and 64-bit Windows
            {
                // Detect whether the current process is a 32-bit process 
                // running on a 64-bit system.
                bool flag;
                return ((DoesWin32MethodExist("kernel32.dll", "IsWow64Process") &&
                    IsWow64Process(GetCurrentProcess(), out flag)) && flag);
            }
        }

        /// <summary>
        /// The function determins whether a method exists in the export 
        /// table of a certain module.
        /// </summary>
        /// <param name="moduleName">The name of the module</param>
        /// <param name="methodName">The name of the method</param>
        /// <returns>
        /// The function returns true if the method specified by methodName 
        /// exists in the export table of the module specified by moduleName.
        /// </returns>
        static bool DoesWin32MethodExist(string moduleName, string methodName)
        {
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            if (moduleHandle == IntPtr.Zero)
            {
                return false;
            }
            return (GetProcAddress(moduleHandle, methodName) != IntPtr.Zero);
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule,
            [MarshalAs(UnmanagedType.LPStr)]string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);

        public SimC()
        {
            var filename = "simc.exe";
            var address = "https://s3.amazonaws.com/magicsim/simc.exe";

            if (!Directory.Exists("bin"))
            {
                Directory.CreateDirectory("bin");
            }
            DirectoryInfo di = new DirectoryInfo("bin");

            var request = WebRequest.CreateHttp(address);
            request.Method = "HEAD";
            var tag = request.GetResponse().Headers["ETag"].Replace("\"", "");
            if (!File.Exists(tag))
            {
                File.Create(tag).Close();
                var file = File.Create("bin" + Path.DirectorySeparatorChar + filename);
                var array = ReadFully(WebRequest.CreateHttp(address).GetResponse().GetResponseStream());
                file.Write(array, 0, array.Length);
                file.Close();
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        
        public bool RunSim(String profilePath)
        {
            ProcessStartInfo info = new ProcessStartInfo("bin\\simc.exe", profilePath);
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            try
            {
                using (var process = Process.Start(info))
                {
                    App.Job.AddProcess(process.Handle);
                    var err = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    var exitCode = process.ExitCode;
                    if (exitCode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error running simc:\n" + err, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error running simc: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }
    }
}
