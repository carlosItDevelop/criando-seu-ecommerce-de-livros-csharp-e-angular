﻿namespace Bookstore.Domain.Entities
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Img { get; set; }
    }
}
