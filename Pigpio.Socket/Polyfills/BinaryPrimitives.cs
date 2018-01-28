#if NETSTANDARD2_0
using System;

namespace Pigpio.Polyfills
{
    internal static class BinaryPrimitives
    {
        public static int ReadInt32LittleEndian(ReadOnlySpan<byte> buffer)
        {
            var value = 0;

            value |= buffer[3] << 24;
            value |= buffer[2] << 16;
            value |= buffer[1] << 8;
            value |= buffer[0];

            return value;
        }

        public static void WriteInt32LittleEndian(Span<byte> buffer, int value)
        {
            buffer[3] = (byte)((value & 0xff000000) >> 24);
            buffer[2] = (byte)((value & 0x00ff0000) >> 16);
            buffer[1] = (byte)((value & 0x0000ff00) >> 8);
            buffer[0] = (byte)(value & 0x000000ff);
        }
    }
}
#endif
