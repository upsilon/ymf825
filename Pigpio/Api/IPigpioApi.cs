using System;

namespace Pigpio.Api
{
    public partial interface IPigpioApi : IDisposable
    {
        void SetMode(int gpio, int mode);
        int GetMode(int gpio);
        void SetPullUpDown(int gpio, int pud);
        int GpioRead(int gpio);
        void GpioWrite(int gpio, int level);

        int SpiOpen(int channel, int baud, int flags);
        void SpiClose(int handle);
        void SpiRead(int handle, Span<byte> buf);
        void SpiWrite(int handle, Span<byte> buf);
        void SpiXfer(int handle, Span<byte> tx, Span<byte> rx);
    }
}
