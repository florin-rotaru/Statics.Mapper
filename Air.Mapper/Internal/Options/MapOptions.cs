using Air.Mapper.Internal;
using Air.Reflection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Air.Mapper
{
    public class MapOptions<S, D>
    {
        private List<IMapOption> Options { get; } = new List<IMapOption>();

        public IEnumerable<IMapOption> Get() { return Options; }

        public MapOptions<S, D> Ignore(Expression<Func<D, object>> destination)
        {
            Options.Add(new Option(nameof(Ignore), new object[] { TypeInfo.GetName(destination, true) }));
            return this;
        }

        public MapOptions<S, D> Ignore(string destination)
        {
            Options.Add(new Option(nameof(Ignore), new object[] { destination }));
            return this;
        }

        public MapOptions<S, D> Map(
            string source,
            string destination,
            bool expand = true,
            bool useMapperConfig = true)
        {
            Options.Add(new Option(nameof(Map), new object[] { source, destination, expand, useMapperConfig }));
            return this;
        }

        public MapOptions<S, D> Map(
            Expression<Func<S, object>> source,
            Expression<Func<D, object>> destination,
            bool expand = true,
            bool useMapperConfig = true) =>
            Map(TypeInfo.GetName(source, true), TypeInfo.GetName(destination, true), expand, useMapperConfig);
    }
}
