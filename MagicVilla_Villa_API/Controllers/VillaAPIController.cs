using MagicVilla_Villa_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Villa_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : Controller
    {
        [HttpGet]
        public IEnumerable<Villa> GetVillas()
        {
            return new List<Villa>
            {
                new Villa {Id=1, Name="Heaven"},
                new Villa {Id=2, Name="Second"}
            };
        }
    }
}
