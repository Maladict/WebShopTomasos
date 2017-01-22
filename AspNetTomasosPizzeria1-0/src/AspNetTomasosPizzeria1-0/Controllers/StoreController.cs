using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetTomasosPizzeria1_0.Models;
using AspNetTomasosPizzeria1_0.ViewModels;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetTomasosPizzeria1_0.Controllers
{
    public class StoreController : Controller
    {
        private TomasosContext _context;

        public StoreController(TomasosContext context)
        {
            _context = context;
        }
        public IActionResult MatrattDetails(int id)
        {
            
            //Solution with LINQ
            //var products =
            //(from ord in _context.Produkt
            //from prod in _context.Matratt
            //join det in _context.MatrattProdukt
            //    on new { ord.ProduktId, prod.MatrattId } equals new { det.ProduktId, det.MatrattId }
            //    into details
            //from det in details
            //select new { ord.ProduktNamn, prod.MatrattId})
            //.Where(x => x.MatrattId == id).Select(y => y.ProduktNamn);

            //var matratt = _context.Matratt.FirstOrDefault(x => x.MatrattId == id);

            //var viewModel = new MatrattProduktViewModel()
            //{
            //    Matratt = matratt,
            //    Produkter = products
            //};

            //Eager loading of matratt to include relationships
            var matratt = _context.Matratt.Include(x => x.MatrattProdukt)
                .ThenInclude(y => y.Produkt).FirstOrDefault(z => z.MatrattId.Equals(id));

            return View(matratt);
        }
    }
}
