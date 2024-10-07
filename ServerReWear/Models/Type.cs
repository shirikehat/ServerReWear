using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

public partial class Type
{
    [Key]
    public int TypeCode { get; set; }

    [StringLength(15)]
    public string? Name { get; set; }

    [InverseProperty("Type")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
