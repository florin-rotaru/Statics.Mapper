using AutoFixture;
using Statics.Mapper;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Mapper.UnitTests
{
    [Collection(nameof(MapAs))]
    public class MapAs
    {
        readonly ITestOutputHelper Console;

        Fixture Fixture { get; }

        public MapAs(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(Fixture);
        }

        public interface ISourceStringMember
        {
            string StringMember { get; set; }
        }

        public class SourceStringMember : ISourceStringMember
        {
            public string StringMember { get; set; }
        }

        public interface IDestinationStringMember
        {
            string StringMember { get; set; }
        }

        public class DestinationStringMember : IDestinationStringMember
        {
            public string StringMember { get; set; }
        }

        public class Source
        {
            public ISourceStringMember SourceStringMember { get; set; }
        }

        public interface IDestination
        {
            IDestinationStringMember DestinationStringMember { get; set; }
        }

        public class Destination : IDestination
        {
            public IDestinationStringMember DestinationStringMember { get; set; }
        }

        [Fact]
        public void MapFromClassMemberToInterfaceMember()
        {
            var source = new Source { SourceStringMember = Fixture.Create<SourceStringMember>() };
            Assert.Throws<NotSupportedException>(() => Mapper<Source, IDestination>.Map(source));

            var destination = Mapper<Source, Destination>.Map(source);
            Assert.Null(destination.DestinationStringMember);

            MapperConfig<Source, Destination>.SetOptions(o => o
                .MapAs(d => d.DestinationStringMember, typeof(DestinationStringMember))
                .Map(s => s.SourceStringMember, d => d.DestinationStringMember),
                true);
            destination = Mapper<Source, Destination>.Map(source);
            Assert.Equal(source.SourceStringMember.StringMember, destination.DestinationStringMember.StringMember);

            destination = null;
            Mapper<Source, Destination>.Map(source, ref destination);
            Assert.Equal(source.SourceStringMember.StringMember, destination.DestinationStringMember.StringMember);

            var mapFunc = Mapper<Source, Destination>.CompileFunc(o => o
                .MapAs(d => d.DestinationStringMember, typeof(DestinationStringMember))
                .Map(s => s.SourceStringMember, d => d.DestinationStringMember));
            destination = mapFunc(source);
            Assert.Equal(source.SourceStringMember.StringMember, destination.DestinationStringMember.StringMember);

            destination = null;
            var mapAction = Mapper<Source, Destination>.CompileActionRef(o => o
                .MapAs(d => d.DestinationStringMember, typeof(DestinationStringMember))
                .Map(s => s.SourceStringMember, d => d.DestinationStringMember));
            mapAction(source, ref destination);
            Assert.Equal(source.SourceStringMember.StringMember, destination.DestinationStringMember.StringMember);

            Assert.Throws<NotSupportedException>(() => Mapper<Source, IDestination>.Map(source));

            MapperConfig<ISourceStringMember, IDestinationStringMember>.SetOptions(o => o
                .MapAs(d => d, typeof(DestinationStringMember)),
                true);
            MapperConfig<Source, IDestination>.SetOptions(o => o
               .MapAs(d => d, typeof(Destination))
               .Map(s => s.SourceStringMember, d => d.DestinationStringMember),
               true);
            var destinationContract = Mapper<Source, IDestination>.Map(source);
            Assert.Equal(source.SourceStringMember.StringMember, destinationContract.DestinationStringMember.StringMember);
        
            Mapper<Source, IDestination>.Map(source, ref destinationContract);
            Assert.Equal(source.SourceStringMember.StringMember, destinationContract.DestinationStringMember.StringMember);
        }
    }
}
