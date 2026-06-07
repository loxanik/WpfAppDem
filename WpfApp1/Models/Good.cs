using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Good
{
    public int Id { get; set; }

    public int UnitId { get; set; }

    public decimal Price { get; set; }

    public int SupplierId { get; set; }

    public int ManufactorerId { get; set; }

    public int CategoryId { get; set; }

    public int Discount { get; set; }

    public int Amount { get; set; }

    public string Description { get; set; } = null!;

    public string? PhotoPath { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufactorer Manufactorer { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
