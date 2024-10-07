using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

[Keyless]
[Table("ProinWish")]
public partial class ProinWish
{
    public int? ProductCode { get; set; }

    public int? WishlistId { get; set; }

    [ForeignKey("ProductCode")]
    public virtual Product? ProductCodeNavigation { get; set; }

    [ForeignKey("WishlistId")]
    public virtual WishList? Wishlist { get; set; }
}
