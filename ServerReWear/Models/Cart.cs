using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

[Table("Cart")]
public partial class Cart
{
    [Key]
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public int? ProductCode { get; set; }

    [ForeignKey("ProductCode")]
    [InverseProperty("Carts")]
    public virtual Product? ProductCodeNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual User? User { get; set; }
}
