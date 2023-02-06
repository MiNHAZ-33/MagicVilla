using MagicVilla_Villa_API.Models;
using System.Linq.Expressions;

namespace MagicVilla_Villa_API.Repository.IRepository
{
    public interface IVillaRespository : IRepository<Villa>
    {
        Task Update(Villa entity);
    }
}
