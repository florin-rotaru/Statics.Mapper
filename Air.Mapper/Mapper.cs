using Air.Mapper.Internal;
using System;
using System.Collections.Generic;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static IEnumerable<IMapOption> ParseMapOptions(Action<MapOptions<S, D>> mapOptions = null)
        {
            MapOptions<S, D> options = new MapOptions<S, D>();
            mapOptions?.Invoke(options);

            return options.Get();
        }

        public static Func<S, D> CompiledFunc { get; private set; } = CompileFunc(MapperConfig<S, D>.GetOptions());
        public static ActionRef CompiledActionRef { get; private set; } = CompileActionRef(MapperConfig<S, D>.GetOptions());

        public delegate void ActionRef(S source, ref D destination);

        private static Func<S, D> CompileFunc(IEnumerable<IMapOption> mapOptions = null) =>
           (Func<S, D>)new FuncCompiler(typeof(S), typeof(D), MethodType.Function)
               .Compile(mapOptions)
               .CreateDelegate(typeof(Func<S, D>));
        public static Func<S, D> CompileFunc(Action<MapOptions<S, D>> mapOptions = null) =>
            CompileFunc(ParseMapOptions(mapOptions));

        private static ActionRef CompileActionRef(IEnumerable<IMapOption> mapOptions = null) =>
             (ActionRef)new ActionRefCompiler(typeof(S), typeof(D), MethodType.ActionRef)
                .Compile(mapOptions)
                .CreateDelegate(typeof(ActionRef));
        public static ActionRef CompileActionRef(Action<MapOptions<S, D>> mapOptions = null) =>
             CompileActionRef(ParseMapOptions(mapOptions));

        public static string ViewFuncIL(Action<MapOptions<S, D>> mapOptions = null) =>
            new FuncCompiler(typeof(S), typeof(D), MethodType.Function).ViewIL(ParseMapOptions(mapOptions));
        public static string ViewActionRefIL(Action<MapOptions<S, D>> mapOptions = null) =>
            new ActionRefCompiler(typeof(S), typeof(D), MethodType.ActionRef).ViewIL(ParseMapOptions(mapOptions));

        public static D Map(S source) => CompiledFunc(source);
        public static void Map(S source, ref D destination) => CompiledActionRef(source, ref destination);
    }
}
