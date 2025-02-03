using ServerReWear.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerReWear.DTO
{
    public class ProinWish
    {
        public int? ProductCode { get; set; }

        public int? WishlistId { get; set; }

        public virtual Product? ProductCodeNavigation { get; set; }

        public virtual WishList? Wishlist { get; set; }

        public ProinWish(Models.ProinWish pw)
        {
            this.ProductCode = pw.ProductCode;
            this.WishlistId = pw.WishlistId;
            this.ProductCodeNavigation = pw.ProductCodeNavigation;
            this.Wishlist = pw.Wishlist;
        }
    }
}
