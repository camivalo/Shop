using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Controllers.API
{
    using Data;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shop.Common.Models;
    using Shop.Web.Data.Entities;
    using Shop.Web.Data.Repositories;
    using Shop.Web.Helpers;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserHelper userHelper;

        public OrdersController(IOrderRepository orderRepository, IUserHelper userHelper)
        {
            this.orderRepository = orderRepository;
            this.userHelper = userHelper;
        }

       

        [HttpPost]
        [Route("GetOrdersByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetOrdersByEmail([FromBody] RecoverPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "User don't exists."
                });
            }
            //var l = await this.orderRepository.GetOrdersAsync(request.Email);
            return this.Ok(await this.orderRepository.GetOrdersAsync(request.Email));
        }

        [HttpPut]
        [Route("PutOrder")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutOrder([FromBody] List<Data.Entities.OrderDetailTemp> model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }


            var addOrder = await this.orderRepository.ConfirmOrderFromAPPAsync(model);
            return Ok(addOrder);
        }

        [HttpPut]
        [Route("PutOrderInToOrderTemp")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutOrderInToOrderTemp([FromBody] List<Data.Entities.OrderDetailTemp> list)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }


            var addOrder = await this.orderRepository.AddItemToOrderFromAppforOrderTempAsync(list);
            return Ok(addOrder);
        }

        [HttpPost]
        [Route("GetOrdersDetailTempByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetOrdersDetailTempByEmail([FromBody] RecoverPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "User don't exists."
                });
            }
            //var l = await this.orderRepository.GetOrdersAsync(request.Email);
            return this.Ok(await this.orderRepository.GetDetailTempsAsync(request.Email));
        }

        //[HttpGet]
        //public IActionResult GetOrdersByOrderTemp()
        //{
        //    return this.Ok(this.productRepository.GetAllWithUsers());
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            await this.orderRepository.DeleteDetailTempAsync(id);
            //if (itemOrder == null)
            //{
            //    return this.NotFound();
            //}

            //await this.orderRepository.DeleteAsync(itemOrder);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder([FromRoute] int id, [FromBody]  Common.Models.Order order)
        {

           

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            var oldOrder= await this.orderRepository.GetByIdAsync(id);
            if (oldOrder == null)
            {
                return this.BadRequest("Order Id don't exists.");
            }

            var user = new Data.Entities.User()
            {
                FirstName = order.User.FirstName,
                Address = order.User.Address,
                CityId = order.User.CityId,
                Email = order.User.Email,
                IsAdmin = order.User.IsAdmin,
                LastName = order.User.LastName,
                PhoneNumber = order.User.PhoneNumber,
                UserName = order.User.UserName
                


            };

            //TODO: Upload images
            oldOrder.OrderDate = order.OrderDate;
            oldOrder.DeliveryDate = order.DeliveryDate;
            oldOrder.User = user;
            //oldProduct.LastSale = product.LastSale;
            //oldProduct.Name = product.Name;
            //oldProduct.Price = product.Price;
            //oldProduct.Stock = product.Stock;

            //oldOrder.OrderDate = order.OrderDate;
            //oldOrder.DeliveryDate = order.DeliveryDate;
            //oldOrder.User.Address = order.User.Address;
            //oldOrder.User.CityId = order.User.CityId;
            //oldOrder.User.Email = order.User.Email;
            //oldOrder.User.FirstName = order.User.FirstName;
            //oldOrder.User.IsAdmin = order.User.IsAdmin;
            //oldOrder.User.LastName = order.User.LastName;
            //oldOrder.User.PhoneNumber = order.User.PhoneNumber;
            //oldOrder.User.UserName = order.User.UserName;


            var updatedOrder = await this.orderRepository.ModifyOrderDeliveryDateAsync(id, order);
            return Ok(updatedOrder);
        }

        //public IQueryable GetAllWithUsers()
        //{
        //    return this.context.Products.Include(p => p.User).OrderBy(p => p.Name);
        //}



        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            
            //if (itemOrder == null)
            //{
            //    return this.NotFound();
            //}

            //await this.orderRepository.DeleteAsync(itemOrder);
            return Ok(await this.orderRepository.GetOrderDetail(id));
        }

    }

}

