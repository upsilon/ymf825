using Pigpio.IO;
using System;
using System.Buffers.Binary;

namespace Pigpio.Api
{
    public partial class PigpioSocketApi
    {
        public int SpiOpen(int channel, int baud, int flags)
        {
            Span<byte> ext = stackalloc byte[4];

            BinaryPrimitives.WriteInt32LittleEndian(ext.Slice(0, 4), flags);

            this.Send(PigpioCommand.SPIO, channel, baud, 4, ext);
            var handle = this.Receive();

            return handle;
        }

        public void SpiClose(int handle)
        {
            this.Send(PigpioCommand.SPIC, handle, 0, 0);
            this.Receive();
        }

        public void SpiRead(int handle, Span<byte> buf)
        {
            this.Send(PigpioCommand.SPIR, handle, buf.Length, 0);
            this.Receive(buf);
        }

        public void SpiWrite(int handle, Span<byte> buf)
        {
            this.Send(PigpioCommand.SPIW, handle, 0, buf.Length, buf);
            this.Receive();
        }

        public void SpiXfer(int handle, Span<byte> tx, Span<byte> rx)
        {
            if (tx.Length != rx.Length)
                throw new ArgumentException("The tx and rx length are must be same.", nameof(rx));

            this.Send(PigpioCommand.SPIX, handle, 0, tx.Length, tx);
            this.Receive(rx);
        }
    }
}
