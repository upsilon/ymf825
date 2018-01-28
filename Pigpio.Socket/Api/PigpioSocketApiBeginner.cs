using Pigpio.IO;

namespace Pigpio.Api
{
    public partial class PigpioSocketApi
    {
        public void SetMode(int gpio, int mode)
        {
            this.Send(PigpioCommand.MODES, gpio, mode, 0);
            this.Receive();
        }

        public int GetMode(int gpio)
        {
            this.Send(PigpioCommand.MODEG, gpio, 0, 0);
            var mode = this.Receive();

            return mode;
        }

        public void SetPullUpDown(int gpio, int pud)
        {
            this.Send(PigpioCommand.PUD, gpio, pud, 0);
            this.Receive();
        }

        public int GpioRead(int gpio)
        {
            this.Send(PigpioCommand.READ, gpio, 0, 0);
            var level = this.Receive();

            return level;
        }

        public void GpioWrite(int gpio, int level)
        {
            this.Send(PigpioCommand.WRITE, gpio, level, 0);
            this.Receive();
        }
    }
}
