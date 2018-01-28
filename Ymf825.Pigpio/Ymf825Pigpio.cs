using Ymf825.IO;

namespace Ymf825
{
    public class Ymf825Pigpio : Ymf825
    {
        public override TargetChip AvailableChip => TargetChip.Board0 | TargetChip.Board1;

        public Ymf825Pigpio(PigpioSpi spiDevice)
            : base(spiDevice)
        {
        }
    }
}
