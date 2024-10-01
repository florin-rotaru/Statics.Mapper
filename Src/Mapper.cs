using Statics.Mapper.Internal;
using System;
using System.Collections.Generic;

namespace Statics.Mapper
{
    public static partial class Mapper<S, D>
    {
        public delegate void ActionRef(S source, ref D destination);

        static Func<S, D>? _compiledFunc { get; set; }
        public static Func<S, D> CompiledFunc
        {
            get
            {
                _compiledFunc ??= CompilerCompileFunc(MapperConfig<S, D>.GetOptions());
                return _compiledFunc!;
            }
            internal set
            { _compiledFunc = value; }
        }

        internal static ActionRef? _compiledActionRef { get; set; }
        public static ActionRef CompiledActionRef
        {
            get
            {
                _compiledActionRef ??= CompilerCompileActionRef(MapperConfig<S, D>.GetOptions());
                return _compiledActionRef;
            }
            internal set
            { _compiledActionRef = value; }
        }

        static List<IMapperOptionArguments> ParseMapOptions(Action<MapperMapOptions<S, D>>? mapOptions = null)
        {
            MapperMapOptions<S, D> options = new();
            mapOptions?.Invoke(options);

            return options.GetMapOptionArguments();
        }

        internal static Func<S, D> CompilerCompileFunc(List<IMapperOptionArguments>? mapOptions = null) =>
            (Func<S, D>)new FuncCompiler(typeof(S), typeof(D), MethodType.Function)
                .Compile(mapOptions)
                .CreateDelegate(typeof(Func<S, D>));

        internal static ActionRef CompilerCompileActionRef(List<IMapperOptionArguments>? mapOptions = null) =>
            (ActionRef)new ActionRefCompiler(typeof(S), typeof(D), MethodType.ActionRef)
                .Compile(mapOptions)
                .CreateDelegate(typeof(ActionRef));



        /// <summary>
        /// Compiles map Func
        /// </summary>
        /// <param name="mapOptions">
        /// When null members will be mapped applying options (MapperConfig) / conventions (public properties with the same names and same/derived/convertible types)
        /// </param>
        /// <returns></returns>
        public static Func<S, D> CompileFunc(Action<MapperMapOptions<S, D>>? mapOptions = null) =>
            CompilerCompileFunc(ParseMapOptions(mapOptions));

        /// <summary>
        /// Compiles map ActionRef
        /// </summary>
        /// <param name="mapOptions">
        /// When null members will be mapped applying options (MapperConfig) / conventions (public properties with the same names and same/derived/convertible types)
        /// </param>
        /// <returns></returns>
        public static ActionRef CompileActionRef(Action<MapperMapOptions<S, D>>? mapOptions = null) =>
             CompilerCompileActionRef(ParseMapOptions(mapOptions));

        public static string ViewIL(Action<MapperMapOptions<S, D>>? mapOptions = null) =>
            new FuncCompiler(typeof(S), typeof(D), MethodType.Function).ViewIL(ParseMapOptions(mapOptions));
        public static string ViewActionRefIL(Action<MapperMapOptions<S, D>>? mapOptions = null) =>
            new ActionRefCompiler(typeof(S), typeof(D), MethodType.ActionRef).ViewIL(ParseMapOptions(mapOptions));

        public static D Map(S source) => CompiledFunc(source);
        public static void Map(S source, ref D destination) => CompiledActionRef(source, ref destination);
    }
}
