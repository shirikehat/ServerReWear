using ServerReWear.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerReWear.DTO
{
    public class ProductDTO
    {
        public int ProductCode { get; set; }

        public int Price { get; set; }

        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserProfile { get; set; } = "";
        public string? Size { get; set; }

        public int? StatusId { get; set; }

        public int? TypeId { get; set; }
        public string? Store { get; set; }
        public string? Description { get; set; }
        public string ProductImagePath { get; set; } = "";
       
        public ProductDTO() { }

        public ProductDTO(Models.Product product)
        {
            ProductCode = product.ProductCode;
            Price = product.Price;
            UserId = product.UserId;
            Size = product.Size;
            StatusId = product.StatusId;
            TypeId = product.TypeId;
            if (product.User != null)
            {
                UserName = product.User.UserName;
                //UserProfile = product.User.ProfileImagePath;
            }
        }

        public Models.Product GetModel()
        {
            return new Models.Product()
            {
                ProductCode = this.ProductCode,
                Price = this.Price,
                UserId = this.UserId,
                Size = this.Size,
                StatusId = this.StatusId,
                TypeId = this.TypeId
            };
        }
        
    }
}
