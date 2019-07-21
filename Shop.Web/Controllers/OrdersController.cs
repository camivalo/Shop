namespace Shop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shop.Web.Data.Entities;
    using Shop.Web.Data.Repositories;
    using Shop.Web.Helpers;
    using Shop.Web.Models;

    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await orderRepository.GetOrdersAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await this.orderRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return this.View(model);
        }

        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Products = this.productRepository.GetComboProducts()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return this.RedirectToAction("Create");
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.orderRepository.DeleteDetailTempAsync(id.Value);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> DeleteItemOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //await this.orderRepository.DeleteDetailTempAsync(id.Value);
            await this.orderRepository.DeleteOrderAsync(id.Value);
            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await this.orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
            if (response)
            {
                return this.RedirectToAction("Index");
            }

            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Deliver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await this.orderRepository.GetOrdersAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            var model = new DeliverViewModel
            {
                Id = order.Id,
                DeliveryDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(DeliverViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.orderRepository.DeliverOrder(model);
                return this.RedirectToAction("Index");
            }

            return this.View();
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var orderDetail = await this.orderRepository.GetOrdersDetailAsync(id.Value);

            var model = new OrderDetail();

            List<OrderDetail> l = new List<OrderDetail>();

            foreach (OrderDetail o in orderDetail)
            {

                model = new OrderDetail
                {
                    Product = o.Product,
                    Quantity = o.Quantity,


                };
                l.Add(model);
            }

            return View(l);
        }


    }

}
