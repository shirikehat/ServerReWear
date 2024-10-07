using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

[Table("WishList")]
public partial class WishList
{
    [Key]
    public int WishlistId { get; set; }

    public int? UserId { get; set; }

    public int? ProductCode { get; set; }

    [ForeignKey("ProductCode")]
    [InverseProperty("WishLists")]
    public virtual Product? ProductCodeNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("WishLists")]
    public virtual User? User { get; set; }
}
