using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    public class Order
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("deliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("items")]
        public List<OrderDetail> Items { get; set; }

        [JsonProperty("lines")]
        public int Lines { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }




        [JsonProperty("value")]
        public decimal Value { get; set; }


        [JsonProperty("orderDateLocal")]
        public DateTime? OrderDateLocal
        {
            get
            {
                if (this.OrderDate == null)
                {
                    return null;
                }

                return this.OrderDate.ToLocalTime();
            }
        }
    }
}
