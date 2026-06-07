using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
