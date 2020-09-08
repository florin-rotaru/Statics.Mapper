using AutoFixture;
using BenchmarkDotNet.Attributes;
using Models;
using System.Collections.Generic;

namespace Benchmarks
{
    [InProcess]
    public class Collections
    {
        private readonly Fixture _fixture;
        private readonly TC0_I0_Members[] _sourceArray;
        private readonly List<TC0_I0_Members> _sourceList;

        public Collections()
        {
            _fixture = new Fixture();

            _sourceList = _fixture.Create<List<TC0_I0_Members>>();
            for (int i = 0; i < 16; i++)
                _sourceList.Add(_fixture.Create<TC0_I0_Members>());

            _sourceArray = _sourceList.ToArray();
        }

        public int LoopArgumentMethod(TC0_I0_Members[] source)
        {
            int result = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != null)
                    result += 1;
            }
            return result;
        }

        [Benchmark]
        public int LoopArgument() =>
            LoopArgumentMethod(_sourceArray);

        public int CopyLocalLoopArgumentMethod(TC0_I0_Members[] source)
        {
            var localSource = source;
            int result = 0;
            for (int i = 0; i < localSource.Length; i++)
            {
                if (localSource[i] != null)
                    result += 1;
            }
            return result;
        }

        [Benchmark]
        public int CopyLocalLoopArgument() =>
            CopyLocalLoopArgumentMethod(_sourceArray);
    }
}
