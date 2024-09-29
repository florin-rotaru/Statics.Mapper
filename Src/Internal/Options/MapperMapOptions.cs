using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Statics.Mapper.Internal.Options
{
    public class MapperMapOptions<S, D>
    {
        List<IMapperOptionArguments> MapOptionArguments { get; } = [];

        public List<IMapperOptionArguments> GetMapOptionArguments() { return MapOptionArguments; }

        public MapperMapOptions<S, D> Ignore(Expression<Func<D, object>> destination)
        {
            Expression expressionBody = destination.Body;
            if (expressionBody is NewExpression)
                MapOptionArguments.Add(new MapperOptionArguments(nameof(Ignore), [.. Expressions.GetNames(destination, true)]));
            else
                MapOptionArguments.Add(new MapperOptionArguments(nameof(Ignore), [Expressions.GetName(destination, true)]));

            return this;
        }

        public MapperMapOptions<S, D> Ignore(params string[] destination)
        {
            MapOptionArguments.Add(new MapperOptionArguments(nameof(Ignore), destination));
            return this;
        }

        public MapperMapOptions<S, D> Map(
            string source,
            string destination,
            bool expand = true,
            bool useMapperConfig = true)
        {
            MapOptionArguments.Add(new MapperOptionArguments(nameof(Map), [source, destination, expand, useMapperConfig]));
            return this;
        }

        public MapperMapOptions<S, D> Map(
            Expression<Func<S, object>> source,
            Expression<Func<D, object>> destination,
            bool expand = true,
            bool useMapperConfig = true) =>
            Map(Expressions.GetName(source, true), Expressions.GetName(destination, true), expand, useMapperConfig);
    }
}
