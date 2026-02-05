using Xunit;

namespace UnitTesting
{
    public class UnitTest1
    {
        [Fact]
        public void PassExampleTest()
        {
            int result = 4 + 3;

            Assert.Equal(7, result);
        }

        [Fact]
        public void FailExampleTest()
        {
            int result = 4 + 3;

            Assert.NotEqual(8, result);
        }
    }
}
