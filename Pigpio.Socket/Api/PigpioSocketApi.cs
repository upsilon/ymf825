using Pigpio.IO;
using System;
using System.Buffers.Binary;

namespace Pigpio.Api
{
    public partial class PigpioSocketApi : IPigpioApi
    {
        private readonly PigpioSocket socket;

        public PigpioSocketApi(PigpioSocket socket)
            => this.socket = socket;

        public void Dispose()
            => this.socket.Dispose();

        public void Send(PigpioCommand cmd, int p1, int p2, int p3)
            => this.Send(cmd, p1, p2, p3, Span<byte>.Empty);

        public void Send(PigpioCommand cmd, int p1, int p2, int p3, Span<byte> ext)
        {
            Span<byte> buffer = stackalloc byte[16];

            var request = new PigpioMessage(buffer, ext);
            request.SetAll(cmd, p1, p2, p3);

            this.socket.Send(request);
        }

        public int Receive()
            => this.Receive(Span<byte>.Empty);

        public int Receive(Span<byte> ext)
        {
            Span<byte> buffer = stackalloc byte[16];

            var response = new PigpioMessage(buffer, ext);
            this.socket.Receive(response);

            var ret = BinaryPrimitives.ReadInt32LittleEndian(response.P3);
            this.ThrowIfError(ret);

            return ret;
        }

        private void ThrowIfError(int ret)
        {
            if (ret < 0)
                throw new PigpioApiException($"code = {ret}");
        }
    }
}
