using System;
using System.Collections.Generic;

namespace backendrestaurante.Models;

public partial class Factura
{
    public int FacturaId { get; set; }

    public int? ReservaId { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal? Total { get; set; }

    public virtual Reserva? Reserva { get; set; }
}
