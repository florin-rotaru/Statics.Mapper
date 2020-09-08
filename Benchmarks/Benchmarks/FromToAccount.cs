using AutoFixture;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using ExpressionDebugger;
using Mapster;
using Models;
using System.Linq;

namespace Benchmarks
{
    [InProcess]
    [MemoryDiagnoser]
    public class From_Account_To_AccountDto
    {
        private readonly Account _source;
        private readonly IMapper _autoMapper;
        private readonly Fixture _fixture;

        public From_Account_To_AccountDto()
        {
            TypeAdapterConfig.GlobalSettings.SelfContainedCodeGeneration = true;
            var account = default(Account);
            var def = new ExpressionDefinitions
            {
                IsStatic = true,    //change to false if you want instance
                MethodName = "Map",
                Namespace = "YourNamespace",
                TypeName = "CustomerMapper"
            };
            var code = account.BuildAdapter()
                .CreateMapExpression<AccountDto>()
                .ToScript(def);


            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(_fixture);

            _source = _fixture.Create<Account>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<OrderItem, OrderItemDto>();
                cfg.CreateMap<Order, OrderDto>();
                cfg.CreateMap<Account, AccountDto>();
            });
            _autoMapper = mapperConfig.CreateMapper();

        }

        [Benchmark]
        public AccountDto AutoMapperMap() => _autoMapper.Map<AccountDto>(_source);

        [Benchmark]
        public AccountDto MapsterMap() => _source.Adapt<AccountDto>();

        [Benchmark]
        public AccountDto AirMapperMap() => Air.Mapper.Mapper<Account, AccountDto>.Map(_source);
    }
}
