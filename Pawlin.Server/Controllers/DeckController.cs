using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;
using Pawlin.WebApi.Dto;

namespace Pawlin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController(IDeckRepository deckRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<DeckDto>> Create([FromBody] DeckCreateDto dto)
        {
            var entity = dto.Adapt<Deck>();
            entity.CreatorUserId = AppConsts.UserId;

            var created = await deckRepository.AddDeck(entity);
            var result = created.Adapt<DeckDto>();

            return CreatedAtAction(nameof(GetById), new { deckId = created.Id }, result);
        }

        [HttpGet("{deckId:int}")]
        public async Task<ActionResult<DeckDto>> GetById(int deckId)
        {
            try
            {
                var deck = await deckRepository.GetDeck(deckId);
                return Ok(deck.Adapt<DeckDto>());
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("byCurrentUser")]
        public async Task<ActionResult<DeckDto[]>> GetByCurrentUser()
        {
            var decks = await deckRepository.GetDecksByUserId(AppConsts.UserId);
            return Ok(decks.Adapt<DeckDto[]>());
        }

        [HttpPut("{deckId:int}")]
        public async Task<IActionResult> Update(int deckId, [FromBody] DeckDto dto)
        {
            if (deckId != dto.Id)
                return BadRequest("Route id and payload id must match.");

            var entity = dto.Adapt<Deck>();
            try
            {
                await deckRepository.UpdateDeck(entity);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{deckId:int}")]
        public async Task<IActionResult> Delete(int deckId)
        {
            try
            {
                await deckRepository.DeleteDeck(deckId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}