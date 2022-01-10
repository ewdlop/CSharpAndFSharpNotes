using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.DependencyInjection;

namespace TestProject1
{
    public class UnitTest1
    {
        private readonly ITestOutputHelperAccessor _testOutputHelperAccessor;

        public UnitTest1(ITestOutputHelperAccessor testOutputHelperAccessor)
        {
            _testOutputHelperAccessor = testOutputHelperAccessor;
            _testOutputHelperAccessor.Output?.WriteLine("1111111111111111");
        }

        [Fact]
        public void Test1()
        {
        }
    }
}