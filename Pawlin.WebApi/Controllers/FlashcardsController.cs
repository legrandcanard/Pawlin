using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;
using Pawlin.WebApi.Dto;

namespace Pawlin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashcardsController(IFlashcardRepository flashcardRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<FlashcardDto>> Create([FromBody] FlashcardCreateDto dto)
        {
            var entity = dto.Adapt<Flashcard>();
            await flashcardRepository.AddAsync(entity);

            var created = await flashcardRepository.GetByIdAsync(entity.Id);
            var result = created.Adapt<FlashcardDto>();

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FlashcardDto>> GetById(int id)
        {
            var entity = await flashcardRepository.GetByIdAsync(id);
            if (entity is null) 
                return NotFound();

            return Ok(entity.Adapt<FlashcardDto>());
        }

        [HttpGet("byDeck/{deckId:int}")]
        public async Task<ActionResult<FlashcardDto[]>> GetByDeck(int deckId)
        {
            var list = await flashcardRepository.GetByDeckIdAsync(deckId);
            return Ok(list.Adapt<FlashcardDto[]>());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] FlashcardDto dto)
        {
            if (id != dto.Id) return BadRequest("Route id and payload id must match.");

            var entity = dto.Adapt<Flashcard>();
            await flashcardRepository.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await flashcardRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}