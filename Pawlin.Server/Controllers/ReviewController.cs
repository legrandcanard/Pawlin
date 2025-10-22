using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pawlin.Common.Repositories;
using Pawlin.Common.Services;
using Pawlin.WebApi.Dto;

namespace Pawlin.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController(
        IFlashcardReviewService flashcardReviewService,
        IFlashcardRepository flashcardRepository,
        IDeckRepository deckRepository) 
        : ControllerBase
    {

        [HttpGet("{deckInstanceId:int}/next")]
        public async Task<ActionResult<FlashcardDto>> GetNextCard(int deckInstanceId)
        {
            var deckInstance = await deckRepository.GetDeckInstance(deckInstanceId);
            var nextFlashcard = await flashcardReviewService.GetNextFlashcard(deckInstance);

            return nextFlashcard.Adapt<FlashcardDto>();
        }

        [HttpPost("{deckInstanceId:int}/review")]
        public async Task<ActionResult<ReviewDataItemDto>> ReviewFlashcard(int deckInstanceId, [FromBody] ReviewDto dto)
        {
            var deckInstance = await deckRepository.GetDeckInstance(deckInstanceId);
            var flashcard = await flashcardRepository.GetByIdAsync(dto.FlashcardId);

            var reviewData = await flashcardReviewService.Review(deckInstance, flashcard!, dto.Quality);
            return reviewData.Adapt<ReviewDataItemDto>();
        }
    }
}
