using ScannerQRcode.Data.Api.Models;
using System.Text.Json;

namespace ScannerQRcode.Data.Api.Repositorios
{
    public class AprovadoRepositorio
    {
        public static List<Aprovado> Aprovados()
        {
            var url = $@"http://localhost:5293/api/Aprovado/SelecionarTodos";
            var resposta = Data.Api.HttpClientUtil.ConsHttpClientAsync(url);

            List<Aprovado> aprovados = JsonSerializer.Deserialize<List<Aprovado>>(resposta.Result);

            return aprovados;

        }
    }
}
