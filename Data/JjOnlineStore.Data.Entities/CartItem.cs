﻿using JjOnlineStore.Data.Entities.Base;

namespace JjOnlineStore.Data.Entities
{
    public class CartItem : BaseDeletableModel<long>
    {
        public long ProductId { get; set; }

        public Product Product { get; set; }

        public short Quantity { get; set; }

        public long CartId { get; set; }

        public Cart Cart { get; set; }

        public decimal TotalSum() =>
            Product.Price * Quantity;
    }
}