using MagicVilla_Villa_API.Data;
using MagicVilla_Villa_API.Models;
using MagicVilla_Villa_API.Repository.IRepository;

namespace MagicVilla_Villa_API.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRespository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
