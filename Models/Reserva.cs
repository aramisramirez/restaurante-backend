using System;
using System.Collections.Generic;

namespace backendrestaurante.Models;

public partial class Reserva
{
    public int ReservaId { get; set; }

    public int? ClienteId { get; set; }

    public int? MesaId { get; set; }

    public DateTime Fecha { get; set; }

    public int NumeroPersonas { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<Consumo> Consumos { get; set; } = new List<Consumo>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Mesa? Mesa { get; set; }
}
