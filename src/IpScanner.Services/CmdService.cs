using IpScanner.Services.Abstract;
using System.Diagnostics;

namespace IpScanner.Services
{
    public class CmdService : ICmdService
    {
        public void Execute(string command)
        {
            Process.Start("cmd.exe", $"/k {command}");
        }
    }
}
