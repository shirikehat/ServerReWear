﻿using ServerReWear.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerReWear.DTO
{
    public class ProductDTO
    {
        public int ProductCode { get; set; }

        public int Price { get; set; }

        public int UserId { get; set; }
        public string? Size { get; set; }

        public int StatusId { get; set; }

        public int TypeId { get; set; }
        public string? Store { get; set; }
        public string? Description { get; set; }
        public string ProductImagePath { get; set; } = "";
        public UserDTO? User { get; set; }
       
        public ProductDTO() { }

        public ProductDTO(Models.Product product, string wwwRoot = "")
        {
            ProductCode = product.ProductCode;
            Price = product.Price;
            UserId = product.UserId;
            Size = product.Size;
            StatusId = product.StatusId;
            TypeId = product.TypeId;
            Store = product.Store;
            Description = product.Description;
            if (product.User != null)
            {
                User = new UserDTO(product.User);
               
            }
            ProductImagePath = GetProductImageVirtualPath(ProductCode, wwwRoot);
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
                TypeId = this.TypeId,
                Store = this.Store,
                Description = this.Description
            };
        }

        private string GetProductImageVirtualPath(int productId, string wwwRoot)
        {
            string virtualPath = $"/productImages/{productId}";
            string path = $"{wwwRoot}\\productImages\\{productId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{wwwRoot}\\productImages\\{productId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/productImages/product.png";
                }
            }

            return virtualPath;
        }

    }
}
