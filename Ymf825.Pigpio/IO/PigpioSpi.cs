using Pigpio;
using System;
using System.Threading;

namespace Ymf825.IO
{
    public sealed class PigpioSpi : ISpi
    {
        public bool IsDisposed { get; private set; }

        public TargetChip TargetChip
        {
            get => this._targetChip;
            set
            {
                if (value != TargetChip.Board0 && value != TargetChip.Board1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                this._targetChip = value;
            }
        }

        private SpiChannel SpiChannel
            => this.TargetChip == TargetChip.Board0 ? this.Spi0 : this.Spi1;

        private SpiChannel Spi0 { get; set; }
        private SpiChannel Spi1 { get; set; }

        private GpioPin PinSS { get; set; }
        private GpioPin PinRST { get; set; }

        private TargetChip _targetChip;

        public PigpioSpi(PigpioClient pi)
            : this(pi.Spi[0], pi.Spi[1], pi.Gpio[25], pi.Gpio[16])
        {
        }

        public PigpioSpi(SpiChannel spi0, SpiChannel spi1, GpioPin pinSS, GpioPin pinRST)
        {
            this.InitializeSpi(spi0, spi1);
            this.InitializeGpio(pinSS, pinRST);
        }

        public void Dispose()
        {
            if (this.IsDisposed)
                return;

            this.Spi0.Dispose();
            this.Spi1.Dispose();

            this.IsDisposed = true;
        }

        public void SetCsTargetPin(byte pin)
            => this.TargetChip = (TargetChip)(pin >> 3);

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

        private void InitializeSpi(SpiChannel spi0, SpiChannel spi1)
        {
            const int frequency = 10_000_000;

            this.Spi0 = spi0;
            this.Spi0.Open(frequency, 0);

            this.Spi1 = spi1;
            this.Spi1.Open(frequency, 0);
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
