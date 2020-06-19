using Air.Mapper;
using Models;
using Xunit;

namespace Playground
{
    public class EmptyTypes
    {
        [Fact]
        public void From_TC0_Empty_To_TC0_Empty()
        {
            var source = new TC0_Empty();
            var mapFunc = Mapper<TC0_Empty, TC0_Empty>.CompileFunc();
            var mapAction = Mapper<TC0_Empty, TC0_Empty>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.NotNull(destination);

            destination = null;
            mapAction(source, ref destination);
            Assert.NotNull(destination);

            source = null;
            destination = mapFunc(source);
            Assert.Null(destination);

            mapAction(source, ref destination);
            Assert.Null(destination);

            destination = new TC0_Empty();
            mapAction(source, ref destination);
            Assert.Null(destination);
        }

        [Fact]
        public void From_TC0_Empty_To_TS0_Empty()
        {
            var source = new TC0_Empty();
            var mapFunc = Mapper<TC0_Empty, TS0_Empty>.CompileFunc();
            var mapAction = Mapper<TC0_Empty, TS0_Empty>.CompileActionRef();

            var destination = mapFunc(source);
            mapAction(source, ref destination);

            source = null;
            destination = mapFunc(source);
            mapAction(source, ref destination);
        }

        [Fact]
        public void From_TS0_Empty_To_TC0_Empty()
        {
            var source = new TS0_Empty();
            var mapFunc = Mapper<TS0_Empty, TC0_Empty>.CompileFunc();
            var mapAction = Mapper<TS0_Empty, TC0_Empty>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.NotNull(destination);

            destination = null;
            mapAction(source, ref destination);
            Assert.NotNull(destination);
        }

        [Fact]
        public void From_TS0_Empty_To_TS0_Empty()
        {
            var source = new TS0_Empty();
            var mapFunc = Mapper<TS0_Empty, TS0_Empty>.CompileFunc();
            var mapAction = Mapper<TS0_Empty, TS0_Empty>.CompileActionRef();

            var destination = mapFunc(source);
            mapAction(source, ref destination);
        }

        [Fact]
        public void From_TC0_Empty_To_NTS0_Empty()
        {
            var source = new TC0_Empty();
            var mapFunc = Mapper<TC0_Empty, TS0_Empty?>.CompileFunc();
            var mapAction = Mapper<TC0_Empty, TS0_Empty?>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.NotNull(destination);

            destination = null;
            mapAction(source, ref destination);
            Assert.NotNull(destination);

            source = null;
            destination = mapFunc(source);
            Assert.Null(destination);

            mapAction(source, ref destination);
            Assert.Null(destination);

            destination = new TS0_Empty?();
            mapAction(source, ref destination);
            Assert.Null(destination);
        }

        [Fact]
        public void From_TS0_Empty_To_NTS0_Empty()
        {
            var source = new TS0_Empty();
            var mapFunc = Mapper<TS0_Empty, TS0_Empty?>.CompileFunc();
            var mapAction = Mapper<TS0_Empty, TS0_Empty?>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.NotNull(destination);

            destination = null;
            mapAction(source, ref destination);
            Assert.NotNull(destination);
        }


        [Fact]
        public void From_NSTS0_Empty_To_TC0_Empty()
        {
            var source = new TS0_Empty?(new TS0_Empty());
            var mapFunc = Mapper<TS0_Empty?, TC0_Empty>.CompileFunc();
            var mapAction = Mapper<TS0_Empty?, TC0_Empty>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.NotNull(destination);

            destination = null;
            mapAction(source, ref destination);
            Assert.NotNull(destination);

            source = null;
            destination = mapFunc(source);
            Assert.Null(destination);

            mapAction(source, ref destination);
            Assert.Null(destination);

            destination = new TC0_Empty();
            mapAction(source, ref destination);
            Assert.Null(destination);
        }

        [Fact]
        public void From_NSTS0_Empty_To_TS0_Empty()
        {
            var source = new TS0_Empty?(new TS0_Empty());
            var mapFunc = Mapper<TS0_Empty?, TS0_Empty>.CompileFunc();
            var mapAction = Mapper<TS0_Empty?, TS0_Empty>.CompileActionRef();

            var destination = mapFunc(source);
            mapAction(source, ref destination);

            source = null;
            destination = mapFunc(source);
            mapAction(source, ref destination);
        }


        [Fact]
        public void From_NSTS0_Empty_To_NTS0_Empty()
        {
            var source = new TS0_Empty?(new TS0_Empty());
            var mapFunc = Mapper<TS0_Empty?, TS0_Empty?>.CompileFunc();
            var mapAction = Mapper<TS0_Empty?, TS0_Empty?>.CompileActionRef();

            var destination = mapFunc(source);
            Assert.NotNull(destination);

            destination = null;
            mapAction(source, ref destination);
            Assert.NotNull(destination);

            source = null;
            destination = mapFunc(source);
            Assert.Null(destination);

            mapAction(source, ref destination);
            Assert.Null(destination);

            destination = new TS0_Empty?(new TS0_Empty());
            mapAction(source, ref destination);
            Assert.Null(destination);
        }
    }
}
