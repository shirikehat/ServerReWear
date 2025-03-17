using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServerReWear.DTO;

namespace ServerReWear.Models;

public partial class ShiriDBContext : DbContext
{
    public User? GetUser(string userName)
    {
        return this.Users.FirstOrDefault(u => u.UserName == userName);
    }

    public Models.User? GetUser1(int id)
    {
        return this.Users.Where(u => u.UserId == id)
                            .FirstOrDefault();
    }

    public void DeleteCart(int cartId)
    {
        try
        {
            Cart c = new Cart()
            {
                CartId = cartId
            };
            this.Carts.Remove(c);
            this.SaveChanges();
        }
        catch { }

    }

    public void DeleteWishlist(int wishlistId)
    {
        try
        {
            WishList w = new WishList()
            {
                WishlistId = wishlistId
            };
            this.WishLists.Remove(w);
            this.SaveChanges();
        }
        catch { }

    }
}

