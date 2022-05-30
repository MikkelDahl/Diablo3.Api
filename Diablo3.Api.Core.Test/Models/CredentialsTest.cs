using System;
using Diablo3.Api.Core.Models;
using NUnit.Framework;

namespace Diablo3.Api.Core.Test.Models
{
    [TestFixture]
    public class CredentialsTest
    {
    
        [Test]
        public void Constructor_throws_on_null_args()
        {
            TestHelper.AssertConstructorThrowsOnNullArgs<Credentials>();
        }
    
        [Test]
        public void Constructor_throws_if_args_contain_whitespace()
        {
            Assert.Throws<ArgumentNullException>(() => new Credentials(string.Empty, string.Empty));
        }
    
        [Test]
        public void Autoproperties_are_set_from_constructor()
        {
            const string id = "test";
            const string secret = "secret";

            var sut = new Credentials(id, secret);
        
            Assert.Multiple(() =>
            {
                Assert.That(id, Is.EqualTo(sut.ClientId));
                Assert.That(secret, Is.EqualTo(sut.Secret));
            });
        }
    }
}