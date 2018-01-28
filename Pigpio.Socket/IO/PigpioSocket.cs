using System;
using System.IO;
using System.Net.Sockets;
#if NETSTANDARD2_0
using Pigpio.Polyfills;
#endif

namespace Pigpio.IO
{
    public sealed class PigpioSocket : IDisposable
    {
        private readonly TcpClient tcpClient;
        private Stream stream;

        public PigpioSocket(string hostname, int port) : this()
            => this.Connect(hostname, port);

        public PigpioSocket()
        {
            this.tcpClient = new TcpClient
            {
                NoDelay = true,
            };
        }

        public void Connect(string hostname, int port)
        {
            this.tcpClient.Connect(hostname, port);
            this.stream = this.tcpClient.GetStream();
        }

        public void Dispose()
        {
            this.stream.Dispose();
            this.tcpClient.Dispose();
        }

        public void Send(PigpioMessage request)
        {
            this.stream.Write(request.Span);

            if (!request.ExtSpan.IsEmpty)
                this.stream.Write(request.ExtSpan);
        }

        public void Receive(PigpioMessage response)
        {
            this.stream.Read(response.Span);

            if (!response.ExtSpan.IsEmpty)
                this.stream.Read(response.ExtSpan);
        }
    }
}
