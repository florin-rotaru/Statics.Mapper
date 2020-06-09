using AutoFixture;
using AutoMapper;
using BenchmarkDotNet.Running;
using Air.Mapper;
using Mapster;
using Nelibur.ObjectMapper;
using System;
using System.Diagnostics;
using static Benchmark.Models;


namespace Benchmark
{
    public class Program
    {
        private static IMapper _autoMapper;
        private static Func<Nodes, Nodes> _mapperFunc;
        private static Mapper<Nodes, Nodes>.ActionRef _mapperAction;

        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BenchmarkContainer>();
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadLine();

            //Init();
            //Run();
            //Console.ReadLine();
        }

        private static void WriteLine(string library, Stopwatch stopwatch, Nodes source, Nodes destination)
        {
            var newSourceMemberValues = new Fixture().Create<SystemTypeCodes>();
            source.Node_1.Segment.SystemTypeCodes = newSourceMemberValues;

            bool sameReference = source.Node_1.Segment.SystemTypeCodes.StringMember == destination.Node_1.Segment.SystemTypeCodes.StringMember;

            Console.WriteLine("{0,-18};{1,-32}",
                stopwatch.Elapsed,
                library);
        }

        private static void Init()
        {
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
            TinyMapper.Bind<Nodes, Nodes>();
            TinyMapper.Bind<Node, Node>();
            TinyMapper.Bind<Segment, Segment>();
            TinyMapper.Bind<SystemTypeCodes, SystemTypeCodes>();
            TinyMapper.Bind<NullableSystemTypeCodes, NullableSystemTypeCodes>();

            //ExpressMapper Configuration 
            ExpressMapper.Mapper.Register<Nodes, Nodes>();
            ExpressMapper.Mapper.Register<Node, Node>();
            ExpressMapper.Mapper.Register<Segment, Segment>();
            ExpressMapper.Mapper.Register<SystemTypeCodes, SystemTypeCodes>();
            ExpressMapper.Mapper.Register<NullableSystemTypeCodes, NullableSystemTypeCodes>();

            //Mapster no configuration required
            //AgileMapper no configuration required

            _mapperFunc = Mapper<Nodes, Nodes>.CompileFunc();
            _mapperAction = Mapper<Nodes, Nodes>.CompileActionRef();
        }

        private static void Run()
        {
            Warmup();

            var source = new Fixture().Create<Nodes>();

            var destination = new Nodes();

            int runs = 1;
            int actions = 25;

            while (runs < 5)
            {
                Stopwatch stopwatch = new Stopwatch();

                Console.WriteLine($" =======  Run: {runs}; Actions: {actions};  =======");

                stopwatch.Start();
                for (int i = 0; i < actions; i++)
                    destination = _autoMapper.Map<Nodes>(source);

                stopwatch.Stop();
                WriteLine("AutoMapper", stopwatch, source, destination);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    destination = AgileObjects.AgileMapper.Mapper.Map(source).ToANew<Nodes>();

                stopwatch.Stop();
                WriteLine("AgileObjects.AgileMapper", stopwatch, source, destination);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    destination = ExpressMapper.Mapper.Map<Nodes, Nodes>(source);

                stopwatch.Stop();
                WriteLine("ExpressMapper", stopwatch, source, destination);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    destination = TinyMapper.Map<Nodes>(source);

                stopwatch.Stop();
                WriteLine("TinyMapper", stopwatch, source, destination);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    destination = source.Adapt<Nodes>();

                stopwatch.Stop();
                WriteLine("Mapster", stopwatch, source, destination);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    destination = _mapperFunc(source);

                stopwatch.Stop();
                WriteLine("Air.Mapper compiled Func", stopwatch, source, destination);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    _mapperAction(source, ref destination);

                stopwatch.Stop();
                WriteLine("Air.Mapper compiled Action", stopwatch, source, destination);

                stopwatch.Restart();
                for (int i = 0; i < actions; i++)
                    destination = Mapper<Nodes, Nodes>.Map(source);

                stopwatch.Stop();
                WriteLine("Air.Mapper", stopwatch, source, destination);

                Console.WriteLine(string.Empty);

                runs += 1;
                actions = actions * 10;
            }
        }

        private static void Warmup()
        {
            var source = new Fixture().Create<Nodes>();
            var destination = new Nodes();

            for (int i = 0; i < 16; i++)
            {
                destination = _autoMapper.Map<Nodes>(source);
                destination = AgileObjects.AgileMapper.Mapper.Map(source).ToANew<Nodes>();
                destination = ExpressMapper.Mapper.Map<Nodes, Nodes>(source);
                destination = TinyMapper.Map<Nodes>(source);
                destination = source.Adapt<Nodes>();
                destination = _mapperFunc(source);
                _mapperAction(source, ref destination);
                destination = Mapper<Nodes, Nodes>.Map(source);
            }
        }

    }
}
