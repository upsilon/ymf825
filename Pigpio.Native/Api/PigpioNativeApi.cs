using System;
using System.Runtime.InteropServices;

namespace Pigpio.Api
{
    public sealed class PigpioNativeApi : IPigpioApi
    {
        public PigpioNativeApi()
            => this.CheckError(() => NativeMethods.gpioInitialise());

        ~PigpioNativeApi()
            => this.Dispose();

        public void Dispose()
            => NativeMethods.gpioTerminate();

        public void SetMode(int gpio, int mode)
            => this.CheckError(() => NativeMethods.gpioSetMode(gpio, mode));

        public int GetMode(int gpio)
            => this.CheckError(() => NativeMethods.gpioGetMode(gpio));

        public void SetPullUpDown(int gpio, int pud)
            => this.CheckError(() => NativeMethods.gpioSetPullUpDown(gpio, pud));

        public int GpioRead(int gpio)
            => this.CheckError(() => NativeMethods.gpioRead(gpio));

        public void GpioWrite(int gpio, int level)
            => this.CheckError(() => NativeMethods.gpioWrite(gpio, level));

        public int SpiOpen(int channel, int baud, int flags)
            => this.CheckError(() => NativeMethods.spiOpen(channel, baud, flags));

        public void SpiClose(int handle)
            => this.CheckError(() => NativeMethods.spiClose(handle));

        public unsafe void SpiRead(int handle, Span<byte> buf)
        {
            fixed (byte* p = &MemoryMarshal.GetReference(buf))
            {
                var ret = NativeMethods.spiRead(handle, p, buf.Length);
                this.CheckError(ret);
            }
        }

        public unsafe void SpiWrite(int handle, Span<byte> buf)
        {
            fixed (byte* p = &MemoryMarshal.GetReference(buf))
            {
                var ret = NativeMethods.spiWrite(handle, p, buf.Length);
                this.CheckError(ret);
            }
        }

        public unsafe void SpiXfer(int handle, Span<byte> tx, Span<byte> rx)
        {
            fixed (byte* ptx = &MemoryMarshal.GetReference(tx))
            fixed (byte* prx = &MemoryMarshal.GetReference(rx))
            {
                var ret = NativeMethods.spiXfer(handle, ptx, prx, tx.Length);
                this.CheckError(ret);
            }
        }

        private int CheckError(Func<int> func)
            => this.CheckError(func());

        private int CheckError(int ret)
            => ret >= 0 ? ret : throw new PigpioApiException($"code = {ret}");
    }
}
