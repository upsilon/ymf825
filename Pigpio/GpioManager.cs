using Pigpio.Api;
using System.Collections.Generic;

namespace Pigpio
{
    public class GpioManager
    {
        private IPigpioApi api;
        private Dictionary<int, GpioPin> pins;

        public GpioManager(IPigpioApi api)
        {
            this.api = api;
            this.pins = new Dictionary<int, GpioPin>();
        }

        public GpioPin this[int p]
        {
            get
            {
                if (this.pins.TryGetValue(p, out var pin))
                    return pin;

                pin = new GpioPin(this.api, p);
                this.pins[p] = pin;

                return pin;
            }
        }
    }
}
