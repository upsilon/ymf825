using Pigpio.Api;
using System.Collections.Generic;

namespace Pigpio
{
    public class SpiManager
    {
        private IPigpioApi api;
        private Dictionary<int, SpiChannel> channels;

        public SpiManager(IPigpioApi api)
        {
            this.api = api;
            this.channels = new Dictionary<int, SpiChannel>();
        }

        public SpiChannel this[int ch]
        {
            get
            {
                if (this.channels.TryGetValue(ch, out var channel))
                    return channel;

                channel = new SpiChannel(this.api, ch);
                this.channels[ch] = channel;

                return channel;
            }
        }
    }
}
