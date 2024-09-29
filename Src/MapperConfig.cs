using Statics.Mapper.Internal.Options;
using System;
using System.Collections.Generic;

namespace Statics.Mapper
{
    public class MapperConfig<S, D>
    {
        internal static List<IMapperOptionArguments> Options { get; set; } = [];
        internal static bool UsePredefinedMap { get; set; }

        public static List<IMapperOptionArguments> GetOptions() => Options;
        public static void SetOptions(Action<MapperMapOptions<S, D>> options, bool overwrite = false)
        {
            if (Options != null && !overwrite)
                throw new InvalidOperationException($"{nameof(Options)} for {typeof(S).Name} and {typeof(D).Name} already set!");

            MapperMapOptions<S, D> mapOptions = new();
            options.Invoke(mapOptions);
            Options = mapOptions.GetMapOptionArguments();
        }

        public static void SetMap(Func<S, D> func, Mapper<S, D>.ActionRef actionRef)
        {
            UsePredefinedMap = true;

            Mapper<S, D>.CompiledFunc = func;
            Mapper<S, D>.CompiledActionRef = actionRef;
        }

        /// <summary>
        /// Resets CompiledFunc and CompiledActionRef to their initial state.
        /// </summary>
        public static void ResetMap()
        {
            UsePredefinedMap = false;

            Mapper<S, D>.CompiledFunc = Mapper<S, D>.CompileFunc();
            Mapper<S, D>.CompiledActionRef = Mapper<S, D>.CompileActionRef();
        }
    }
}
