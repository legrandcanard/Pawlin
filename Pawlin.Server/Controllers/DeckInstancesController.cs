using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;
using Pawlin.Server.Dto;

namespace Pawlin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckInstancesController(IDeckRepository deckRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<DeckInstanceDto>> Create([FromBody] DeckInstanceDto dto)
        {
            var entity = dto.Adapt<DeckInstance>();
            await deckRepository.AddDeckInstance(entity);

            var created = await deckRepository.GetDeckInstance(entity.Id);
            var result = created.Adapt<DeckInstanceDto>();

            return CreatedAtAction(nameof(GetById), new { deckInstanceId = created.Id }, result);
        }

        [HttpGet("{deckInstanceId:int}")]
        public async Task<ActionResult<DeckInstanceDto>> GetById(int deckInstanceId)
        {
            try
            {
                var instance = await deckRepository.GetDeckInstance(deckInstanceId);
                return Ok(instance.Adapt<DeckInstanceDto>());
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("byCurrentUser")]
        public async Task<ActionResult<DeckInstanceDto[]>> GetByUser()
        {
            var instances = await deckRepository.GetDeckInstancesByUserId(AppConsts.UserId);
            return Ok(instances.Adapt<DeckInstanceDto[]>());
        }

        [HttpPut("{deckInstanceId:int}")]
        public async Task<IActionResult> Update(int deckInstanceId, [FromBody] DeckInstanceDto dto)
        {
            if (deckInstanceId != dto.Id)
                return BadRequest("Id in route and payload must match.");

            var entity = dto.Adapt<DeckInstance>();
            try
            {
                await deckRepository.UpdateDeckInstance(entity);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{deckInstanceId:int}")]
        public async Task<IActionResult> Delete(int deckInstanceId)
        {
            try
            {
                await deckRepository.DeleteDeckInstance(deckInstanceId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}