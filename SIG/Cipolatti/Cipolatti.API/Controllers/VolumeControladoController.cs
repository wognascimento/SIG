using Cipolatti.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cipolatti.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolumeControladoController : Controller
    {
        private readonly IVolumeControladoRepository _volumeControladoRepository;
    }
}
