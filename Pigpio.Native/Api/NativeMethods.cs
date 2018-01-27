using System.Runtime.InteropServices;

namespace Pigpio.Api
{
    internal static class NativeMethods
    {
        public const string PigpioLibrary = "libpigpio.so";

#pragma warning disable IDE1006 // Naming rule violation: These words must begin with upper case characters

        [DllImport(PigpioLibrary, EntryPoint = nameof(gpioInitialise))]
        public static extern int gpioInitialise();

        [DllImport(PigpioLibrary, EntryPoint = nameof(gpioTerminate))]
        public static extern void gpioTerminate();

        [DllImport(PigpioLibrary, EntryPoint = nameof(gpioSetMode))]
        public static extern int gpioSetMode(int gpio, int mode);

        [DllImport(PigpioLibrary, EntryPoint = nameof(gpioGetMode))]
        public static extern int gpioGetMode(int gpio);

        [DllImport(PigpioLibrary, EntryPoint = nameof(gpioSetPullUpDown))]
        public static extern int gpioSetPullUpDown(int gpio, int pud);

        [DllImport(PigpioLibrary, EntryPoint = nameof(gpioRead))]
        public static extern int gpioRead(int gpio);

        [DllImport(PigpioLibrary, EntryPoint = nameof(gpioWrite))]
        public static extern int gpioWrite(int gpio, int level);

        [DllImport(PigpioLibrary, EntryPoint = nameof(spiOpen))]
        public static extern int spiOpen(int spiChan, int baud, int spiFlags);

        [DllImport(PigpioLibrary, EntryPoint = nameof(spiClose))]
        public static extern int spiClose(int handle);

        [DllImport(PigpioLibrary, EntryPoint = nameof(spiRead))]
        public static extern unsafe int spiRead(int handle, byte* buf, int count);

        [DllImport(PigpioLibrary, EntryPoint = nameof(spiWrite))]
        public static extern unsafe int spiWrite(int handle, byte* buf, int count);

        [DllImport(PigpioLibrary, EntryPoint = nameof(spiXfer))]
        public static extern unsafe int spiXfer(int handle, byte* txBuf, byte* rxBuf, int count);

#pragma warning restore IDE1006
    }
}
