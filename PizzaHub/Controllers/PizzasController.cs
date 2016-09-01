using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PizzaHub.Models;
using PizzaHub.Extensions;

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
          ViewBag.comments = db.Comments.Where(c=> c.PostID==id);
            ViewBag.count = db.Comments.Where(c => c.PostID == id).Count();
            return View(pizza);
        }

        // GET: Pizzas/Create
        [Authorize]
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
                // pizza.Date = DateTime.Now;
                var data = Request.Form["Date"];
                if (data == "")
                {
                    pizza.Date = DateTime.Now;
                }
                db.Pizzas.Add(pizza);
                db.SaveChanges();
                this.AddNotification("Успешно създадохте пица.", NotificationType.INFO);
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
            ViewBag.check = db.AspNetUsers.Single(it => it.UserName == User.Identity.Name);
            
            if (User.IsInRole("Administrators") || (pizza.Author == ViewBag.check.Id))
            {
                return View(pizza);
            }
            /* ViewBag.Author = new SelectList(db.AspNetUsers, "Id", "Email", pizza.Author); */
            else
            {
                this.AddNotification("Нямате право да редактирате тази пица!", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
        }

        // POST: Pizzas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]

        //Махам Author ot бинда, защото го взимам автоматично от Identity.Name
        public ActionResult Edit([Bind(Include = "ID,Name,Text")] Pizza pizza)
        { //Взимам данни на логнатият потребител
            ViewBag.Author = db.AspNetUsers.Single(it => it.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                //Вкарвам в таблицата ID на потребителя
                pizza.Author = ViewBag.Author.Id;
                pizza.Date = DateTime.Now;
                    
                //Винаги да взима текуща дата/час
               //  pizza.Date = DateTime.Now; 
                db.Entry(pizza).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Успешно редактирахте пица.", NotificationType.INFO);
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
        [Authorize(Roles = "Administrators")]
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

        // GET: Pizzas
        [Authorize]
        public ActionResult UserPizza()
        {
            ViewBag.Author = db.AspNetUsers.Single(it => it.UserName == User.Identity.Name);
            string curr = ViewBag.Author.Id;
            var pizzas = db.Pizzas.Include(p => p.AspNetUser).Where(e => e.Author == curr);
            return View(pizzas.ToList());
        }
        public ActionResult CreateComment()
        {
            return RedirectToAction("Index");
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        //Махам Author ot бинда, защото го взимам автоматично от Identity.Name
        public ActionResult CreateComment([Bind(Include = "Text,AuthorName")] Comment comment)
        {
            //Взимам данни на логнатият потребител
            ViewBag.Author = db.AspNetUsers.Single(it => it.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                //Вкарвам в таблицата ID на потребителя
                comment.AuthorID = ViewBag.Author.Id;
                //Винаги да взима текуща дата/час
                comment.Date = DateTime.Now;
                comment.PostID = 2;
                db.Comments.Add(comment);
                db.SaveChanges();
                this.AddNotification("Успешно написахте коментар", NotificationType.INFO);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

    }
}
