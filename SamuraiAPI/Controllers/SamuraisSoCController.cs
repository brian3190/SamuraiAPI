using Microsoft.AspNetCore.Mvc;
using SamuraiAPI.Data;
using SamuraiAPI.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamuraisSoCController : ControllerBase //Separation of Concerns
    {
        private readonly BusinessLogicData _bizdata;

        public SamuraisSoCController(BusinessLogicData bizdata)
        {
            _bizdata = bizdata;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Samurai>>> GetSamurais()
        {
            var samurais = await _bizdata.GetAllSamurais();
            return Ok(samurais);
        }

        // GET: api/Samurais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Samurai>> GetSamurai(int id)
        {
            var samurai = await _bizdata.GetSamuraiById(id);
            if (samurai == null)
            {
                return NotFound();
            }
            return Ok(samurai);
        }

        // PUT: api/Samurais/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSamurai(int id, Samurai samurai)
        {
            if (id != samurai.Id)
            {
                return BadRequest();
            }
            try
            {
                bool result = await _bizdata.UpdateSamurai(samurai);

                if (result == false)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Samurais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Samurai>> PostSamurai(Samurai samurai)
        {
            var addedSamurai = await _bizdata.AddNewSamurai(samurai);

            return CreatedAtAction("GetSamurai", new { id = samurai.Id }, addedSamurai);
        }

        // DELETE: api/Samurais/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Samurai>> DeleteSamurai(int id)
        {
            return await _bizdata.DeleteSamurai(id);
        }
    }
}
