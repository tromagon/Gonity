using System;

namespace Gonity
{
    public static class BitUtils
    {
        public static bool HasFlag(this int bit, int bitFlag)
        {
            return ((bit & bitFlag) != 0);
        }

        public static bool HasFlag(this Enum bit, Enum bitFlag)
        {
            return (((int)(IConvertible)bit & (int)(IConvertible)bitFlag) != 0);
        }
    }
}