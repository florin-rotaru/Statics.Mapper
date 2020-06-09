using Air.Mapper;
using AutoFixture;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using ExpressionDebugger;
using Mapster;
using static Benchmark.Models;

namespace Benchmark
{
    [InProcess]
    public class BenchmarkContainer
    {
        private readonly Nodes _source;
        private readonly IMapper _autoMapper;

        public BenchmarkContainer()
        {
            _source = new Fixture().Create<Nodes>();

            //Automapper Configuration 
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Nodes, Nodes>();
                cfg.CreateMap<Node, Node>();
                cfg.CreateMap<Segment, Segment>();
                cfg.CreateMap<SystemTypeCodes, SystemTypeCodes>();
                cfg.CreateMap<NullableSystemTypeCodes, NullableSystemTypeCodes>();
            });
            _autoMapper = mapperConfig.CreateMapper();

            //TinyMapper Configuration 
            Nelibur.ObjectMapper.TinyMapper.Bind<Nodes, Nodes>();
            Nelibur.ObjectMapper.TinyMapper.Bind<Node, Node>();
            Nelibur.ObjectMapper.TinyMapper.Bind<Segment, Segment>();
            Nelibur.ObjectMapper.TinyMapper.Bind<SystemTypeCodes, SystemTypeCodes>();
            Nelibur.ObjectMapper.TinyMapper.Bind<NullableSystemTypeCodes, NullableSystemTypeCodes>();

            //ExpressMapper Configuration 
            global::ExpressMapper.Mapper.Register<Nodes, Nodes>();
            global::ExpressMapper.Mapper.Register<Node, Node>();
            global::ExpressMapper.Mapper.Register<Segment, Segment>();
            global::ExpressMapper.Mapper.Register<SystemTypeCodes, SystemTypeCodes>();
            global::ExpressMapper.Mapper.Register<NullableSystemTypeCodes, NullableSystemTypeCodes>()
                // fails when mapping EnumMember
                .Ignore(d => d.EnumMember);

