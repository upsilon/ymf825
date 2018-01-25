using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace Ymf825.IO
{
    public sealed class WiringPiSpi : ISpi
    {
        public bool IsDisposed { get; private set; }

        private SpiChannel SpiChannel { get; set; }

        private GpioPin PinSS { get; set; }
        private GpioPin PinRST { get; set; }

        public WiringPiSpi()
            : this(Pi.Gpio.Pin06, Pi.Gpio.Pin27)
        {
        }

        public WiringPiSpi(GpioPin pinSS, GpioPin pinRST)
        {
            this.InitializeSpi();
            this.InitializeGpio(pinSS, pinRST);
        }

        public void Dispose()
            => this.IsDisposed = true;

        public void SetCsTargetPin(byte pin)
        {
        }

        public void Write(byte command, byte data)
        {
            try
            {
                this.PinSS.Write(GpioPinValue.Low);

                this.SpiChannel.Write(new[] { command, data });
            }
            finally
            {
                this.PinSS.Write(GpioPinValue.High);
            }
        }

        public void BurstWrite(byte command, byte[] data, int offset, int count)
        {
            try
            {
                this.PinSS.Write(GpioPinValue.Low);

                var buffer = new byte[count + 1];

                buffer[0] = command;
                Array.Copy(data, offset, buffer, 1, count);

                this.SpiChannel.Write(buffer);
            }
            finally
            {
                this.PinSS.Write(GpioPinValue.High);
            }
        }

        public byte Read(byte command)
        {
            byte[] received;
            try
            {
                this.PinSS.Write(GpioPinValue.Low);

                this.SpiChannel.Write(new[] { command });

                const byte dummy = 0x00;
                received = this.SpiChannel.SendReceive(new[] { dummy });
            }
            finally
            {
                this.PinSS.Write(GpioPinValue.High);
            }

            return received[0];
        }

        public void Flush()
        {
        }

        public void ResetHardware()
        {
            this.PinRST.Write(GpioPinValue.Low);

            Thread.Sleep(1);

            this.PinRST.Write(GpioPinValue.High);
        }

        private void InitializeSpi()
        {
            const int frequency = 10_000_000;

            Pi.Spi.Channel0Frequency = frequency;
            this.SpiChannel = Pi.Spi.Channel0;
        }

        private void InitializeGpio(GpioPin pinSS, GpioPin pinRST)
        {
            this.PinSS = pinSS;
            this.PinSS.PinMode = GpioPinDriveMode.Output;
            this.PinSS.Write(GpioPinValue.High);

            this.PinRST = pinRST;
            this.PinRST.PinMode = GpioPinDriveMode.Output;
        }
    }
}
