using System;
using System.Collections.Generic;

namespace backendrestaurante.Models;

public partial class Consumo
{
    public int ConsumoId { get; set; }

    public int? ReservaId { get; set; }

    public int? ItemId { get; set; }

    public int? Cantidad { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Menu? Item { get; set; }

    public virtual Reserva? Reserva { get; set; }
}
