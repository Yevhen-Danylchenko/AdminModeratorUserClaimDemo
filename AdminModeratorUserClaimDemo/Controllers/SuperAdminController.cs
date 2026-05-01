using AdminModeratorUserClaimDemo.Data;
using AdminModeratorUserClaimDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminModeratorUserClaimDemo.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public SuperAdminController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SuperAdmin
        [Authorize(Roles = "SuperAdmin")]
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

        // GET: SuperAdmin/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: SuperAdmin/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                // Sign in the user
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Role", "SuperAdmin"));
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        // GET: SuperAdmin/Register
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Register()
        {
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: SuperAdmin/Register
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
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

        // GET: SuperAdmin/EditUser/5
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
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

        // POST: SuperAdmin/EditUser/5
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
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

        // POST: SuperAdmin/DeleteUser/5
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
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
    }
}

