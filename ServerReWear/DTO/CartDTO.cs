using ServerReWear.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace ServerReWear.DTO
{
    public class CartDTO
    {
        public int CartId { get; set; }

        public int? UserId { get; set; }

        public int? ProductCode { get; set; }

        public virtual Product? ProductCodeNavigation { get; set; }

        public virtual User? User { get; set; }

        public CartDTO() { }

        public CartDTO(Models.Cart cart)
        {
            CartId= cart.CartId;
            UserId= cart.UserId;
            ProductCode = cart.ProductCode;
            

        }

        public Models.Cart GetModel()
        {
            return new Models.Cart()
            {
                CartId = this.CartId,
                UserId = this.UserId,
                ProductCode = this.ProductCode,
            };
        }
    }
}
