using System;
using System.Collections.Generic;

namespace backendrestaurante.Models;

public partial class Menu
{
    public int ItemId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Consumo> Consumos { get; set; } = new List<Consumo>();
}
