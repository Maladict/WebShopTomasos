using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetTomasosPizzeria1_0.Models;
using AspNetTomasosPizzeria1_0.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspNetTomasosPizzeria1_0.Components
{
    [ViewComponent(Name="MenuCart")]
    public class MenuCartViewComponent: ViewComponent
    {
        private List<CartItem> _cart;
        private IHttpContextAccessor _iHttpContextAccessor;
        private ISession _iSession => _iHttpContextAccessor.HttpContext.Session;
        private TomasosContext _context;
        private UserManager<AppUser> _userManager;
        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public MenuCartViewComponent(IHttpContextAccessor httpContextAccessor, TomasosContext context, UserManager<AppUser> userManager )
        {
            _iHttpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;


            //Get cart from session
            if (_iSession.GetString("Cart") != null)
            {
                var serialized = _iSession.GetString("Cart");

                _cart = JsonConvert.DeserializeObject<List<CartItem>>(serialized);
            }
            else
            {
                _cart = new List<CartItem>();
            }
        }

        public async Task<Kund> GetUser()
        {

            var user = await GetCurrentUserAsync();
            var userId = user?.KundId;

            var kund = _context.Kund.SingleOrDefault(x => x.KundId == userId);


            return kund;
        }

        public IViewComponentResult Invoke()
        {
            return View(new MenuBarViewModel() {Cart = _cart, User = GetUser().Result});
        }
    }
}
