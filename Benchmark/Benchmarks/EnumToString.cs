using BenchmarkDotNet.Attributes;
using Models;

namespace Benchmark.Benchmarks
{
    [InProcess]
    public class EnumToString
    {
        private readonly TABCEnum @enum;

        public EnumToString() =>
            @enum = TABCEnum.B;

        [Benchmark]
        public string ObjectToString() =>
            @enum.ToString();

        [Benchmark]
        public string GetName() =>
            Air.Reflection.Emit.ILGenerator.Converters.Enum<TABCEnum>.GetName(@enum);
    }
}
