using Cipolatti.API.Interfaces;
using Cipolatti.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cipolatti.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoVolumeShoppingController : Controller
    {
        private readonly IMovimentacaoVolumeShoppingRepository _movimentacaoVolumeShoppingRepository;

        public MovimentacaoVolumeShoppingController(IMovimentacaoVolumeShoppingRepository movimentacaoVolumeShoppingRepository)
        {
            _movimentacaoVolumeShoppingRepository = movimentacaoVolumeShoppingRepository;
        }

        [HttpPost("GravarVolume")]
        public async Task<ActionResult> CadastrarVolume(TblMovimentacaoVolumeShopping endercoVolume)
        {
            var volume = await _movimentacaoVolumeShoppingRepository.SelecionarByBarcode(endercoVolume.BarcodeVolume);
            if (volume == null)
            {
                _movimentacaoVolumeShoppingRepository.Incluir(endercoVolume);
                if (await _movimentacaoVolumeShoppingRepository.SaveAllAsync())
                {
                    return Ok("Volume enviado com sucesso!");
                }

                return BadRequest("Ocorreu um erro ao enviar o volume.");
            }
            return Ok("Nada a fazer.");
        }
    }
}
