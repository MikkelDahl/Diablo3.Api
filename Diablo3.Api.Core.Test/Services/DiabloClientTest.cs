using Diablo3.Api.Core.Services;
using Diablo3.Api.Core.Services.Characters;
using Moq;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Services
{
    [TestFixture]
    public class DiabloClientTest
    {
        private Mock<IHeroFetcher> heroFetcherMock;
        private Mock<IAccountFetcher> accountFetcherMock;
        private Mock<ILeaderBoardService> leaderBoardServiceMock;
        private Mock<IItemFetcher> itemCacheMock;

        [SetUp]
        public void Setup()
        {
            leaderBoardServiceMock = new Mock<ILeaderBoardService>(); 
            heroFetcherMock = new Mock<IHeroFetcher>();
            itemCacheMock = new Mock<IItemFetcher>();
            accountFetcherMock = new Mock<IAccountFetcher>();
        }

        [Test]
        public void Constructor_throws_on_null_args()
        {
           TestHelper.AssertConstructorThrowsOnNullArgs<DiabloClient>();
        }


        private DiabloClient Sut() =>
            new(leaderBoardServiceMock.Object, heroFetcherMock.Object, itemCacheMock.Object, accountFetcherMock.Object);
    }
}