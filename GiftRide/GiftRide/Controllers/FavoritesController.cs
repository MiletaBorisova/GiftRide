using GiftRide.Core.Contracts;
using GiftRide.Infrastructure.Data.Entities;
using GiftRide.Models.Favorites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace GiftRide.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {

        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(IFavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        private string GetUserId() => _userManager.GetUserId(User);


        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var products = await _favoriteService.GetFavoritesAsync(userId);
            var favoriteViewModels = products.Select(p => new FavoriteVM
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Picture = p.Picture,
                Price = p.Price
            }).ToList();

            
            return View(favoriteViewModels);
        }



        public async Task<IActionResult> Add(int productId)
        {
            var userId = GetUserId();
            await _favoriteService.AddFavoriteAsync(userId, productId);
            return RedirectToAction("Index", "Favorites");

            
        }

        [Authorize]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetUserId();
            await _favoriteService.RemoveFavoriteAsync(userId, productId);
            return RedirectToAction("Index");
        }
        
        
    }
}