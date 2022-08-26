using Fluid.Core;

namespace Fluid.Core.Tests
{
    [TestFixture]
    public class FluidTest_DirectionBitmap
    {
        [Test]
        public void TestUpBitmap()
        {
            var code = (int)GridDirection.Up;
            var correctCode = 1 << 3;
            Assert.AreEqual(code, correctCode);
        }
        [Test]
        public void TestDownBitmap()
        {
            var code = (int)GridDirection.Down;
            var correctCode = 1 << 2;
            Assert.AreEqual(code, correctCode);
        }
        [Test]
        public void TestLeftBitmap()
        {
            var code = (int)GridDirection.Left;
            var correctCode = 1 << 1;
            Assert.AreEqual(code, correctCode);
        }
        [Test]
        public void TestRightBitmap()
        {
            var code = (int)GridDirection.Right;
            var correctCode = 1 << 0;
            Assert.AreEqual(code, correctCode);
        }
    }
}