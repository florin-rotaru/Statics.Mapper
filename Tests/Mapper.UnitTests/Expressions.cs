using AutoFixture;
using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Mapper.UnitTests
{
    [Collection(nameof(Expressions))]
    public class Expressions
    {
        Fixture Fixture { get; }

        public Expressions()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(Fixture);
        }

        [Fact]
        public void GetName()
        {
            Expression<Func<TC2C1C0_I0_Members, object>> expression = (c) => c.N1.N0.StringMember;

            Assert.Equal("N1.N0.StringMember", Statics.Mapper.Internal.Expressions.GetName(expression, true));
            Assert.Equal("StringMember", Statics.Mapper.Internal.Expressions.GetName(expression, false));

            expression = (c) => "N1.N0.StringMember";
            Assert.Equal("N1.N0.StringMember", Statics.Mapper.Internal.Expressions.GetName(expression, true));

            expression = (c) => new { c.N1.N0.StringMember };
            Assert.Throws<ArgumentException>(() => Statics.Mapper.Internal.Expressions.GetName(expression, true));
        }
    }
}
