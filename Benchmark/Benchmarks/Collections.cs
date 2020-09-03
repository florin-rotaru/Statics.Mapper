using AutoFixture;
using BenchmarkDotNet.Attributes;
using ExpressionDebugger;
using Mapster;
using Models;
using System;
using System.Collections.Generic;

namespace Benchmark
{
    [InProcess]
    public class Collections
    {
        private readonly Fixture _fixture;
        private readonly TC0_I0_Members _entry;
        private readonly TC0_I0_Members[] _sourceArray;
        private readonly List<TC0_I0_Members> _sourceList;
        private readonly Queue<TC0_I0_Members> _sourceQueue;
        private readonly Func<TC0_I0_Members, TC0_I0_Members> _mapFunc;

        public Collections()
        {
            _fixture = new Fixture();

            _sourceList = _fixture.Create<List<TC0_I0_Members>>();
            for (int i = 0; i < 16; i++)
                _sourceList.Add(_fixture.Create<TC0_I0_Members>());

            _entry = _fixture.Create<TC0_I0_Members>();
            _sourceArray = _sourceList.ToArray();
            _sourceQueue = new Queue<TC0_I0_Members>(_sourceList);

            _mapFunc = Air.Mapper.Mapper<TC0_I0_Members, TC0_I0_Members>.CompileFunc();


            //TypeAdapterConfig.GlobalSettings.SelfContainedCodeGeneration = true;
            //var account = default(TC0_I0_Members[]);
            //var def = new ExpressionDefinitions
            //{
            //    IsStatic = true,    //change to false if you want instance
            //    MethodName = "Map",
            //    Namespace = "YourNamespace",
            //    TypeName = "CustomerMapper"
            //};
            //var code = account.BuildAdapter()
            //    .CreateMapExpression<TC0_I0_Members[]>()
            //    .ToScript(def);

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


        //[Benchmark]
        //public List<TC0_I0_Members> MapsterMapList() => _sourceList.Adapt<List<TC0_I0_Members>>();

        //[Benchmark]
        //public List<TC0_I0_Members> AirMapperMapList() => Air.Mapper.Mapper<TC0_I0_Members, TC0_I0_Members>.ToList(_sourceList);

        //[Benchmark]
        //public TC0_I0_Members[] MapsterMapArray() => _sourceList.Adapt<TC0_I0_Members[]>();

        //[Benchmark]
        //public TC0_I0_Members[] AirMapperMapArray() => Air.Mapper.Mapper<TC0_I0_Members, TC0_I0_Members>.ToArray(_sourceList);


        //[Benchmark]
        //public TC0_I0_Members[] AirMapperMapDelegateToArray()
        //{
        //    TC0_I0_Members[] destination = new TC0_I0_Members[_sourceList.Count];
        //    for (int i = 0; i < _sourceList.Count; i++)
        //        destination[i] = _mapFunc(_sourceList[i]);
        //    return destination;
        //}


        //[Benchmark]
        //public int MapsterMapInLoop()
        //{
        //    int result = 0;
        //    TC0_I0_Members members;
        //    for (int i = 0; i < 1024; i++)
        //    {
        //        members = _entry.Adapt<TC0_I0_Members>();
        //        if (members != null)
        //            result++;
        //    }
        //    return result;
        //}

        //[Benchmark]
        //public int AirMapperMapInLoop()
        //{
        //    int result = 0;
        //    TC0_I0_Members members;
        //    for (int i = 0; i < 1024; i++)
        //    {
        //        members = Air.Mapper.Mapper<TC0_I0_Members, TC0_I0_Members>.Map(_entry);
        //        if (members != null)
        //            result++;
        //    }
        //    return result;
        //}


        //[Benchmark]
        //public TC0_I0_Members[] MapsterMapInLoopToArray()
        //{
        //    TC0_I0_Members[] result = new TC0_I0_Members[1024];

        //    for (int i = 0; i < 1024; i++)
        //        result[i] = _entry.Adapt<TC0_I0_Members>();

        //    return result;
        //}

        //[Benchmark]
        //public TC0_I0_Members[] AirMapperMapInLoopToArray()
        //{
        //    TC0_I0_Members[] result = new TC0_I0_Members[1024];

        //    for (int i = 0; i < 1024; i++)
        //        result[i] = Air.Mapper.Mapper<TC0_I0_Members, TC0_I0_Members>.Map(_entry);

        //    return result;
        //}


        //[Benchmark]
        //public TC0_I0_Members[] AirMapperMapDelegateToArrayV2()
        //{
        //    TC0_I0_Members[] result = new TC0_I0_Members[_sourceList.Count];

        //    int v = 0;

        //    int i = 0;
        //    int len = _sourceList.Count;

        //    while (i < len)
        //    {
        //        TC0_I0_Members item = _sourceList[i];
        //        result[v++] = _mapFunc(item);
        //        i++;
        //    }
        //    return result;
        //}

        //[Benchmark]
        //public TC0_I0_Members[] ManualMapp() =>
        //    Map(_sourceList);



        //public static TC0_I0_Members[] Map(List<TC0_I0_Members> p1)
        //{
        //    if (p1 == null)
        //    {
        //        return null;
        //    }
        //    TC0_I0_Members[] result = new TC0_I0_Members[p1.Count];

        //    int v = 0;

        //    int i = 0;
        //    int len = p1.Count;

        //    while (i < len)
        //    {
        //        TC0_I0_Members item = p1[i];
        //        result[v++] = item == null ? null : new TC0_I0_Members()
        //        {
        //            BooleanMember = item.BooleanMember,
        //            CharMember = item.CharMember,
        //            SByteMember = item.SByteMember,
        //            ByteMember = item.ByteMember,
        //            Int16SMember = item.Int16SMember,
        //            UInt16Member = item.UInt16Member,
        //            Int32Member = item.Int32Member,
        //            UInt32Member = item.UInt32Member,
        //            Int64Member = item.Int64Member,
        //            UInt64Member = item.UInt64Member,
        //            SingleMember = item.SingleMember,
        //            DoubleMember = item.DoubleMember,
        //            DecimalMember = item.DecimalMember,
        //            StringMember = item.StringMember
        //        };
        //        i++;
        //    }
        //    return result;

        //}
    }
}
