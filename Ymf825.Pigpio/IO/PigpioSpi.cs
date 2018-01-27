using Pigpio;
using System;
using System.Threading;

namespace Ymf825.IO
{
    public sealed class PigpioSpi : ISpi
    {
        public bool IsDisposed { get; private set; }

        private SpiChannel SpiChannel { get; set; }

        private GpioPin PinSS { get; set; }
        private GpioPin PinRST { get; set; }

        public PigpioSpi(PigpioClient pi)
            : this(pi.Spi[0], pi.Gpio[25], pi.Gpio[16])
        {
        }

        public PigpioSpi(SpiChannel spi, GpioPin pinSS, GpioPin pinRST)
        {
            this.InitializeSpi(spi);
            this.InitializeGpio(pinSS, pinRST);
        }

        public void Dispose()
        {
            if (this.IsDisposed)
                return;

            this.SpiChannel.Dispose();

            this.IsDisposed = true;
        }

        public void SetCsTargetPin(byte pin)
        {
        }

        public void Write(byte command, byte data)
        {
            try
            {
                this.PinSS.Write(GpioPinLevel.Low);

                Span<byte> buffer = stackalloc byte[2];

                buffer[0] = command;
                buffer[1] = data;

                this.SpiChannel.Write(buffer);
            }
            finally
            {
                this.PinSS.Write(GpioPinLevel.High);
            }
        }

        public void BurstWrite(byte command, byte[] data, int offset, int count)
            => this.BurstWrite(command, data.AsSpan().Slice(offset, count));

        public void BurstWrite(byte command, Span<byte> data)
        {
            try
            {
                this.PinSS.Write(GpioPinLevel.Low);

                var bufferSize = data.Length + 1;
                Span<byte> buffer = bufferSize < 128 ? stackalloc byte[bufferSize] : new byte[bufferSize];

                buffer[0] = command;
                data.CopyTo(buffer.Slice(1, data.Length));

                this.SpiChannel.Write(buffer);
            }
            finally
            {
                this.PinSS.Write(GpioPinLevel.High);
            }
        }

        public byte Read(byte command)
        {
            try
            {
                this.PinSS.Write(GpioPinLevel.Low);

                Span<byte> buffer = stackalloc byte[1];
                buffer[0] = command;

                this.SpiChannel.Write(buffer);
                this.SpiChannel.Read(buffer);

                return buffer[0];
            }
            finally
            {
                this.PinSS.Write(GpioPinLevel.High);
            }
        }

        public void Flush()
        {
        }

        public void ResetHardware()
        {
            this.PinRST.Write(GpioPinLevel.Low);

            Thread.Sleep(1);

            this.PinRST.Write(GpioPinLevel.High);
        }

        private void InitializeSpi(SpiChannel spi)
        {
            const int frequency = 10_000_000;

            this.SpiChannel = spi;
            this.SpiChannel.Open(frequency, 0);
        }

        private void InitializeGpio(GpioPin pinSS, GpioPin pinRST)
        {
            this.PinSS = pinSS;
            this.PinSS.PinMode = GpioPinMode.Output;
            this.PinSS.Write(GpioPinLevel.High);

            this.PinRST = pinRST;
            this.PinRST.PinMode = GpioPinMode.Output;
        }
    }
}
