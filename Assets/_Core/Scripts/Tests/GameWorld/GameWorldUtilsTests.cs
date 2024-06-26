using Genesis.GameWorld;
using NUnit.Framework;

namespace Genesis.Tests.GameWorld
{
    public class GameWorldUtilsTests
    {
        [TestCase(0, 16, 0)]
        [TestCase(-1, 16, 15)]
        [TestCase(16, 16, 0)]
        [TestCase(32, 16, 0)]
        [TestCase(-17, 16, 15)]
        public void GetCoordinateWorks(int x, int size, int expected)
        {
            var result = GameWorldUtils.GetCoordinate(x, size);
            Assert.That(result >= 0, "result >= 0");
            Assert.That(result < size, $"result < {size}");
            Assert.That(result == expected, $"result == {expected}");
        }
    }
}