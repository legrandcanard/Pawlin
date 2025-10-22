using Pawlin.Common;
using Pawlin.Common.Repositories;
using Pawlin.Common.Services;
using Pawlin.Data.Repositories;

namespace Pawlin.WebApi
{
    public static class Setup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IDeckRepository, DeckRepository>();
            services.AddTransient<IFlashcardRepository, FlashcardRepository>();
            services.AddTransient<IReviewHistoryRepository, ReviewHistoryRepository>();
            services.AddTransient<IFlashcardReviewService, FlashcardReviewService>();
            services.AddTransient<IFlashcardReviewer, FlashcardReviewer>();

            return services;
        }
    }
}
