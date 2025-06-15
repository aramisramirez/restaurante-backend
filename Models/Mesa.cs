using System;
using System.Collections.Generic;

namespace backendrestaurante.Models;

public partial class Mesa
{
    public int MesaId { get; set; }

    public int Numero { get; set; }

    public int Capacidad { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
