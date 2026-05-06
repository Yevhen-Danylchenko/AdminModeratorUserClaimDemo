using AdminModeratorUserClaimDemo.Data;
using AdminModeratorUserClaimDemo.Models;
using AdminModeratorUserClaimDemo.ViewModels;
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
        private readonly SignInManager<User> _signInManager;

        public SuperAdminController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: SuperAdmin
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            var users = _context.Users
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Name = u.Name,
                    IsAdmin = u.IsAdmin,
                    ProductNames = string.Join(", ", u.Products.Select(p => p.Name))
                })
                .ToList();

            return View(users);
        }


        //// GET: SuperAdmin/Login
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //// POST: SuperAdmin/Login
        //[HttpPost]
        //public async Task<IActionResult> Login(string username, string password)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //    return View();
        //}

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "SuperAdmin");

            ModelState.AddModelError("", "Невірний логін або пароль");
            return View(model);
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
        public async Task<IActionResult> Register(User model, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = model.Name,
                    IsAdmin = model.IsAdmin
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

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View(model);
        }

        // GET: SuperAdmin/EditUser/5
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name", user.Products?.FirstOrDefault()?.Id);
            return View(user);
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

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
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
            return RedirectToAction(nameof(Index));
        }
    }
}


