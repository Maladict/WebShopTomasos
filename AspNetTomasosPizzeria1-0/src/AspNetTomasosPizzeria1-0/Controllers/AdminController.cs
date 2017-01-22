using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AspNetTomasosPizzeria1_0.Models;
using AspNetTomasosPizzeria1_0.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Remotion.Linq.Clauses;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetTomasosPizzeria1_0.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private TomasosContext _context;

        public AdminController(TomasosContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewUsers()
        {
            var users = _context.Kund.ToList();

            return View("ViewUsers", users);
        }

        public IActionResult UserDetails(int id)
        {
            var user = _context.Kund.SingleOrDefault(x => x.KundId == id);

            return View("~/Views/Account/ShowAccount.cshtml", user);
        }

        public IActionResult ViewOrders()
        {
            var orders = _context.Bestallning.Include(x => x.Kund).ToList();

            return View(orders);
        }

        public IActionResult ViewFoodItems()
        {
            var foodItems = _context.Matratt.ToList();

            return View(foodItems);
        }

        public IActionResult ViewIngredients()
        {
            var ingredients = _context.Produkt.ToList();

            return View(ingredients);
        }

        public IActionResult CreateFoodItem()

        {
            var viewModel = new CreateFoodItemViewModel()
            {
                IngredientsOptions = GetIngredients(0),
                TypeOptions = GetTypes(0)
            };

            return View(viewModel);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult CreateFoodItem(CreateFoodItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var foodItem = new Matratt()
                {
                    MatrattNamn = viewModel.Name,
                    Beskrivning = viewModel.Description,
                    MatrattTyp = viewModel.Type,
                    Pris = viewModel.Price,
                };

                _context.Matratt.Add(foodItem);
                _context.SaveChanges();

                //Create MatrattProdukt
                foreach (var ingredient in viewModel.IngredientsChosen.ToList())
                {
                    var matrattProdukt = new MatrattProdukt()
                    {
                        MatrattId = _context.Matratt.FirstOrDefault(x => x.MatrattNamn == viewModel.Name).MatrattId,
                        ProduktId = _context.Produkt.FirstOrDefault(x => x.ProduktId == ingredient).ProduktId
                    };

                    _context.MatrattProdukt.Add(matrattProdukt);
                }

                _context.SaveChanges();

                return RedirectToAction("ViewFoodItems");
            }
            else
            {
                return View(viewModel);
            }
        }

        public IActionResult UpdateFoodItem(int id)
        {
            var foodItem = _context.Matratt.SingleOrDefault(x => x.MatrattId == id);

            var viewModel = new UpdateFoodItemViewModel()
            {
                FoodItemId = foodItem.MatrattId,
                Name = foodItem.MatrattNamn,
                Description = foodItem.Beskrivning,
                Price = foodItem.Pris,
                IngredientsOptions = GetIngredients(id),
                TypeOptions = GetTypes(id)
            };

            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFoodItem(UpdateFoodItemViewModel viewModel)
        {
            var itemToUpdate = _context.Matratt.SingleOrDefault(x => x.MatrattId == viewModel.FoodItemId);

            if (ModelState.IsValid)
            {
                var updatedFoodItem = new Matratt()
                {
                    MatrattId = viewModel.FoodItemId,
                    MatrattNamn = viewModel.Name,
                    Beskrivning = viewModel.Description,
                    Pris = viewModel.Price,
                    MatrattTyp = viewModel.Type
                };


                ////Remove old ingredients
                foreach (var oldIngredent in _context.MatrattProdukt.Where(x => x.MatrattId == viewModel.FoodItemId).ToList())
                {
                    _context.Remove(oldIngredent);
                }
                _context.SaveChanges();


                //Add new ingredients
                foreach (var ingredient in viewModel.IngredientsChosen.ToList())
                {
                    var newIngredient = new MatrattProdukt()
                    {
                        MatrattId = _context.Matratt.FirstOrDefault(x => x.MatrattId == viewModel.FoodItemId).MatrattId,
                        ProduktId = _context.Produkt.FirstOrDefault(x => x.ProduktId == ingredient).ProduktId
                    };

                    _context.MatrattProdukt.Add(newIngredient);
                }

                _context.Entry(itemToUpdate).CurrentValues.SetValues(updatedFoodItem);
                _context.SaveChanges();

                return RedirectToAction("ViewFoodItems");
            }
            return View(viewModel);           
        }

        private List<SelectListItem> GetTypes(int id)
        {
            var types = new List<SelectListItem>();

            foreach (var type in _context.MatrattTyp.ToList())
            {
                var option = new SelectListItem() { Text = type.Beskrivning, Value = type.MatrattTyp1.ToString() };

                //Check which option is selected
                if (id != 0 && type.MatrattTyp1 == _context.Matratt.SingleOrDefault(x => x.MatrattId == id).MatrattTyp )
                {
                    option.Selected = true;
                }

                types.Add(option);
            }

            return types;
        }

        private List<SelectListItem> GetIngredients(int id)
        {
            var ingredients = new List<SelectListItem>();

            foreach (var ingredient in _context.Produkt.ToList())
            {
                var option = new SelectListItem() { Text = ingredient.ProduktNamn, Value = ingredient.ProduktId.ToString() };

                //Check if ingredient exists in object
                if (_context.MatrattProdukt.Any(x => x.ProduktId == ingredient.ProduktId && x.MatrattId == id))
                {
                    option.Selected = true;
                }

                ingredients.Add(option);
            }
            return ingredients;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrder(int id, string delivered)
        {
            bool result = delivered == "1";

            _context.Bestallning.SingleOrDefault(x => x.BestallningId == id).Levererad = result;

            _context.SaveChanges();

            return RedirectToAction("ViewOrders");
        }

        public IActionResult CreateIngredient()
        {
            var ingredient = new Produkt();

            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateIngredient(Produkt ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Produkt.Add(ingredient);
                _context.SaveChanges();

                return RedirectToAction("ViewIngredients");
            }

            return View(ingredient);
        }

        public IActionResult UpdateIngredient(int id)
        {
            var ingredientToUpdate = _context.Produkt.SingleOrDefault(x => x.ProduktId == id);

            return View(ingredientToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateIngredient(Produkt updatedIngredient)
        {
            if (ModelState.IsValid)
            {
                var ingredientToUpdate = _context.Produkt.SingleOrDefault(x => x.ProduktId == updatedIngredient.ProduktId);

                _context.Entry(ingredientToUpdate).CurrentValues.SetValues(updatedIngredient);
                _context.SaveChanges();

                return RedirectToAction("ViewIngredients");
            }

            return View(updatedIngredient);
        }
        
        public IActionResult DeleteFoodItem(int id)
        {
            ////Remove old ingredients
            foreach (var oldIngredent in _context.MatrattProdukt.Where(x => x.MatrattId == id).ToList())
            {
                _context.Remove(oldIngredent);
            }
            _context.SaveChanges();

            //Remove Matratt
            var itemToRemove = _context.Matratt.SingleOrDefault(x => x.MatrattId == id);

            _context.Remove(itemToRemove);
            _context.SaveChanges();
            
         
            return RedirectToAction("ViewFoodItems");
        }

        public IActionResult DeleteIngredient(int id)
        {
            //Remove from MatrattProdukt
            foreach (var oldIngredent in _context.MatrattProdukt.Where(x => x.ProduktId == id).ToList())
            {
                _context.Remove(oldIngredent);
            }
            _context.SaveChanges();

            //Remove from Produkt
            var ingredientToDelete = _context.Produkt.SingleOrDefault(x => x.ProduktId == id);

            _context.Remove(ingredientToDelete);
            _context.SaveChanges();

            return RedirectToAction("ViewIngredients");
        }

        public IActionResult DeleteOrder(int id)
        {
            //Delete BestallningMatratt
            foreach (var item in _context.BestallningMatratt.Where(x => x.BestallningId == id).ToList())
            {
                _context.Remove(item);
            }
            _context.SaveChanges();

            //Delete Bestallning
            var orderToDelete = _context.Bestallning.FirstOrDefault(x => x.BestallningId == id);

            _context.Remove(orderToDelete);
            _context.SaveChanges();

            return RedirectToAction("ViewOrders");
        }
    }
}

