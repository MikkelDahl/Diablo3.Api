using Diablo3.Api.Core.Models;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Models
{
    [TestFixture]
    public class ItemBaseTest
    {
    
        [Test]
        public void Constructor_throws_on_null_args()
        {
            TestHelper.AssertConstructorThrowsOnNullArgs<ItemBase>();
        }
    
        [Test]
        public void Autoproperties_are_set_from_constructor()
        {
            var name = "test";
            var uri = "https://test.test";
            var path = "testpath";

            var sut = new ItemBase(name, uri, path);
        
            Assert.Multiple(() =>
            {
                Assert.That(name, Is.EqualTo(sut.Name));
                Assert.That(uri, Is.EqualTo(sut.IconUri));
                Assert.That(path, Is.EqualTo(sut.Path));
            });
        }
    }
}