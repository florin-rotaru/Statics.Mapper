using Air.Mapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public static class Mapper<S, D>
    {
        private static bool IsConfigured = false;
        private static ActionRef CompiledActionRef = new ActionRefCompiler<S, D>().Compile();
        private static Func<S, D> CompiledFunc = new FuncCompiler<S, D>().Compile();

        public delegate void ActionRef(S source, ref D destination);

        #region Configure

        public static void Reset()
        {
            IsConfigured = false;
            TryConfigure();
        }

        public static void Configure(Action<MapOptions<S, D>> mapOptions)
        {
            if (IsConfigured)
                throw new InvalidOperationException($"Mapper<{typeof(S)}, {typeof(D)}> already configured!");

            TryConfigure(mapOptions);
        }

        public static void Configure(ActionRef actionRef, Func<S, D> func)
        {
            if (IsConfigured)
                throw new InvalidOperationException($"Mapper<{typeof(S)}, {typeof(D)}> already configured!");

            TryConfigure(actionRef, func);
        }

        public static void ReConfigure(Action<MapOptions<S, D>> mapOptions)
        {
            IsConfigured = false;
            TryConfigure(mapOptions);
        }

        public static void ReConfigure(ActionRef actionRef, Func<S, D> func)
        {
            IsConfigured = false;
            TryConfigure(actionRef, func);
        }

        public static bool TryConfigure(Action<MapOptions<S, D>> mapOptions = null)
        {
            if (IsConfigured)
                return false;

            CompiledActionRef = new ActionRefCompiler<S, D>().Compile(mapOptions);
            CompiledFunc = new FuncCompiler<S, D>().Compile(mapOptions);

            return true;
        }

        public static bool TryConfigure(ActionRef actionRef, Func<S, D> func)
        {
            if (IsConfigured)
                return false;

            CompiledActionRef = actionRef;
            CompiledFunc = func;

            return true;
        }

        #endregion

        #region Compile

        public static ActionRef GetCompiledActionRef() => CompiledActionRef;
        public static Func<S, D> GetCompiledFunc() => CompiledFunc;

        public static Func<S, D> CompileFunc(Action<MapOptions<S, D>> mapOptions = null)
        {
            return new FuncCompiler<S, D>().Compile(mapOptions);
        }

        public static string ViewFuncIL(Action<MapOptions<S, D>> mapOptions = null)
        {
            return new FuncCompiler<S, D>().ViewIL(mapOptions);
        }

        public static ActionRef CompileActionRef(Action<MapOptions<S, D>> mapOptions = null)
        {
            return new ActionRefCompiler<S, D>().Compile(mapOptions);
        }

        public static string ViewActionRefIL(Action<MapOptions<S, D>> mapOptions = null)
        {
            return new ActionRefCompiler<S, D>().ViewIL(mapOptions);
        }

        #endregion

        #region Map

        public static void Map(S source, ref D destination)
        {
            CompiledActionRef(source, ref destination);
        }

        public static D Map(S source)
        {
            return CompiledFunc(source);
        }

        public static D[] ToArray(IEnumerable<S> source)
        {
            if (source == null)
                return new D[0] { };

            S[] sourceArray = source.ToArray();
            D[] returnValue = new D[sourceArray.Length];

            for (int i = 0; i < sourceArray.Length; i++)
                returnValue[i] = CompiledFunc(sourceArray[i]);

            return returnValue;
        }

        public static void ToArray(IEnumerable<S> source, ref D[] destination)
        {
            if (source == null)
            {
                destination = new D[0] { };
                return;
            }

            S[] sourceArray = source.ToArray();

            if (destination == null)
                destination = new D[sourceArray.Length];

            if (sourceArray.Length > destination.Length)
                Array.Resize(ref destination, sourceArray.Length);

            for (int i = 0; i < sourceArray.Length; i++)
            {
                D entry = destination[i];
                CompiledActionRef(sourceArray[i], ref entry);
                destination[i] = entry;
            }
        }

        private static IEnumerable<KeyValuePair<DK, D>> ToKeyValuePairs<SK, DK>(IDictionary<SK, S> source)
        {
            foreach (KeyValuePair<SK, S> sourceEntry in source)
                yield return new KeyValuePair<DK, D>(
                    Mapper<SK, DK>.Map(sourceEntry.Key),
                    CompiledFunc(sourceEntry.Value));
        }

        public static Dictionary<DK, D> ToDictionary<SK, DK>(IDictionary<SK, S> source)
        {
            if (source == null)
                return new Dictionary<DK, D>(0);

            return new Dictionary<DK, D>(ToKeyValuePairs<SK, DK>(source));
        }

        public static void ToDictionary<SK, DK>(IDictionary<SK, S> source, ref Dictionary<DK, D> destination)
        {
            if (source == null)
            {
                destination = new Dictionary<DK, D>(0);
                return;
            }

            if (destination == null)
                destination = new Dictionary<DK, D>();

            foreach (KeyValuePair<SK, S> sourceEntry in source)
            {
                DK dkEntry = Mapper<SK, DK>.Map(sourceEntry.Key);

                if (destination.TryGetValue(dkEntry, out D dEntry))
                {
                    CompiledActionRef(sourceEntry.Value, ref dEntry);
                }
                else
                {
                    CompiledActionRef(sourceEntry.Value, ref dEntry);
                    destination.TryAdd(dkEntry, dEntry);
                }
            }
        }

        #endregion
    }
}
