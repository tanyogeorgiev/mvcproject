using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PizzaHub.Models;

namespace PizzaHub.Controllers
{
    public class PizzasController : Controller
    {
        private PizzaDbContext db = new PizzaDbContext();

        // GET: Pizzas
        public ActionResult Index()
        {
            var pizzas = db.Pizzas.Include(p => p.AspNetUser);
            return View(pizzas.ToList());
        }

        // GET: Pizzas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizza pizza = db.Pizzas.Find(id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
            return View(pizza);
        }

        // GET: Pizzas/Create
        public ActionResult Create()
        {
          /*  ViewBag.Author = new SelectList(db.AspNetUsers, "Id", "Email"); Това го махам, защото при създаване нямам нужда от списък*/
            return View();
        }

        // POST: Pizzas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Махам Author ot бинда, защото го взимам автоматично от Identity.Name
        public ActionResult Create([Bind(Include = "ID,Name,Text,Date")] Pizza pizza)
        {
            //Взимам данни на логнатият потребител
            ViewBag.Author = db.AspNetUsers.Single(it => it.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                //Вкарвам в таблицата ID на потребителя
                pizza.Author = ViewBag.Author.Id;
                //Винаги да взима текуща дата/час
                pizza.Date = DateTime.Now;
                db.Pizzas.Add(pizza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Author = new SelectList(db.AspNetUsers, "Id", "Email", pizza.Author);
            return View(pizza);
        }

        // GET: Pizzas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizza pizza = db.Pizzas.Find(id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
           /* ViewBag.Author = new SelectList(db.AspNetUsers, "Id", "Email", pizza.Author); */
            return View(pizza);
        }

        // POST: Pizzas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Махам Author ot бинда, защото го взимам автоматично от Identity.Name
        public ActionResult Edit([Bind(Include = "ID,Name,Text,Date")] Pizza pizza)
        { //Взимам данни на логнатият потребител
            ViewBag.Author = db.AspNetUsers.Single(it => it.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                //Вкарвам в таблицата ID на потребителя
                pizza.Author = ViewBag.Author.Id;
                //Винаги да взима текуща дата/час
                pizza.Date = DateTime.Now;
                db.Entry(pizza).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Author = new SelectList(db.AspNetUsers, "Id", "Email", pizza.Author);
            return View(pizza);
        }

        // GET: Pizzas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizza pizza = db.Pizzas.Find(id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
            return View(pizza);
        }

        // POST: Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pizza pizza = db.Pizzas.Find(id);
            db.Pizzas.Remove(pizza);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
