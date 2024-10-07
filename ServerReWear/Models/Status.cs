using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

[Table("Status")]
public partial class Status
{
    [Key]
    public int StatusCode { get; set; }

    [StringLength(15)]
    public string? Name { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
