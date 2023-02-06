using MagicVilla_Villa_API.Data;
using MagicVilla_Villa_API.Models;
using MagicVilla_Villa_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_Villa_API.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRespository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        public async Task Update(Villa entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
