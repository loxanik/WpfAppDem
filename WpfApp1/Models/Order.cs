using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Order
{
    public int Id { get; set; }

    public string Article { get; set; } = null!;

    public DateOnly DateOfOrder { get; set; }

    public DateOnly DateOfPickup { get; set; }

    public int PickupAddressId { get; set; }

    public string ClientName { get; set; } = null!;

    public int Code { get; set; }

    public int StatusId { get; set; }

    public virtual PickupPoint PickupAddress { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
