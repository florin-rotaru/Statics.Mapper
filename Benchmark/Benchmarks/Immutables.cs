using AutoFixture;
using BenchmarkDotNet.Attributes;
using Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Benchmark.Benchmarks
{
    [InProcess]
    public class Immutables
    {
        private readonly List<TC0_I0_Members> _source;

        public Immutables()
        {
            _source = new List<TC0_I0_Members>();

            for (int i = 0; i < 32; i++)
                _source.AddRange(new Fixture().Create<List<TC0_I0_Members>>());
        }

        //[Benchmark]
        //public int ImmutableCreate()
        //{
        //    var destination = ImmutableList.Create(_source);
        //    return destination.Count;
        //}

        //[Benchmark]
        //public int ImmutableAddRange()
        //{
        //    var destination = ImmutableList<TC0_I0_Members>.Empty;
        //    destination = destination.AddRange(_source);
        //    return destination.Count;
        //}

        //[Benchmark]
        //public int ImmutableEnumerator()
        //{
        //    var destination = ImmutableStack.Create(_source.ToArray());
        //    int count = 0;
        //    var enumerator = destination.GetEnumerator();
        //    while (enumerator.MoveNext())
        //        count++;
        //    return count;
        //}

        //[Benchmark]
        //public int ImmutableIEnumerableEnumerator()
        //{
        //    var destination = ImmutableStack.Create(_source.ToArray());
        //    int count = 0;
        //    var enumerator = ((IEnumerable<TC0_I0_Members>)destination).GetEnumerator();
        //    while (enumerator.MoveNext())
        //        count++;
        //    return count;
        //}
    }
}
