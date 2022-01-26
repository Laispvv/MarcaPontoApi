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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Home do BatePontoAPI");
        }

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

                var client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(_address, requisicao);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (InvalidDataException)
            {
                return BadRequest("Data de marcação inválida");
            }
        }
    }
}
