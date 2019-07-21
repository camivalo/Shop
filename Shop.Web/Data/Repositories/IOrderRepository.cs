namespace Shop.Web.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Shop.Web.Models;

    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string userName);


        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);

        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task<bool> AddItemToOrderFromAppforOrderTempAsync(List<OrderDetailTemp> model);

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);

        Task DeleteDetailTempAsync(int id);

        Task DeleteOrderAsync(int id);

        Task<bool> ConfirmOrderAsync(string userName);

        Task<bool> ConfirmOrderFromAPPAsync(List<OrderDetailTemp> model);

        Task DeliverOrder(DeliverViewModel model);

        Task<Order> GetOrdersAsync(int id);


        Task<List<OrderDetail>> GetOrdersDetailAsync(int id);


    }

}
