using AdminModeratorUserClaimDemo.Data;
using AdminModeratorUserClaimDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminModeratorUserClaimDemo.Controllers
{
    public class ModeratorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ModeratorController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Moderator
        [Authorize(Roles = "Moderator")]
        public IActionResult Index()
        {
            var users = _context.Users.
                Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.Name,
                    u.IsAdmin,
                    ProductName = u.Products != null ? u.Products.Name : "No Product"
                })
                .ToList();
            return View(users);
        }

        // GET: Moderator/Register
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult Register()
        {
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Moderator/Register
        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = model.Name,
                    IsAdmin = model.IsAdmin,
                    ProductId = model.ProductId
                };

                var result = await _userManager.CreateAsync(user, model.PasswordHash);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View(model);
        }

        // GET: Moderator/EditUser/5
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult EditUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                ProductId = user.ProductId
            };
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name", user.ProductId);
            return View(model);
        }

        // POST: Moderator/EditUser/5
        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> EditUser(string id, User model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Name = model.Name;
                user.IsAdmin = model.IsAdmin;
                user.ProductId = model.ProductId;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name", model.ProductId);
            return View(model);
        }

        // POST: Moderator/DeleteUser/5
        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle deletion errors if necessary
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Moderator/Products
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult Products()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // GET: Moderator/CreateProduct
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult CreateProduct()
        {
            return View();
        }

        // POST: Moderator/CreateProduct
        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult CreateProduct(Product model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.UtcNow;
                _context.Products.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Products));
            }
            return View(model);
        }

        // GET: Moderator/UpdateProduct/5
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult UpdateProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Moderator/UpdateProduct/5
        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult UpdateProduct(int id, Product model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var product = _context.Products.FirstOrDefault(x => x.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Status = model.Status;
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Products));
            }
            return View(model);
        }

        // POST: Moderator/DeleteProduct/5
        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Products));
        }
    }
}
