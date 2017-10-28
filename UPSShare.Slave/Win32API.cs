using System.Runtime.InteropServices;

namespace UPSShare.Slave
{
    public enum SYSTEM_POWER_STATE
    {
        PowerSystemUnspecified = 0,
        PowerSystemWorking = 1,
        PowerSystemSleeping1 = 2,
        PowerSystemSleeping2 = 3,
        PowerSystemSleeping3 = 4,
        PowerSystemHibernate = 5,
        PowerSystemShutdown = 6,
        PowerSystemMaximum = 7
    }

    public struct BATTERY_REPORTING_SCALE
    {
       public ulong Granularity;
       public ulong Capacity;
    }

    public unsafe struct SYSTEM_POWER_CAPABILITIES
    {
        public bool PowerButtonPresent;
        public bool SleepButtonPresent;
        public bool LidPresent;
        public bool SystemS1;
        public bool SystemS2;
        public bool SystemS3;
        public bool SystemS4;
        public bool SystemS5;
        public bool HiberFilePresent;
        public bool FullWake;
        public bool VideoDimPresent;
        public bool ApmPresent;
        public bool UpsPresent;
        public bool ThermalControl;
        public bool ProcessorThrottle;
        public byte ProcessorMinThrottle;
        public byte ProcessorMaxThrottle;
        public bool FastSystemS4;
        public bool HiberBoot;
        public bool WakeAlarmPresent;
        public bool AoAc;
        public bool DiskSpinDown;
        public fixed byte spare3[8];
        public bool SystemBatteriesPresent;
        public bool BatteriesAreShortTerm;
        public BATTERY_REPORTING_SCALE BatteryScale1;
        public BATTERY_REPORTING_SCALE BatteryScale2;
        public BATTERY_REPORTING_SCALE BatteryScale3;
        public SYSTEM_POWER_STATE AcOnLineWake;
        public SYSTEM_POWER_STATE SoftLidWake;
        public SYSTEM_POWER_STATE RtcWake;
        public SYSTEM_POWER_STATE MinDeviceWakeState;
        public SYSTEM_POWER_STATE DefaultLowLatencyWake;
    }

    public class Win32API
    {
        [DllImport("PowrProf.dll", CharSet = CharSet.Unicode)]
        extern static bool GetPwrCapabilities(SYSTEM_POWER_CAPABILITIES spc);
    }
}
