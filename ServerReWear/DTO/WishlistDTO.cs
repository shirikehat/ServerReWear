using ServerReWear.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerReWear.DTO
{
    public class WishlistDTO
    {
        public int WishlistId { get; set; }

        public int? UserId { get; set; }

        public int? ProductCode { get; set; }

        public virtual Product? ProductCodeNavigation { get; set; }

        public virtual User? User { get; set; }

        public WishlistDTO() { }

        public WishlistDTO(Models.WishList w)
        {
            WishlistId = w.WishlistId;
            UserId = w.UserId;
            ProductCode = w.ProductCode;

        }

        public Models.WishList GetModel()
        {
            return new Models.WishList()
            {
                WishlistId = this.WishlistId,
                UserId = this.UserId,
                ProductCode = this.ProductCode,
            };
        }
    }
}
