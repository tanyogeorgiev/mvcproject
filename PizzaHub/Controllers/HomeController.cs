using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PizzaHub.Models;

namespace PizzaHub.Controllers
{
    public class HomeController : Controller
    {
        private PizzaDbContext db = new PizzaDbContext();
        //GET: Pizzas
        public ActionResult Index()
        {
            var pizzas = db.Pizzas.Include(p => p.AspNetUser).OrderByDescending(p => p.Date).Take(5);
            return View(pizzas.ToList());
        }

    }
}