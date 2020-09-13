using System;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public class MapperConfig<S, D>
    {
        internal static Func<S, D> DefaultFunc = (source) => default;
        internal static Mapper<S, D>.ActionRef DefaultActionRef = (S source, ref D destination) => destination = default;

        internal static List<IMapOption> Options { get; set; } = null;
        internal static bool UsePredefinedMap { get; set; }

        public static IEnumerable<IMapOption> GetOptions() => Options;
        public static void SetOptions(Action<MapOptions<S, D>> options, bool overwrite = false)
        {
            if (Options != null && !overwrite)
                throw new InvalidOperationException($"{nameof(Options)} for {typeof(S).Name} and {typeof(D).Name} already set!");

            MapOptions<S, D> mapOptions = new MapOptions<S, D>();
            options.Invoke(mapOptions);
            Options = mapOptions.Get().ToList();
        }

        public static void SetMap(Func<S, D> func, Mapper<S, D>.ActionRef actionRef)
        {
            UsePredefinedMap = true;

            Mapper<S, D>.CompiledFunc = func;
            Mapper<S, D>.CompiledActionRef = actionRef;
        }

        /// <summary>
        /// Clears both Mapper CompiledFunc and CompiledActionRef
        /// </summary>
        /// <param name="compile">
        /// When false, mapping from <typeparamref name="S"/> to <typeparamref name="D"/> will return a default <typeparamref name="D"/>. 
        /// When true, <typeparamref name="D"/> members will be mapped applying options / conventions (public properties with the same names and same/derived/convertible types)
        /// </param>
        public static void ClearMap(bool compile = true)
        {
            UsePredefinedMap = false;

            Mapper<S, D>.CompiledFunc = compile ? Mapper<S, D>.CompileFunc() : DefaultFunc;
            Mapper<S, D>.CompiledActionRef = compile ? Mapper<S, D>.CompileActionRef() : DefaultActionRef;
        }
    }
}
