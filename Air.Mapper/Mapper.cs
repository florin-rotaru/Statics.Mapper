using Air.Mapper.Internal;
using System;
using System.Collections.Generic;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        private static bool Configured = false;
        private static ActionRef CompiledActionRef = new ActionRefCompiler<S, D>().Compile();
        private static Func<S, D> CompiledFunc = new FuncCompiler<S, D>().Compile();

        public delegate void ActionRef(S source, ref D destination);

        public static void Reset()
        {
            Configured = false;
            TryConfigure();
        }

        public static bool IsConfigured() => Configured;

        public static void Configure(Action<MapOptions<S, D>> mapOptions)
        {
            if (Configured)
                throw new InvalidOperationException($"Mapper<{typeof(S)}, {typeof(D)}> already configured!");

            TryConfigure(mapOptions);
        }

        public static void Configure(ActionRef actionRef, Func<S, D> func)
        {
            if (Configured)
                throw new InvalidOperationException($"Mapper<{typeof(S)}, {typeof(D)}> already configured!");

            TryConfigure(actionRef, func);
        }

        public static bool TryConfigure(ActionRef actionRef, Func<S, D> func)
        {
            if (Configured)
                return false;

            Configured = true;

            CompiledActionRef = actionRef;
            CompiledFunc = func;

            return true;
        }

        public static bool TryConfigure(Action<MapOptions<S, D>> mapOptions = null)
        {
            if (Configured)
                return false;

            return TryConfigure(
                new ActionRefCompiler<S, D>().Compile(mapOptions),
                new FuncCompiler<S, D>().Compile(mapOptions));
        }

        public static void ReConfigure(Action<MapOptions<S, D>> mapOptions)
        {
            Configured = false;
            TryConfigure(mapOptions);
        }

        public static void ReConfigure(ActionRef actionRef, Func<S, D> func)
        {
            Configured = false;
            TryConfigure(actionRef, func);
        }

        public static Func<S, D> GetCompiledFunc() => CompiledFunc;
        public static ActionRef GetCompiledActionRef() => CompiledActionRef;

        public static Func<S, D> CompileFunc(Action<MapOptions<S, D>> mapOptions = null) => new FuncCompiler<S, D>().Compile(mapOptions);
        public static ActionRef CompileActionRef(Action<MapOptions<S, D>> mapOptions = null) => new ActionRefCompiler<S, D>().Compile(mapOptions);

        public static string ViewFuncIL(Action<MapOptions<S, D>> mapOptions = null) => new FuncCompiler<S, D>().ViewIL(mapOptions);
        public static string ViewActionRefIL(Action<MapOptions<S, D>> mapOptions = null) => new ActionRefCompiler<S, D>().ViewIL(mapOptions);

        public static void Map(S source, ref D destination) => CompiledActionRef(source, ref destination);
        public static D Map(S source) => CompiledFunc(source);

    }
}
