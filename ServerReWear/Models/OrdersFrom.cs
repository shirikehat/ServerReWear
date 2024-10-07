using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

[Keyless]
[Table("OrdersFrom")]
public partial class OrdersFrom
{
    public int? UserId { get; set; }

    public int? ProductCode { get; set; }

    [StringLength(100)]
    public string? Adress { get; set; }

    [ForeignKey("ProductCode")]
    public virtual Product? ProductCodeNavigation { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}
