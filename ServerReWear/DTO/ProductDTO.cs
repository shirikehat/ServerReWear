﻿using ServerReWear.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerReWear.DTO
{
    public class ProductDTO
    {
        public int ProductCode { get; set; }

        public int? Price { get; set; }

        public int? UserId { get; set; }

        public string? Size { get; set; }

        public int? StatusId { get; set; }

        public int? TypeId { get; set; }
        public string ImagePath { get; set; } = "";
        public ProductDTO() { }

        public ProductDTO(Models.Product product)
        {
            ProductCode = product.ProductCode;
            Price = product.Price;
            UserId = product.UserId;
            Size = product.Size;
            StatusId = product.StatusId;
            TypeId = product.TypeId;
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
