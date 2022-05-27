using Diablo3.Api.Core.Models;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Models;

[TestFixture]
public class ItemTest
{
    
    [Test]
    public void Constructor_throws_on_null_args()
    {
        TestHelper.AssertConstructorThrowsOnNullArgs<Item>();
    }
    
    [Test]
    public void Autoproperties_are_set_from_constructor()
    {
        var name = "test";
        var uri = "https://test.test";

        var sut = new Item(name, uri);
        
        Assert.Multiple(() =>
        {
            Assert.That(name, Is.EqualTo(sut.Name));
            Assert.That(uri, Is.EqualTo(sut.IconUri));
        });
    }
}