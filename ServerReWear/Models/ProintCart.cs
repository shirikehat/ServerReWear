using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

[Keyless]
[Table("ProintCart")]
public partial class ProintCart
{
    public int? ProductCode { get; set; }

    public int? CartId { get; set; }

    [ForeignKey("CartId")]
    public virtual Cart? Cart { get; set; }

    [ForeignKey("ProductCode")]
    public virtual Product? ProductCodeNavigation { get; set; }
}
