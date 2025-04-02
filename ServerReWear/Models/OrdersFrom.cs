using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

[Table("OrdersFrom")]
public partial class OrdersFrom
{
    [Key]
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public int? ProductCode { get; set; }

    [StringLength(100)]
    public string? Adress { get; set; }

    [ForeignKey("ProductCode")]
    [InverseProperty("OrdersFroms")]
    public virtual Product? ProductCodeNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("OrdersFroms")]
    public virtual User? User { get; set; }
}
