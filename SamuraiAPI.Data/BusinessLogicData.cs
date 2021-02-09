using Microsoft.EntityFrameworkCore;
using SamuraiAPI.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamuraiAPI.Data
{
    public class BusinessLogicData
    {
        private SamuraiContext _context;

        public BusinessLogicData(SamuraiContext context)
        {
            _context = context;
        }

        public async Task<List<Samurai>> GetAllSamurais()
        {
            return await _context.Samurais.ToListAsync();
        }

        public async ValueTask<Samurai> GetSamuraiById(int id)
        {
            return await _context.Samurais.FindAsync(id);
        }

        public async Task<bool> UpdateSamurai(Samurai samurai)
        {
            _context.Entry(samurai).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Samurais.Any(e => e.Id == samurai.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Samurai> AddNewSamurai(Samurai samurai)
        {
            _context.Samurais.Add(samurai);
            await _context.SaveChangesAsync(); 
            return samurai;    
        }

        public async Task<Samurai> DeleteSamurai(int id)
        {
            var samurai = await _context.Samurais.FindAsync(id);
            if (samurai == null)
            {
                return null;
            }
            _context.Samurais.Remove(samurai); //Use Remove not Context Entry,retrieved samurai object, no related data attached to samurai (not a graph)
            await _context.SaveChangesAsync();
            return samurai;
        }
    }
}

