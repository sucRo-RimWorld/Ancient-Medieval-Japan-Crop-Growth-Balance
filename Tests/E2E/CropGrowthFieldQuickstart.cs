using RimWorks.Quickstarts;
using Verse;

namespace AMJ.CropGrowthBalance.NativeTests
{
    // Disposable map only; this class never enters the distributed gameplay payload.
    public sealed class CropGrowthFieldQuickstart : AbstractQuickstart
    {
        public override TaggedString description => "Isolated real crop sow, TickLong growth and harvest";
        public override int mapSize => 50;
        public override string seed => "AMJ-CROP-GROWTH-FIELD-20261014";
    }
}
