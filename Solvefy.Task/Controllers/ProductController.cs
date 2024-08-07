using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Solvefy.Task.Data;
using Solvefy.Task.Models.Dtos;
using Solvefy.Task.Models.Entities;

namespace Solvefy.Task.Controllers
{
    public class ProductController : Controller
    {
        private readonly InventoryDbContext _dbContext;
        public ProductController(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, string name = "")
        {

            string cookieValue = Request.Cookies["role"]!;

            int pageSize = 4;
            var filterProducts = _dbContext.Products.AsEnumerable();

            if (!string.IsNullOrEmpty(name))
            {
                filterProducts = filterProducts
                    .Where(p => p.ProductName.ToLower().Contains(name.ToLower()));
            }

            var totalProducts = filterProducts.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var products = filterProducts
                .OrderBy(p => p.ProductName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.FilterName = name;
            ViewBag.Role = cookieValue;

            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct() {
            var cookieValue = Request.Cookies["role"]!;
            if(cookieValue != null && cookieValue == "Admin")
            {
                return View();

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var cookieValue = Request.Cookies["role"]!;
                if (cookieValue != null && cookieValue == "Admin")
                {
                    var product = _dbContext.Products.SingleOrDefault(p => p.Id == id);
                    if (product != null)
                    {
                        _dbContext.Products.Remove(product);
                        _dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDto product)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Any())
                {
                    var file = files[0];
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine("wwwroot", "uploads", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var newProduct = new Product
                    {
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Description = product.Description,
                        Quantity = product.Quantity,
                        ImageUrl = $"/uploads/{fileName}" 
                    };

                    _dbContext.Products.Add(newProduct);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Image", "Please upload an image.");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var cookieValue = Request.Cookies["role"]!;
            if (cookieValue != null && cookieValue == "Admin")
            {
                var product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var editProductDto = new EditProductDto
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Description = product.Description,
                    Quantity = product.Quantity,
                };

                return View(editProductDto);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult EditProduct(EditProductDto productDto)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.Id == productDto.Id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            product.ProductName = productDto.ProductName;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.Quantity = productDto.Quantity;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
