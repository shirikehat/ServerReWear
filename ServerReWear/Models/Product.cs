using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

public partial class Product
{
    [Key]
    public int ProductCode { get; set; }

    public int Price { get; set; }

    public int UserId { get; set; }

    [StringLength(15)]
    public string? Size { get; set; }

    public int StatusId { get; set; }

    public int TypeId { get; set; }

    [StringLength(25)]
    public string? Store { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }

    [InverseProperty("ProductCodeNavigation")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("ProductCodeNavigation")]
    public virtual ICollection<OrdersFrom> OrdersFroms { get; set; } = new List<OrdersFrom>();

    [ForeignKey("StatusId")]
    [InverseProperty("Products")]
    public virtual Status Status { get; set; } = null!;

    [ForeignKey("TypeId")]
    [InverseProperty("Products")]
    public virtual Type Type { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Products")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("ProductCodeNavigation")]
    public virtual ICollection<WishList> WishLists { get; set; } = new List<WishList>();
}
