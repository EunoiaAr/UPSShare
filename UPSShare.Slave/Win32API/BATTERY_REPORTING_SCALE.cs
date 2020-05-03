using System;

namespace UPSShare.Slave.Win32API
{
    public struct BATTERY_REPORTING_SCALE : IEquatable<BATTERY_REPORTING_SCALE>
    {
        public ulong Granularity;
        public ulong Capacity;

        public bool Equals(BATTERY_REPORTING_SCALE other)
        {
            return Capacity == other.Capacity && Granularity == other.Granularity;
        }

        public override bool Equals(object obj)
        {
            var other = (BATTERY_REPORTING_SCALE) obj;
            return other != default && Equals(other);
        }

        public override int GetHashCode()
        {
            return Granularity.GetHashCode() | Capacity.GetHashCode();
        }

        public static bool operator ==(BATTERY_REPORTING_SCALE left, BATTERY_REPORTING_SCALE right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BATTERY_REPORTING_SCALE left, BATTERY_REPORTING_SCALE right)
        {
            return !(left == right);
        }
    }
}