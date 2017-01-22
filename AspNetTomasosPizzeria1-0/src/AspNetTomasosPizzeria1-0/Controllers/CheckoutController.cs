using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetTomasosPizzeria1_0.Models;
using AspNetTomasosPizzeria1_0.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AspNetTomasosPizzeria1_0.Controllers
{
    public class CheckoutController : Controller
    {
        private TomasosContext _context;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public CheckoutController(TomasosContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult AddToCart(int id, int quantity)
        {
            var productToAdd = _context.Matratt
                .SingleOrDefault(x => x.MatrattId.Equals(id));

            List<CartItem> cart;

            //Create new CartItem
            var cartItem = new CartItem()
            {
                Matratt = productToAdd,
                Quantity = quantity
            };

            //If no previous cart exists, create new cart and add the cart item.
            if (HttpContext.Session.GetString("Cart") == null)
            {
                cart = new List<CartItem>()
                {
                    cartItem
                };
            }
            else
            {
                //Get existing cart from session
                cart = GetCart();


                //Search for duplicates and add quantities if found
                if (cart.Any(x => x.Matratt.MatrattId == productToAdd.MatrattId))
                {
                    cart.FirstOrDefault(x => x.Matratt.MatrattId == productToAdd.MatrattId).Quantity +=
                        cartItem.Quantity;
                }
                else
                {
                    cart.Add(cartItem);
                }                
            }
              
            SetCart(cart);

            

            return RedirectToAction("ShowCart", cart);
        }

        public IActionResult DeleteFromCart(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(x => x.CartItemId.Equals(id));

            cart.Remove(cartItem);

            SetCart(cart);



            return RedirectToAction("ShowCart");
        }

        public IActionResult ShowCart()
        {
            List<CartItem> cart;

            if (HttpContext.Session.GetString("Cart") == null)
            {
                cart = new List<CartItem>();

                return View(cart);
            }
            else
            {
                cart = GetCart();
                return View(cart);
            }           
        }

        public IActionResult AdjustQuantity(int id, int choice)
        {
            //Check if cart is empty
            if (!GetCart().Any())
            {
                return RedirectToAction("ShowCart");
            }

            //Find correct item in session
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(x => x.CartItemId.Equals(id));

            //Execute action

            if (choice == 0 && cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else if (choice == 1 && cartItem.Quantity > 0)
            {
                cartItem.Quantity++;
            }

            //Save to session
            SetCart(cart);

            return RedirectToAction("ShowCart");
        }
        
        public async Task<IActionResult> Checkout()
        {
            //Check if user is signed in
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            //Check if cart is empty
            if (!GetCart().Any())
            {
                return RedirectToAction("ShowCart");
            }

            var cart = GetCart();

            double orderTotal = 0;
            double originalPrice = 0;

                //Get user
                var user = await GetCurrentUserAsync();

            //Create new order
            var order = new Bestallning()
                {
                    KundId = user.KundId,
                    Levererad = false, 
                    Totalbelopp = (int) orderTotal
                };

                _context.Bestallning.Add(order);

                //Create new BeställningMaträtt for each item
                foreach (var item in cart)
                {
                    var bestallningMatratt = new BestallningMatratt()
                    {
                        Antal = item.Quantity,
                        MatrattId = item.Matratt.MatrattId,
                        BestallningId = order.BestallningId,                       
                    };

                    //Add beställningmaträtt to database

                    _context.BestallningMatratt.Add(bestallningMatratt);

                    orderTotal += item.Quantity*item.Matratt.Pris;
                    originalPrice = orderTotal;
                }

            var discountPoints = 0;
            var discountMultipleOrders = 0;

            if (User.IsInRole("PremiumUser"))
            {
                //Calculate points
                var totalQuantity = cart.Sum(x => x.Quantity);
                int pointsToAdd = totalQuantity * 10;

                var kund = _context.Kund.FirstOrDefault(x => x.KundId == user.KundId);
                
                kund.Points += pointsToAdd;


                //100 points off
                if (kund.Points >= 100)
                {
                    discountPoints = cart.Min(x => x.Matratt.Pris);

                    orderTotal = orderTotal - discountPoints;

                    //Set points to 0
                    kund.Points = 0;
                }

                //More than three items
                if (totalQuantity >=3)
                {
                    discountMultipleOrders = (int) (orderTotal * 0.2);
                    orderTotal = orderTotal - discountMultipleOrders;
                }
            }        
                //Set total of order
                order.Totalbelopp = (int) orderTotal;

                //Remove cart session
                HttpContext.Session.Remove("Cart");

                //Add order to database
                _context.Bestallning.Add(order);
                _context.SaveChanges();

            var viewModel = new ConfirmOrderViewModel()
            {
                Order = order,
                DiscountMultipleOrders = discountMultipleOrders,
                DiscountPoints = discountPoints,
                OriginalPrice = originalPrice

            };

                return View("ConfirmOrder", viewModel);        
        }

        public List<CartItem> GetCart()
        {
            List<CartItem> cart;

            if (HttpContext.Session.GetString("Cart") != null)
            {
                var temp = HttpContext.Session.GetString("Cart");
                cart = JsonConvert.DeserializeObject<List<CartItem>>(temp);
            }
            else
            {
                cart = new List<CartItem>();
            }

            return (cart);
        }

        public void SetCart(List<CartItem> cart)
        {
            var temp = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", temp);
        }
    }
}
