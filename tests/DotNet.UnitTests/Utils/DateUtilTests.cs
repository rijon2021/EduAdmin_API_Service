using DotNet.ApplicationCore.Utils;
using Xunit;

namespace DotNet.UnitTests.Utils
{
    public class DateUtilTests
    {
        [Fact]
        public void GetCurrentDate_ReturnsCorrectDate()
        {
            var currentDate = DateUtil.GetCurrentDate();

            Assert.True(currentDate.Year >= 2021);
        }
    }
}