using System;

namespace UPSShare.Slave.Win32API
{
    public unsafe struct SYSTEM_POWER_CAPABILITIES : IEquatable<SYSTEM_POWER_CAPABILITIES>
    {
        public const int Spare3Length = 8;

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
        public fixed byte spare3[Spare3Length];
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

        public override bool Equals(object obj)
        {
            var other = (SYSTEM_POWER_CAPABILITIES) obj;
            return other != default && Equals(other);
        }

        public override int GetHashCode()
        {
            var Spare3HashCode = spare3[0];
            for (var i = 1; i < Spare3Length; i++) {
                Spare3HashCode |= spare3[i];
            }
            return Spare3HashCode |
                   PowerButtonPresent.GetHashCode() |
                   SleepButtonPresent.GetHashCode() |
                   LidPresent.GetHashCode() |
                   SystemS1.GetHashCode() |
                   SystemS2.GetHashCode() |
                   SystemS3.GetHashCode() |
                   SystemS4.GetHashCode() |
                   SystemS5.GetHashCode() |
                   HiberFilePresent.GetHashCode() |
                   FullWake.GetHashCode() |
                   VideoDimPresent.GetHashCode() |
                   ApmPresent.GetHashCode() |
                   UpsPresent.GetHashCode() |
                   ThermalControl.GetHashCode() |
                   ProcessorThrottle.GetHashCode() |
                   ProcessorMinThrottle.GetHashCode() |
                   ProcessorMaxThrottle.GetHashCode() |
                   FastSystemS4.GetHashCode() |
                   HiberBoot.GetHashCode() |
                   WakeAlarmPresent.GetHashCode() |
                   AoAc.GetHashCode() |
                   DiskSpinDown.GetHashCode() |
                   SystemBatteriesPresent.GetHashCode() |
                   BatteriesAreShortTerm.GetHashCode() |
                   BatteryScale1.GetHashCode() |
                   BatteryScale2.GetHashCode() |
                   BatteryScale3.GetHashCode() |
                   AcOnLineWake.GetHashCode() |
                   SoftLidWake.GetHashCode() |
                   RtcWake.GetHashCode() |
                   MinDeviceWakeState.GetHashCode() |
                   DefaultLowLatencyWake.GetHashCode();
        }

        public bool Equals(SYSTEM_POWER_CAPABILITIES other)
        {
            for (var i = 0; i < Spare3Length; i++) {
                if (spare3[i] != other.spare3[i]) {
                    return false;
                }
            }
            return PowerButtonPresent == other.PowerButtonPresent &&
                   SleepButtonPresent == other.SleepButtonPresent &&
                   LidPresent == other.LidPresent &&
                   SystemS1 == other.SystemS1 &&
                   SystemS2 == other.SystemS2 &&
                   SystemS3 == other.SystemS3 &&
                   SystemS4 == other.SystemS4 &&
                   SystemS5 == other.SystemS5 &&
                   HiberFilePresent == other.HiberFilePresent &&
                   FullWake == other.FullWake &&
                   VideoDimPresent == other.VideoDimPresent &&
                   ApmPresent == other.ApmPresent &&
                   UpsPresent == other.UpsPresent &&
                   ThermalControl == other.ThermalControl &&
                   ProcessorThrottle == other.ProcessorThrottle &&
                   ProcessorMinThrottle == other.ProcessorMinThrottle &&
                   ProcessorMaxThrottle == other.ProcessorMaxThrottle &&
                   FastSystemS4 == other.FastSystemS4 &&
                   HiberBoot == other.HiberBoot &&
                   WakeAlarmPresent == other.WakeAlarmPresent &&
                   AoAc == other.AoAc &&
                   DiskSpinDown == other.DiskSpinDown &&
                   SystemBatteriesPresent == other.SystemBatteriesPresent &&
                   BatteriesAreShortTerm == other.BatteriesAreShortTerm &&
                   BatteryScale1 == other.BatteryScale1 &&
                   BatteryScale2 == other.BatteryScale2 &&
                   BatteryScale2 == other.BatteryScale3 &&
                   AcOnLineWake == other.AcOnLineWake &&
                   SoftLidWake == other.SoftLidWake &&
                   RtcWake == other.RtcWake &&
                   MinDeviceWakeState == other.MinDeviceWakeState &&
                   DefaultLowLatencyWake == other.DefaultLowLatencyWake;
        }

        public static bool operator ==(SYSTEM_POWER_CAPABILITIES left, SYSTEM_POWER_CAPABILITIES right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SYSTEM_POWER_CAPABILITIES left, SYSTEM_POWER_CAPABILITIES right)
        {
            return !(left == right);
        }
    }
}