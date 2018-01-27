using Pigpio.Api;

namespace Pigpio
{
    public sealed class PigpioClient
    {
        public GpioManager Gpio { get; }
        public SpiManager Spi { get; }

        private readonly IPigpioApi api;

        public PigpioClient(IPigpioApi api)
        {
            this.api = api;

            this.Gpio = new GpioManager(api);
            this.Spi = new SpiManager(api);
        }
    }
}
