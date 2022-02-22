using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RShopping.WEB.Models;
using RShopping.WEB.Services.IServices;
using RShopping.WEB.Utils;

namespace RShopping.WEB.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.FindAll();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Create(model);
                if (response != null)
                    return RedirectToAction("Index");
            }

            return View(model);

        }

        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.FindById(id);
            if (product != null) return View(product);

            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Update(model);
                if (response != null)
                    return RedirectToAction("Index");
            }

            return View(model);

        }

        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.FindById(id);
            if (product != null) return View(product);

            return NotFound();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Delete(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Delete(model.Id);
                if (response)
                    return RedirectToAction("Index");
            }

            return View();
        }
    }
}
