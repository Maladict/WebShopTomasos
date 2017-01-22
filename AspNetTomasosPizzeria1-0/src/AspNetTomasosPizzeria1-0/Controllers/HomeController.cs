using System.Linq;
using AspNetTomasosPizzeria1_0.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetTomasosPizzeria1_0.Controllers
{
    public class HomeController : Controller
    {
        private TomasosContext _context;

        public HomeController(TomasosContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Matratt.ToList();

            return View(products);
        }

        public IActionResult FilterCategories(int id)
        {
            var products = _context.Matratt
                .Where(x => x.MatrattTyp.Equals(id)).ToList();

            return View("Index", products);
        }

        public IActionResult FooterPartial(string answer)
        {
            bool likesPizza = answer != "0";

            return PartialView("_LikesPizzaPartial", likesPizza);
        }
    }
}
