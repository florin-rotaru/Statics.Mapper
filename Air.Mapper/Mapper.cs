using Air.Mapper.Internal;
using System;
using System.Collections.Generic;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static List<IMapOption> ParseMapOptions(Action<MapOptions<S, D>> mapOptions = null)
        {
            MapOptions<S, D> options = new MapOptions<S, D>();
            mapOptions?.Invoke(options);

            return options.Get();
        }

        public static Func<S, D> CompiledFunc { get; private set; } = CompileFunc();
        public static ActionRef CompiledActionRef { get; private set; } = CompileActionRef();

        public delegate void ActionRef(S source, ref D destination);

        public static void Configure(Action<MapOptions<S, D>> mapOptions = null)
        {
            CompiledActionRef = CompileActionRef(mapOptions);
            CompiledFunc = CompileFunc(mapOptions);
        }

        //public static Func<S, D> GetCompiledFunc() => CompiledFunc;
        //public static ActionRef GetCompiledActionRef() => CompiledActionRef;

        public static Func<S, D> CompileFunc(Action<MapOptions<S, D>> mapOptions = null) =>
            (Func<S, D>)new FuncCompiler(typeof(S), typeof(D), MethodType.Function, new List<IMapOption>())
                .Compile(ParseMapOptions(mapOptions))
                .CreateDelegate(typeof(Func<S, D>));
        public static ActionRef CompileActionRef(Action<MapOptions<S, D>> mapOptions = null) =>
             (ActionRef)new ActionRefCompiler(typeof(S), typeof(D), MethodType.ActionRef, new List<IMapOption>())
                .Compile(ParseMapOptions(mapOptions))
                .CreateDelegate(typeof(ActionRef));

        public static string ViewFuncIL(Action<MapOptions<S, D>> mapOptions = null) =>
            new FuncCompiler(typeof(S), typeof(D), MethodType.Function, new List<IMapOption>()).ViewIL(ParseMapOptions(mapOptions));
        public static string ViewActionRefIL(Action<MapOptions<S, D>> mapOptions = null) =>
            new ActionRefCompiler(typeof(S), typeof(D), MethodType.ActionRef, new List<IMapOption>()).ViewIL(ParseMapOptions(mapOptions));

        public static D Map(S source) => CompiledFunc(source);
        public static void Map(S source, ref D destination) => CompiledActionRef(source, ref destination);
    }
}
