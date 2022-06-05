using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Services
{
    [TestFixture]
    public class DiabloClientTest
    {
        [Test]
        public void Constructor_throws_on_null_args()
        {
           TestHelper.AssertConstructorThrowsOnNullArgs<DiabloClient>();
        }
    }
}