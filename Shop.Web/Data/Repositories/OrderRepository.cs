﻿namespace Shop.Web.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Helpers;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Models;

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await this.userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return this.context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return this.context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task<IQueryable<Order>> GetOrdersTempAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await this.userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return this.context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return this.context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var product = await this.context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await this.context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                this.context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                this.context.OrderDetailTemps.Update(orderDetailTemp);
            }

            await this.context.SaveChangesAsync();
        }


        public async Task<bool>  AddItemToOrderFromAppforOrderTempAsync(List<OrderDetailTemp> model)
        {
            var user = await this.userHelper.GetUserByEmailAsync(model.Select(u => u.User.UserName).First());
            if (user == null)
            {
                return false;
            }

            foreach (OrderDetailTemp m in model)
            {
                var tmp = new AddItemViewModel
                {
                    ProductId = m.Product.Id,
                    Quantity = m.Quantity
                };

                await AddItemToOrderAsync(tmp, user.ToString());
            }
            return true;
        }

            public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                this.context.OrderDetailTemps.Update(orderDetailTemp);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            var orderSelected = await this.context.Orders.FindAsync(id);
            if (orderSelected == null)
            {
                return;
            }

            //var orderDetail = orderSelected.Where(o => o.Id == product.Id).FirstOrDefault();
            var orderDetail = this.context.OrderDetails.Where(o => o.OrderId == orderSelected.Id).ToList();

            foreach(OrderDetail o in orderDetail)
            {
                this.context.OrderDetails.Remove(o);
                await this.context.SaveChangesAsync();

            }
           


            this.context.Orders.Remove(orderSelected);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            this.context.OrderDetailTemps.Remove(orderDetailTemp);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }

            var orderTmps = await this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            this.context.Orders.Add(order);
            this.context.OrderDetailTemps.RemoveRange(orderTmps);
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmOrderFromAPPAsync(List<OrderDetailTemp> model)
        {
            var user = await this.userHelper.GetUserByEmailAsync(model.Select(u => u.User.UserName).First());
            if (user == null)
            {
                return false;
            }

            foreach (OrderDetailTemp m in model)
            {
                var tmp = new AddItemViewModel
                {
                    ProductId = m.Product.Id,
                    Quantity = m.Quantity
                };

                await AddItemToOrderAsync(tmp, user.ToString());
            }





            var orderTmps = await this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = model.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            this.context.Orders.Add(order);
            this.context.OrderDetailTemps.RemoveRange(orderTmps);
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task DeliverOrder(DeliverViewModel model)
        {
            var order = await this.context.Orders.FindAsync(model.Id);
            if (order == null)
            {
                return;
            }

            order.DeliveryDate = model.DeliveryDate;
            this.context.Orders.Update(order);
            await this.context.SaveChangesAsync();
        }

        public async Task<Order> GetOrdersAsync(int id)
        {
            return await this.context.Orders.FindAsync(id);
        }



        public async Task<List<OrderDetail>> GetOrdersDetailAsync(int id)
        {

            //var order= await this.context.Orders
            //   .Include(o => o.Items)
            //   .Where(o => o.Id == id)
            //   .ToListAsync();

            //var details = order.Select(o => new OrderDetail
            //{
            //    Price = o.Price,
            //    Product = o.Product,
            //    Quantity = o.Quantity
            //}).ToList();


            var result = await this.context.OrderDetails
                    .Include(o => o.Product)
                    .Where(o => o.OrderId == id)
                    .ToListAsync();
            return result;

        }

        public async Task <Order> ModifyOrderDeliveryDateAsync(int id, Common.Models.Order order)
        {
            var orderNew = await this.context.Orders.FindAsync(id);
            //if (orderDetailTemp == null)
            //{
            //    return;
            //}

            orderNew.DeliveryDate = order.DeliveryDate;
             this.context.Orders.Update(orderNew);
             await this.context.SaveChangesAsync();

            return await this.context.Orders.Include(o => o.User).Include(o => o.Items).Where(o => o.Id == id).FirstAsync();
                //FindAsync(id);

        }


        public async Task<IQueryable<OrderDetail>> GetOrderDetail(int id)
        {
            
         
            return  this.context.OrderDetails
                .Include(o => o.Product)
                .Where(o => o.OrderId == id);
        }

    }

}
