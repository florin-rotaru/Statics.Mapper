using Air.Mapper;
using Models;
using Xunit;

namespace Playground
{
    public class ReturnDefault
    {
        [Fact]
        public void From_NTS1C0_I0_Members_To_TC1C0_I0_Members()
        {
            var source = new TS1C0_I0_Members?();
            var mapFunc = Mapper<TS1C0_I0_Members?, TC1C0_I0_Members>.CompileFunc();
            var mapAction = Mapper<TS1C0_I0_Members?, TC1C0_I0_Members>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.Null(destination);

            mapAction(source, ref destination);
            Assert.Null(destination);

            destination = new TC1C0_I0_Members();
            mapAction(source, ref destination);
            Assert.Null(destination);
        }

        [Fact]
        public void From_NTS1C0_I0_Members_To_TS1C0_I0_Members()
        {
            var source = new TS1C0_I0_Members?();
            var mapFunc = Mapper<TS1C0_I0_Members?, TS1C0_I0_Members>.CompileFunc();
            var mapAction = Mapper<TS1C0_I0_Members?, TS1C0_I0_Members>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.Null(destination.N0);

            mapAction(source, ref destination);
            Assert.Null(destination.N0);

            destination = new TS1C0_I0_Members { N0 = new TC0_I0_Members() };
            mapAction(source, ref destination);
            Assert.Null(destination.N0);
        }

        [Fact]
        public void From_NTS1C0_I0_Members_To_NTS1C0_I0_Members()
        {
            var source = new TS1C0_I0_Members?();
            var mapFunc = Mapper<TS1C0_I0_Members?, TS1C0_I0_Members?>.CompileFunc();
            var mapAction = Mapper<TS1C0_I0_Members?, TS1C0_I0_Members?>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.Null(destination);

            mapAction(source, ref destination);
            Assert.Null(destination);

            destination = new TS1C0_I0_Members?(new TS1C0_I0_Members());
            mapAction(source, ref destination);
            Assert.Null(destination);
        }


        [Fact]
        public void From_TC1C0_I0_Members_To_TC1C0_I0_Members()
        {
            var source = new TC1C0_I0_Members();
            var mapFunc = Mapper<TC1C0_I0_Members, TC1C0_I0_Members>.CompileFunc();
            var mapAction = Mapper<TC1C0_I0_Members, TC1C0_I0_Members>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.NotNull(destination);

            source = null;
            destination = mapFunc(source);
            Assert.Null(destination);

            mapAction(source, ref destination);
            Assert.Null(destination);

            destination = new TC1C0_I0_Members();
            mapAction(source, ref destination);
            Assert.Null(destination);
        }

        [Fact]
        public void From_TC1C0_I0_Members_To_TS1C0_I0_Members()
        {
            var source = new TC1C0_I0_Members();
            var mapFunc = Mapper<TC1C0_I0_Members, TS1C0_I0_Members>.CompileFunc();
            var mapAction = Mapper<TC1C0_I0_Members, TS1C0_I0_Members>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.Null(destination.N0);

            source = null;
            destination = mapFunc(source);
            Assert.Null(destination.N0);

            mapAction(source, ref destination);
            Assert.Null(destination.N0);

            destination = new TS1C0_I0_Members { N0 = new TC0_I0_Members() };
            mapAction(source, ref destination);
            Assert.Null(destination.N0);
        }

        [Fact]
        public void From_TC1C0_I0_Members_To_NTS1C0_I0_Members()
        {
            var source = new TC1C0_I0_Members();
            var mapFunc = Mapper<TC1C0_I0_Members, TS1C0_I0_Members?>.CompileFunc();
            var mapAction = Mapper<TC1C0_I0_Members, TS1C0_I0_Members?>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.NotNull(destination);

            source = null;
            destination = mapFunc(source);
            Assert.Null(destination);

            mapAction(source, ref destination);
            Assert.Null(destination);

            destination = new TS1C0_I0_Members?(new TS1C0_I0_Members());
            mapAction(source, ref destination);
            Assert.Null(destination);
        }
    }
}
