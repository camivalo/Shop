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

        //public IQueryable GetAllWithUsers()
        //{
        //    return this.context.Products.Include(p => p.User).OrderBy(p => p.Name);
        //}

    }

}

