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
}

