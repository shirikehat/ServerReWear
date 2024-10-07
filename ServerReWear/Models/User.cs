using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string? UserName { get; set; }

    [StringLength(100)]
    public string? Password { get; set; }

    [StringLength(10)]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("User")]
    public virtual ICollection<WishList> WishLists { get; set; } = new List<WishList>();
}
