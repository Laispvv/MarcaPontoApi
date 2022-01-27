using MarcaPontoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace MarcaPontoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BatePontoController : ControllerBase
    {
        private readonly string _address = "https://api.mockytonk.com/proxy/ab2198a3-cafd-49d5-8ace-baac64e72222";
        SemaphoreSlim _semaphore = new SemaphoreSlim(5);

        // POST: BatePonto/Marcar
        [Route("Marcar"), HttpPost]
        public async Task<ActionResult> Marcar(string IncludedAt, int EmployeeId, int EmployerId)
        {
            try
            {
                DateTime date;
                DateTime.TryParse(IncludedAt, out date);

                if (EmployeeId == 0 || EmployeeId == 0 || date == default)
                    return BadRequest("Data inválida ou código de empresa ou funcionário incorretos");

                var requisicao = new Requisicao() { IncludedAt = date, EmployeeId = EmployeeId, EmployerId = EmployerId };

                var result = MarcarApiLegado(requisicao);
                Task.WaitAny(result);
                if (!result.Result)
                    return BadRequest("Houve um problema na marcação de ponto");
                return Ok("Ponto Marcado!");
            }
            catch (InvalidDataException)
            {
                return BadRequest("Data de marcação inválida");
            }
        }
        [Route("MarcarApiLegado"), HttpPost]
        private async Task<bool> MarcarApiLegado(Requisicao requisicao)
        {
            var client = new HttpClient();
            await _semaphore.WaitAsync();
            HttpResponseMessage response = await client.PostAsJsonAsync(_address, requisicao);
            _semaphore.Release();
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
    }
}
