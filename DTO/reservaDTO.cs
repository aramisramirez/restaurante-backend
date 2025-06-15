namespace backendrestaurante.Dtos
{
    public class ReservaDto
    {
        public int? ClienteId { get; set; }
        public int? MesaId { get; set; }
        public DateTime Fecha { get; set; }
        public int NumeroPersonas { get; set; }
        public string? Estado { get; set; }
    }
}