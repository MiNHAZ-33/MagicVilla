using MagicVilla_Villa_API.Models;

namespace MagicVilla_Villa_API.Repository.IRepository
{
    public interface IVillaNumberRespository : IRepository<VillaNumber>
    {
        Task Update(VillaNumber entity);
    }
}
