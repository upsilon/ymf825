using Pigpio.Api;

namespace Pigpio
{
    public class GpioPin
    {
        public int Pin { get; }

        private IPigpioApi Api { get; }

        public GpioPin(IPigpioApi api, int pin)
        {
            this.Api = api;
            this.Pin = pin;
        }

        public GpioPinMode PinMode
        {
            get => (GpioPinMode)this.Api.GetMode(this.Pin);
            set => this.Api.SetMode(this.Pin, (int)value);
        }

        public void SetPullUpDown(int pud)
            => this.Api.SetPullUpDown(this.Pin, pud);

        public int Read()
            => this.Api.GpioRead(this.Pin);

        public void Write(bool level)
            => this.Write(level ? GpioPinLevel.High : GpioPinLevel.Low);

        public void Write(GpioPinLevel level)
            => this.Api.GpioWrite(this.Pin, (int)level);
    }

    public enum GpioPinMode
    {
        Input = Constants.PI_INPUT,
        Output = Constants.PI_OUTPUT,
        Alt0 = Constants.PI_ALT0,
        Alt1 = Constants.PI_ALT1,
        Alt2 = Constants.PI_ALT2,
        Alt3 = Constants.PI_ALT3,
        Alt4 = Constants.PI_ALT4,
        Alt5 = Constants.PI_ALT5,
    }

    public enum GpioPinLevel
    {
        Low = Constants.LOW,
        High = Constants.HIGH,
    }
}
