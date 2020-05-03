using System.Runtime.InteropServices;

namespace UPSShare.Slave.Win32API
{
    public static class Win32API
    {
        [DllImport("PowrProf.dll", CharSet = CharSet.Unicode)]
        static extern bool GetPwrCapabilities(SYSTEM_POWER_CAPABILITIES spc);
    }
}
