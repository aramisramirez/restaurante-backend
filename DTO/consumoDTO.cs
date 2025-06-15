namespace backendrestaurante.Dtos
{
    public class ConsumoDto
    {
        public int ReservaId { get; set; }
        public int ItemId { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}