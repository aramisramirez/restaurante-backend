namespace backendrestaurante.Dtos
{
    public class FacturaDto
    {
        public int ReservaId { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? Total { get; set; }
    }
}