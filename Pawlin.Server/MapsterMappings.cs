using System;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Pawlin.Common.Entities;
using Pawlin.Server.Dto;

namespace Pawlin.Common.Dtos
{
    public static class MapsterMappings
    {
        public static void RegisterMappings(TypeAdapterConfig config)
        {
            if (config is null) throw new ArgumentNullException(nameof(config));

            config.NewConfig<Flashcard, FlashcardDto>().TwoWays();
            config.NewConfig<Flashcard, FlashcardCreateDto>().TwoWays();
            config.NewConfig<Deck, DeckDto>().TwoWays();
            config.NewConfig<Deck, DeckCreateDto>().TwoWays();
            config.NewConfig<DeckInstance, DeckInstanceDto>().TwoWays();
            config.NewConfig<User, UserDto>().TwoWays();
            config.NewConfig<ReviewDataItem, ReviewDataItemDto>().TwoWays();
        }

        public static IServiceCollection AddMapsterMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            RegisterMappings(config);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}