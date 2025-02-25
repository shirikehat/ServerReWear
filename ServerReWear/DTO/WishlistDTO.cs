using ServerReWear.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerReWear.DTO
{
    public class WishlistDTO
    {
        public int WishlistId { get; set; }

        public int? UserId { get; set; }

        public int? ProductCode { get; set; }

        public virtual ProductDTO? ProductCodeNavigation { get; set; }

        public virtual User? User { get; set; }

        public WishlistDTO() { }

        public WishlistDTO(Models.WishList w, string wwwRoot)
        {
            WishlistId = w.WishlistId;
            UserId = w.UserId;
            ProductCode = w.ProductCode;
            if (w.ProductCodeNavigation != null)
                ProductCodeNavigation = new ProductDTO(w.ProductCodeNavigation, wwwRoot);
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
