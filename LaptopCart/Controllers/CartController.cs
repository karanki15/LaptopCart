using LaptopCart.Data;
using LaptopCart.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LaptopCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _context.CartItems.Where(x => x.UserId == userId)
                .Include(x => x.Product).ToList();
            return View(cartItems);
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cartFromDb = _context.CartItems.FirstOrDefault(u => u.Id == cartId);
            if (cartFromDb == null) return NotFound();

            cartFromDb.Quantity += 1;
            _context.CartItems.Update(cartFromDb);
            await _context.SaveChangesAsync();
            TempData["success"] = "Cart Item Incremented Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cartFromDb = _context.CartItems.FirstOrDefault(x => x.Id == cartId);
            if (cartFromDb == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (cartFromDb.Quantity <= 1) {
                _context.CartItems.Remove(cartFromDb);
                await _context.SaveChangesAsync();

                var Count = _context.CartItems.Count(x => x.UserId == userId);
                HttpContext.Session.SetInt32(SD.SessionCart, Count);
            
            }
            else
            {
                cartFromDb.Quantity -= 1;
                _context.CartItems.Update(cartFromDb);
                await _context.SaveChangesAsync();
            }
            TempData["success"] = "Cart Item Decremented Successfully";
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartFromDb = _context.CartItems.FirstOrDefault(x => x.Id == cartId);

            if (cartFromDb == null) return NotFound();

            _context.CartItems.Remove(cartFromDb);
            await _context.SaveChangesAsync();

            var Count = _context.CartItems.Count(x => x.UserId == userId);
            HttpContext.Session.SetInt32(SD.SessionCart, Count);

            TempData["success"] = "Cart Item Removed Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
