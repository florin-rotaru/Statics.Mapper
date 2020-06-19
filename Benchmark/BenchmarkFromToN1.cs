﻿using AutoFixture;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using Mapster;
using Models;
using System.Linq;

namespace Benchmark
{
    [InProcess]
    public class BenchmarkFromToN1<S, D>
    {
        private readonly S _source;
        private readonly IMapper _autoMapper;
        private readonly Fixture _fixture;

        S NewSource() =>
            Air.Mapper.Mapper<TC1, S>.Map(_fixture.Create<TC1>());

        public BenchmarkFromToN1()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(_fixture);

            _source = NewSource();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<S, D>();

                cfg.CreateMap<TC0_Members, TC0_I0_Members>();
                cfg.CreateMap<TC0_Members, TC0_I1_Members>();
                cfg.CreateMap<TC0_Members, TC0_I2_Nullable_Members>();

                cfg.CreateMap<TC0_Members, TS0_I0_Members>();
                cfg.CreateMap<TC0_Members, TS0_I1_Members>();
                cfg.CreateMap<TC0_Members, TS0_I2_Nullable_Members>();

                cfg.CreateMap<TS0_Members, TC0_I0_Members>();
                cfg.CreateMap<TS0_Members, TC0_I1_Members>();
                cfg.CreateMap<TS0_Members, TC0_I2_Nullable_Members>();

                cfg.CreateMap<TS0_Members, TS0_I0_Members>();
                cfg.CreateMap<TS0_Members, TS0_I1_Members>();
                cfg.CreateMap<TS0_Members, TS0_I2_Nullable_Members>();

            });
            _autoMapper = mapperConfig.CreateMapper();


            Nelibur.ObjectMapper.TinyMapper.Bind<S, D>();

            Nelibur.ObjectMapper.TinyMapper.Bind<TC0_Members, TC0_I0_Members>();
            Nelibur.ObjectMapper.TinyMapper.Bind<TC0_Members, TC0_I1_Members>();
            Nelibur.ObjectMapper.TinyMapper.Bind<TC0_Members, TC0_I2_Nullable_Members>();

            Nelibur.ObjectMapper.TinyMapper.Bind<TC0_Members, TS0_I0_Members>();
            Nelibur.ObjectMapper.TinyMapper.Bind<TC0_Members, TS0_I1_Members>();
            Nelibur.ObjectMapper.TinyMapper.Bind<TC0_Members, TS0_I2_Nullable_Members>();

            Nelibur.ObjectMapper.TinyMapper.Bind<TS0_Members, TC0_I0_Members>();
            Nelibur.ObjectMapper.TinyMapper.Bind<TS0_Members, TC0_I1_Members>();
            Nelibur.ObjectMapper.TinyMapper.Bind<TS0_Members, TC0_I2_Nullable_Members>();

            Nelibur.ObjectMapper.TinyMapper.Bind<TS0_Members, TS0_I0_Members>();
            Nelibur.ObjectMapper.TinyMapper.Bind<TS0_Members, TS0_I1_Members>();
            Nelibur.ObjectMapper.TinyMapper.Bind<TS0_Members, TS0_I2_Nullable_Members>();


            ExpressMapper.Mapper.Register<S, D>();

            ExpressMapper.Mapper.Register<TC0_Members, TC0_I0_Members>();
            ExpressMapper.Mapper.Register<TC0_Members, TC0_I1_Members>();
            ExpressMapper.Mapper.Register<TC0_Members, TC0_I2_Nullable_Members>();

            ExpressMapper.Mapper.Register<TC0_Members, TS0_I0_Members>();
            ExpressMapper.Mapper.Register<TC0_Members, TS0_I1_Members>();
            ExpressMapper.Mapper.Register<TC0_Members, TS0_I2_Nullable_Members>();

            ExpressMapper.Mapper.Register<TS0_Members, TC0_I0_Members>();
            ExpressMapper.Mapper.Register<TS0_Members, TC0_I1_Members>();
            ExpressMapper.Mapper.Register<TS0_Members, TC0_I2_Nullable_Members>();

            ExpressMapper.Mapper.Register<TS0_Members, TS0_I0_Members>();
            ExpressMapper.Mapper.Register<TS0_Members, TS0_I1_Members>();
            ExpressMapper.Mapper.Register<TS0_Members, TS0_I2_Nullable_Members>();
        }

        [Benchmark]
        public D ExpressMapperMap() => ExpressMapper.Mapper.Map<S, D>(_source);

        [Benchmark]
        public D AgileMapperMap() => AgileObjects.AgileMapper.Mapper.Map(_source).ToANew<D>();

        //for whatever reasons it will crash
        //[Benchmark]
        //public D TinyMapperMap() => Nelibur.ObjectMapper.TinyMapper.Map<D>(_source);

        [Benchmark]
        public D AutoMapperMap() => _autoMapper.Map<D>(_source);

        [Benchmark]
        public D MapsterMap() => _source.Adapt<D>();

        [Benchmark]
        public D AirMapperMap() => Air.Mapper.Mapper<S, D>.Map(_source);
    }
}
