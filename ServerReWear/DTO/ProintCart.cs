using ServerReWear.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerReWear.DTO
{
    public class ProintCart
    {
        public int? ProductCode { get; set; }

        public int? CartId { get; set; }

        public virtual Cart? Cart { get; set; }

        public virtual Product? ProductCodeNavigation { get; set; }

        public ProintCart(Models.ProintCart pc)
        {
            this.ProductCode = pc.ProductCode;
            this.CartId = pc.CartId;
            this.ProductCodeNavigation = pc.ProductCodeNavigation;
            this.Cart = pc.Cart;
        }
    }
}
