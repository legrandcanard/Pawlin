using Pawlin.Common.Repositories;
using Pawlin.Data.Repositories;

namespace Pawlin.Server
{
    public static class Setup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IDeckRepository, DeckRepository>();
            services.AddTransient<IFlashcardRepository, FlashcardRepository>();
            services.AddTransient<IReviewHistoryRepository, ReviewHistoryRepository>();

            return services;
        }
    }
}
