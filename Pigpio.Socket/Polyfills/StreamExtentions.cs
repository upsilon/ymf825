#if NETSTANDARD2_0
using System;
using System.IO;

namespace Pigpio.Polyfills
{
    internal static class StreamExtentions
    {
        public static void Read(this Stream stream, Span<byte> destination)
        {
            var buffer = new byte[destination.Length];
            stream.Read(buffer, 0, buffer.Length);
            buffer.CopyTo(destination);
        }

        public static void Write(this Stream stream, ReadOnlySpan<byte> source)
            => stream.Write(source.ToArray(), 0, source.Length);
    }
}
#endif
