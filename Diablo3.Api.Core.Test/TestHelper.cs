using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;

namespace Diablo3.Api.Core.Test
{
    public static class TestHelper
    {
        public static void AssertConstructorThrowsOnNullArgs<T>()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var _ = fixture.Create<T>();
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(T).GetConstructors());
        }
    }
}