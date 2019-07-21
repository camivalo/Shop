using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Models
{
    using Newtonsoft.Json;
    using System;
    public class OrderDetail
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("product")]
        public Product Product { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }

        [JsonProperty("value")]
        public decimal Value { get { return this.Price * (decimal)this.Quantity; } set { } }
    }
}
