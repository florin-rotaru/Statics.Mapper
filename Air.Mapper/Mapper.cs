using Air.Mapper.Internal;
using System;
using System.Collections.Generic;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        public delegate void ActionRef(S source, ref D destination);

        private static IEnumerable<IMapOption> ParseMapOptions(Action<MapOptions<S, D>> mapOptions = null)
        {
            MapOptions<S, D> options = new MapOptions<S, D>();
            mapOptions?.Invoke(options);

            return options.Get();
        }

        internal static Func<S, D> CompilerCompileFunc(IEnumerable<IMapOption> mapOptions = null) =>
         (Func<S, D>)new FuncCompiler(typeof(S), typeof(D), MethodType.Function)
             .Compile(mapOptions)
             .CreateDelegate(typeof(Func<S, D>));

        internal static ActionRef CompilerCompileActionRef(IEnumerable<IMapOption> mapOptions = null) =>
           (ActionRef)new ActionRefCompiler(typeof(S), typeof(D), MethodType.ActionRef)
              .Compile(mapOptions)
              .CreateDelegate(typeof(ActionRef));

        public static Func<S, D> CompiledFunc { get; internal set; } =
            !MapperConfig<S, D>.UsePredefinedMap ? CompilerCompileFunc(MapperConfig<S, D>.GetOptions()) : MapperConfig<S, D>.DefaultFunc;
        public static ActionRef CompiledActionRef { get; internal set; } =
            !MapperConfig<S, D>.UsePredefinedMap ? CompilerCompileActionRef(MapperConfig<S, D>.GetOptions()) : MapperConfig<S, D>.DefaultActionRef;

        /// <summary>
        /// Compiles map Func
        /// </summary>
        /// <param name="mapOptions">
        /// When null members will be mapped applying options (MapperConfig) / conventions (public properties with the same names and same/derived/convertible types)
        /// </param>
        /// <returns></returns>
        public static Func<S, D> CompileFunc(Action<MapOptions<S, D>> mapOptions = null) =>
            CompilerCompileFunc(ParseMapOptions(mapOptions));

        /// <summary>
        /// Compiles map ActionRef
        /// </summary>
        /// <param name="mapOptions">
        /// When null members will be mapped applying options (MapperConfig) / conventions (public properties with the same names and same/derived/convertible types)
        /// </param>
        /// <returns></returns>
        public static ActionRef CompileActionRef(Action<MapOptions<S, D>> mapOptions = null) =>
             CompilerCompileActionRef(ParseMapOptions(mapOptions));

        public static string ViewFuncIL(Action<MapOptions<S, D>> mapOptions = null) =>
            new FuncCompiler(typeof(S), typeof(D), MethodType.Function).ViewIL(ParseMapOptions(mapOptions));
        public static string ViewActionRefIL(Action<MapOptions<S, D>> mapOptions = null) =>
            new ActionRefCompiler(typeof(S), typeof(D), MethodType.ActionRef).ViewIL(ParseMapOptions(mapOptions));

        public static D Map(S source) => CompiledFunc(source);
        public static void Map(S source, ref D destination) => CompiledActionRef(source, ref destination);
    }
}
