using System;
#if !NETSTANDARD2_0
using System.Buffers.Binary;
#else
using Pigpio.Polyfills;
#endif

namespace Pigpio.IO
{
    public readonly ref struct PigpioMessage
    {
        public Span<byte> Span { get; }
        public Span<byte> ExtSpan { get; }

        public Span<byte> Command
            => this.Span.Slice(0, 4);

        public Span<byte> P1
            => this.Span.Slice(4, 4);

        public Span<byte> P2
            => this.Span.Slice(8, 4);

        public Span<byte> P3
            => this.Span.Slice(12, 4);

        public PigpioMessage(Span<byte> span, Span<byte> ext)
        {
            this.Span = span;
            this.ExtSpan = ext;
        }

        public void SetAll(PigpioCommand cmd, int p1, int p2, int p3)
        {
            BinaryPrimitives.WriteInt32LittleEndian(this.Command, (int)cmd);
            BinaryPrimitives.WriteInt32LittleEndian(this.P1, p1);
            BinaryPrimitives.WriteInt32LittleEndian(this.P2, p2);
            BinaryPrimitives.WriteInt32LittleEndian(this.P3, p3);
        }
    }
}
