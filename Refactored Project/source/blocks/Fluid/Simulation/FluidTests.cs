namespace Game.Blocks.Fluid
{
    public class FluidTests
    {
        private IFluid _fluid;

        public static void RunAllFluidTests(IFluid fluid)
        {
            TestFixtureBase[] tests = new[]
            {
                (TestFixtureBase)new TestEnumFromVec(),
                (TestFixtureBase)new TestEnumerationToVec()
            };
        }
    }
}