using Ymf825.IO;

namespace Ymf825
{
    public class Ymf825WiringPi : Ymf825
    {
        public override TargetChip AvailableChip => TargetChip.Board0;

        public Ymf825WiringPi(WiringPiSpi spiDevice)
            : base(spiDevice)
        {
        }
    }
}