            //Mapster don't need configuration
            //AgileMapper don't need configuration

        }

        [Benchmark]
        public Nodes ManualMapping()
        {
            return Mapper.Map(_source);
        }

        [Benchmark]
        public Nodes ExpressMapper()
        {
            return global::ExpressMapper.Mapper.Map<Nodes, Nodes>(_source);
        }

        [Benchmark]
        public Nodes AgileMapper()
        {
            return AgileObjects.AgileMapper.Mapper.Map(_source).ToANew<Nodes>();
        }

        [Benchmark]
        public Nodes TinyMapper()
        {
            return Nelibur.ObjectMapper.TinyMapper.Map<Nodes>(_source);
        }

        [Benchmark]
        public Nodes AutoMapper()
        {
            return _autoMapper.Map<Nodes>(_source);
        }

        [Benchmark]
        public Nodes Mapster()
        {
            return _source.Adapt<Nodes>();
        }

        [Benchmark]
        public Nodes StaticsMapper()
        {
            return Mapper<Nodes, Nodes>.Map(_source);
        }


    }

    public static class Mapper
    {
        public static Nodes Map(Nodes source)
        {
            //script generated with ExpressionDebugger
            Nodes nodes = null;
            var script = nodes.BuildAdapter()
               .CreateMapExpression<Nodes>()
               .ToScript();

            return source == null ? null : new Nodes()
            {
                Node_1 = source.Node_1 == null ? null : new Node()
                {
                    Name = source.Node_1.Name,
                    Segment = source.Node_1.Segment == null ? null : new Segment()
                    {
                        SystemTypeCodes = source.Node_1.Segment.SystemTypeCodes == null ? null : new SystemTypeCodes()
                        {
                            BooleanMember = source.Node_1.Segment.SystemTypeCodes.BooleanMember,
                            CharMember = source.Node_1.Segment.SystemTypeCodes.CharMember,
                            SByteMember = source.Node_1.Segment.SystemTypeCodes.SByteMember,
                            ByteMember = source.Node_1.Segment.SystemTypeCodes.ByteMember,
                            Int16SMember = source.Node_1.Segment.SystemTypeCodes.Int16SMember,
                            UInt16Member = source.Node_1.Segment.SystemTypeCodes.UInt16Member,
                            Int32Member = source.Node_1.Segment.SystemTypeCodes.Int32Member,
                            UInt32Member = source.Node_1.Segment.SystemTypeCodes.UInt32Member,
                            Int64Member = source.Node_1.Segment.SystemTypeCodes.Int64Member,
                            UInt64Member = source.Node_1.Segment.SystemTypeCodes.UInt64Member,
                            SingleMember = source.Node_1.Segment.SystemTypeCodes.SingleMember,
                            DoubleMember = source.Node_1.Segment.SystemTypeCodes.DoubleMember,
                            DecimalMember = source.Node_1.Segment.SystemTypeCodes.DecimalMember,
                            StringMember = source.Node_1.Segment.SystemTypeCodes.StringMember,
                            DateTimeMember = source.Node_1.Segment.SystemTypeCodes.DateTimeMember,
                            DateTimeOffsetMember = source.Node_1.Segment.SystemTypeCodes.DateTimeOffsetMember,
                            TimeSpanMember = source.Node_1.Segment.SystemTypeCodes.TimeSpanMember,
                            GuidMember = source.Node_1.Segment.SystemTypeCodes.GuidMember,
                            EnumMember = source.Node_1.Segment.SystemTypeCodes.EnumMember,
                            DtoEnumMember = source.Node_1.Segment.SystemTypeCodes.DtoEnumMember
                        },
                        NullableSystemTypeCodes = source.Node_1.Segment.NullableSystemTypeCodes == null ? null : new NullableSystemTypeCodes()
                        {
                            BooleanMember = source.Node_1.Segment.NullableSystemTypeCodes.BooleanMember,
                            CharMember = source.Node_1.Segment.NullableSystemTypeCodes.CharMember,
                            SByteMember = source.Node_1.Segment.NullableSystemTypeCodes.SByteMember,
                            ByteMember = source.Node_1.Segment.NullableSystemTypeCodes.ByteMember,
                            Int16SMember = source.Node_1.Segment.NullableSystemTypeCodes.Int16SMember,
                            UInt16Member = source.Node_1.Segment.NullableSystemTypeCodes.UInt16Member,
                            Int32Member = source.Node_1.Segment.NullableSystemTypeCodes.Int32Member,
                            UInt32Member = source.Node_1.Segment.NullableSystemTypeCodes.UInt32Member,
                            Int64Member = source.Node_1.Segment.NullableSystemTypeCodes.Int64Member,
                            UInt64Member = source.Node_1.Segment.NullableSystemTypeCodes.UInt64Member,
                            SingleMember = source.Node_1.Segment.NullableSystemTypeCodes.SingleMember,
                            DoubleMember = source.Node_1.Segment.NullableSystemTypeCodes.DoubleMember,
                            DecimalMember = source.Node_1.Segment.NullableSystemTypeCodes.DecimalMember,
                            StringMember = source.Node_1.Segment.NullableSystemTypeCodes.StringMember,
                            DateTimeMember = source.Node_1.Segment.NullableSystemTypeCodes.DateTimeMember,
                            DateTimeOffsetMember = source.Node_1.Segment.NullableSystemTypeCodes.DateTimeOffsetMember,
                            TimeSpanMember = source.Node_1.Segment.NullableSystemTypeCodes.TimeSpanMember,
                            GuidMember = source.Node_1.Segment.NullableSystemTypeCodes.GuidMember,
                            EnumMember = source.Node_1.Segment.NullableSystemTypeCodes.EnumMember,
                            DtoEnumMember = source.Node_1.Segment.NullableSystemTypeCodes.DtoEnumMember
                        }
                    }
                },
                Node_2 = source.Node_2 == null ? null : new Node()
                {
                    Name = source.Node_2.Name,
                    Segment = source.Node_2.Segment == null ? null : new Segment()
                    {
                        SystemTypeCodes = source.Node_2.Segment.SystemTypeCodes == null ? null : new SystemTypeCodes()
                        {
                            BooleanMember = source.Node_2.Segment.SystemTypeCodes.BooleanMember,
                            CharMember = source.Node_2.Segment.SystemTypeCodes.CharMember,
                            SByteMember = source.Node_2.Segment.SystemTypeCodes.SByteMember,
                            ByteMember = source.Node_2.Segment.SystemTypeCodes.ByteMember,
                            Int16SMember = source.Node_2.Segment.SystemTypeCodes.Int16SMember,
                            UInt16Member = source.Node_2.Segment.SystemTypeCodes.UInt16Member,
                            Int32Member = source.Node_2.Segment.SystemTypeCodes.Int32Member,
                            UInt32Member = source.Node_2.Segment.SystemTypeCodes.UInt32Member,
                            Int64Member = source.Node_2.Segment.SystemTypeCodes.Int64Member,
                            UInt64Member = source.Node_2.Segment.SystemTypeCodes.UInt64Member,
                            SingleMember = source.Node_2.Segment.SystemTypeCodes.SingleMember,
                            DoubleMember = source.Node_2.Segment.SystemTypeCodes.DoubleMember,
                            DecimalMember = source.Node_2.Segment.SystemTypeCodes.DecimalMember,
                            StringMember = source.Node_2.Segment.SystemTypeCodes.StringMember,
                            DateTimeMember = source.Node_2.Segment.SystemTypeCodes.DateTimeMember,
                            DateTimeOffsetMember = source.Node_2.Segment.SystemTypeCodes.DateTimeOffsetMember,
                            TimeSpanMember = source.Node_2.Segment.SystemTypeCodes.TimeSpanMember,
                            GuidMember = source.Node_2.Segment.SystemTypeCodes.GuidMember,
                            EnumMember = source.Node_2.Segment.SystemTypeCodes.EnumMember,
                            DtoEnumMember = source.Node_2.Segment.SystemTypeCodes.DtoEnumMember
                        },
                        NullableSystemTypeCodes = source.Node_2.Segment.NullableSystemTypeCodes == null ? null : new NullableSystemTypeCodes()
                        {
                            BooleanMember = source.Node_2.Segment.NullableSystemTypeCodes.BooleanMember,
                            CharMember = source.Node_2.Segment.NullableSystemTypeCodes.CharMember,
                            SByteMember = source.Node_2.Segment.NullableSystemTypeCodes.SByteMember,
                            ByteMember = source.Node_2.Segment.NullableSystemTypeCodes.ByteMember,
                            Int16SMember = source.Node_2.Segment.NullableSystemTypeCodes.Int16SMember,
                            UInt16Member = source.Node_2.Segment.NullableSystemTypeCodes.UInt16Member,
                            Int32Member = source.Node_2.Segment.NullableSystemTypeCodes.Int32Member,
                            UInt32Member = source.Node_2.Segment.NullableSystemTypeCodes.UInt32Member,
                            Int64Member = source.Node_2.Segment.NullableSystemTypeCodes.Int64Member,
                            UInt64Member = source.Node_2.Segment.NullableSystemTypeCodes.UInt64Member,
                            SingleMember = source.Node_2.Segment.NullableSystemTypeCodes.SingleMember,
                            DoubleMember = source.Node_2.Segment.NullableSystemTypeCodes.DoubleMember,
                            DecimalMember = source.Node_2.Segment.NullableSystemTypeCodes.DecimalMember,
                            StringMember = source.Node_2.Segment.NullableSystemTypeCodes.StringMember,
                            DateTimeMember = source.Node_2.Segment.NullableSystemTypeCodes.DateTimeMember,
                            DateTimeOffsetMember = source.Node_2.Segment.NullableSystemTypeCodes.DateTimeOffsetMember,
                            TimeSpanMember = source.Node_2.Segment.NullableSystemTypeCodes.TimeSpanMember,
                            GuidMember = source.Node_2.Segment.NullableSystemTypeCodes.GuidMember,
                            EnumMember = source.Node_2.Segment.NullableSystemTypeCodes.EnumMember,
                            DtoEnumMember = source.Node_2.Segment.NullableSystemTypeCodes.DtoEnumMember
                        }
                    }
                },
                Node_3 = source.Node_3 == null ? null : new Node()
                {
                    Name = source.Node_3.Name,
                    Segment = source.Node_3.Segment == null ? null : new Segment()
                    {
                        SystemTypeCodes = source.Node_3.Segment.SystemTypeCodes == null ? null : new SystemTypeCodes()
                        {
                            BooleanMember = source.Node_3.Segment.SystemTypeCodes.BooleanMember,
                            CharMember = source.Node_3.Segment.SystemTypeCodes.CharMember,
                            SByteMember = source.Node_3.Segment.SystemTypeCodes.SByteMember,
                            ByteMember = source.Node_3.Segment.SystemTypeCodes.ByteMember,
                            Int16SMember = source.Node_3.Segment.SystemTypeCodes.Int16SMember,
                            UInt16Member = source.Node_3.Segment.SystemTypeCodes.UInt16Member,
                            Int32Member = source.Node_3.Segment.SystemTypeCodes.Int32Member,
                            UInt32Member = source.Node_3.Segment.SystemTypeCodes.UInt32Member,
                            Int64Member = source.Node_3.Segment.SystemTypeCodes.Int64Member,
                            UInt64Member = source.Node_3.Segment.SystemTypeCodes.UInt64Member,
                            SingleMember = source.Node_3.Segment.SystemTypeCodes.SingleMember,
                            DoubleMember = source.Node_3.Segment.SystemTypeCodes.DoubleMember,
                            DecimalMember = source.Node_3.Segment.SystemTypeCodes.DecimalMember,
                            StringMember = source.Node_3.Segment.SystemTypeCodes.StringMember,
                            DateTimeMember = source.Node_3.Segment.SystemTypeCodes.DateTimeMember,
                            DateTimeOffsetMember = source.Node_3.Segment.SystemTypeCodes.DateTimeOffsetMember,
                            TimeSpanMember = source.Node_3.Segment.SystemTypeCodes.TimeSpanMember,
                            GuidMember = source.Node_3.Segment.SystemTypeCodes.GuidMember,
                            EnumMember = source.Node_3.Segment.SystemTypeCodes.EnumMember,
                            DtoEnumMember = source.Node_3.Segment.SystemTypeCodes.DtoEnumMember
                        },
                        NullableSystemTypeCodes = source.Node_3.Segment.NullableSystemTypeCodes == null ? null : new NullableSystemTypeCodes()
                        {
                            BooleanMember = source.Node_3.Segment.NullableSystemTypeCodes.BooleanMember,
                            CharMember = source.Node_3.Segment.NullableSystemTypeCodes.CharMember,
                            SByteMember = source.Node_3.Segment.NullableSystemTypeCodes.SByteMember,
                            ByteMember = source.Node_3.Segment.NullableSystemTypeCodes.ByteMember,
                            Int16SMember = source.Node_3.Segment.NullableSystemTypeCodes.Int16SMember,
                            UInt16Member = source.Node_3.Segment.NullableSystemTypeCodes.UInt16Member,
                            Int32Member = source.Node_3.Segment.NullableSystemTypeCodes.Int32Member,
                            UInt32Member = source.Node_3.Segment.NullableSystemTypeCodes.UInt32Member,
                            Int64Member = source.Node_3.Segment.NullableSystemTypeCodes.Int64Member,
                            UInt64Member = source.Node_3.Segment.NullableSystemTypeCodes.UInt64Member,
                            SingleMember = source.Node_3.Segment.NullableSystemTypeCodes.SingleMember,
                            DoubleMember = source.Node_3.Segment.NullableSystemTypeCodes.DoubleMember,
                            DecimalMember = source.Node_3.Segment.NullableSystemTypeCodes.DecimalMember,
                            StringMember = source.Node_3.Segment.NullableSystemTypeCodes.StringMember,
                            DateTimeMember = source.Node_3.Segment.NullableSystemTypeCodes.DateTimeMember,
                            DateTimeOffsetMember = source.Node_3.Segment.NullableSystemTypeCodes.DateTimeOffsetMember,
                            TimeSpanMember = source.Node_3.Segment.NullableSystemTypeCodes.TimeSpanMember,
                            GuidMember = source.Node_3.Segment.NullableSystemTypeCodes.GuidMember,
                            EnumMember = source.Node_3.Segment.NullableSystemTypeCodes.EnumMember,
                            DtoEnumMember = source.Node_3.Segment.NullableSystemTypeCodes.DtoEnumMember
                        }
                    }
                }
            };
        }
    }
}
