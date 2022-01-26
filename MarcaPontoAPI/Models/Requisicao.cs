namespace MarcaPontoAPI.Models
{
    public class Requisicao
    {
        public DateTime IncludedAt { get; set; }
        public int EmployeeId { get; set; } // validar se quantidade de empregados cabe em int
        public int EmployerId { get; set; } // validar se quantidade de empresas cabe em int
    }
}
