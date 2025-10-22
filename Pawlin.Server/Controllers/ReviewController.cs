using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;
using Pawlin.Common.Services;
using Pawlin.Data;
using Pawlin.Data.Repositories;
using Pawlin.Server.Dto;

namespace Pawlin.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController(
        IFlashcardReviewService flashcardReviewService,
        IFlashcardRepository flashcardRepository,
        DeckRepository deckRepository) 
        : ControllerBase
    {

        [HttpGet("next")]
        public async Task<FlashcardDto> GetNextCard(int deckInstanceId)
        {
            var deckInstance = await deckRepository.GetDeckInstance(deckInstanceId);

            var nextFlashcard = flashcardReviewService.GetNextFlashcard(deckInstance);

            return nextFlashcard.Adapt<FlashcardDto>();
        }

        [HttpPost("review")]
        public async Task ReviewFlashcard(int flashcardId, int deckInstanceId, int quality)
        {
            var deckInstance = await deckRepository.GetDeckInstance(deckInstanceId);
            var flashcard = await flashcardRepository.GetByIdAsync(flashcardId);

            await flashcardReviewService.Review(deckInstance, flashcard!, quality);
        }
    }
}
