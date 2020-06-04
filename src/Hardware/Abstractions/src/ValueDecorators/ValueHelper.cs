using EquipApps.Hardware.Behaviors.Digital;

namespace EquipApps.Hardware.ValueDecorators
{
    public static class ValueHelper
    {
        public static byte ToNil(byte value, int bitIndex)
        {
            byte mask1 = (byte)(1 << bitIndex);
            byte mask2 = (byte)(~mask1);
            byte rezlt = (byte)(value & mask2);

            return rezlt;
        }
        public static ushort ToNil(ushort value, int bitIndex)
        {
            ushort mask1 = (ushort)(1 << bitIndex);
            ushort mask2 = (ushort)(~mask1);
            ushort rezlt = (ushort)(value & mask2);

            return rezlt;
        }

        public static byte ToOne(byte value, int bitIndex)
        {
            byte mask1 = (byte)(1 << bitIndex);            
            byte rezlt = (byte)(value | mask1);

            return rezlt;
        }
        public static ushort ToOne(ushort value, int bitIndex)
        {
            ushort mask1 = (ushort)(1 << bitIndex);
            ushort rezlt = (ushort)(value | mask1);

            return rezlt;
        }

        public static Digit GetDigit(byte value, int bitIndex)
        {
            byte mask1 = (byte)(1 << bitIndex);
            byte mask2 = (byte)(value & mask1);

            return mask2 == 0 
                ? Digit.Nil 
                : Digit.One;
        }
        public static Digit GetDigit(ushort value, int bitIndex)
        {
            ushort mask1 = (ushort)(1 << bitIndex);
            ushort mask2 = (ushort)(value & mask1);

            return mask2 == 0
                ? Digit.Nil
                : Digit.One;
        }
    }
}
