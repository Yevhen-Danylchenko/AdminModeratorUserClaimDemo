using AdminModeratorUserClaimDemo.Data;
using AdminModeratorUserClaimDemo.Models;
using AdminModeratorUserClaimDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminModeratorUserClaimDemo.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Index()
        {
            var users = _context.Users
                .Include(u => u.Products) // завантажуємо продукти
                .ToList()                 // виконуємо SQL-запит
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Name = u.Name,
                    ProductNames = u.Products.Any()
                        ? string.Join(", ", u.Products.Select(p => p.Name))
                        : "No Products"
                })
                .ToList();

            return View(users);
        }


        // GET: Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        // GET: Admin/Register
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Admin/Register
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Register(User model, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = model.Name,
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        // GET: Admin/EditUser/5
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
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

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        // POST: Admin/DeleteUser/5
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // --- Products CRUD залишаються без змін ---

        // GET: Admin/Products
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Products()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // GET: Admin/CreateProduct
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult CreateProduct()
        {
            return View();
        }

        // POST: Admin/CreateProduct
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
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

        // GET: Admin/UpdateProduct/5
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult UpdateProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Admin/UpdateProduct/5
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
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

        // POST: Admin/DeleteProduct/5
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
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



