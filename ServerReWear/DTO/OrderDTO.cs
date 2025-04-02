using ServerReWear.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerReWear.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public int? ProductCode { get; set; }
        public string? Adress { get; set; }
        public virtual ProductDTO? ProductCodeNavigation { get; set; }

        public virtual User? User { get; set; }

        public OrderDTO() { }

        public OrderDTO(Models.OrdersFrom or, string wwwRoot)
        {
            OrderId = or.OrderId;
            UserId = or.UserId;
            ProductCode = or.ProductCode;
            Adress= or.Adress;
            if (or.ProductCodeNavigation != null)
                ProductCodeNavigation = new ProductDTO(or.ProductCodeNavigation, wwwRoot);


        }

        public Models.OrdersFrom GetModel()
        {
            return new Models.OrdersFrom()
            {
                OrderId = this.OrderId,
                UserId = this.UserId,
                ProductCode = this.ProductCode,
                Adress= this.Adress,
            };
        }
    }
}
