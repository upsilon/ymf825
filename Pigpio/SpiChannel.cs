using Pigpio.Api;
using System;

namespace Pigpio
{
    public sealed class SpiChannel : IDisposable
    {
        public int Channel { get; }
        public int? Handle { get; private set; }

        public IPigpioApi Api { get; }

        public SpiChannel(IPigpioApi api, int channel)
        {
            this.Api = api;
            this.Channel = channel;
        }

        public void Dispose()
            => this.Close();

        public void Open(int baud, int flags)
            => this.Handle = this.Api.SpiOpen(this.Channel, baud, flags);

        public void Close()
        {
            if (this.Handle == null)
                return;

            this.Api.SpiClose(this.Handle.Value);
            this.Handle = null;
        }

        public void Read(Span<byte> buffer)
            => this.Api.SpiRead(this.Handle.Value, buffer);

        public void Write(Span<byte> buffer)
            => this.Api.SpiWrite(this.Handle.Value, buffer);

        public void Xfer(Span<byte> tx, Span<byte> rx)
            => this.Api.SpiXfer(this.Handle.Value, tx, rx);
    }
}
