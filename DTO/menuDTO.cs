namespace backendrestaurante.Dtos
{
    public class MenuItemDto
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string? Tipo { get; set; }
    }
}