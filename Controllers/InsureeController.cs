using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                insuree.Quote = GetQuote(insuree);
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
        

        public decimal GetQuote(Insuree insuree)
        {
            // write logic for this function dammit
            // do we need a connection string to connect to database to get access to models?
            decimal baseQuote = 50;

            // 18 or under, add 100 per month
            if (insuree.DateOfBirth.Year >= 2004)
            {
                baseQuote += 100;
            }

            // age 19-25, add 50
            if (insuree.DateOfBirth.Year <= 2003 || insuree.DateOfBirth.Year >= 1997)
            {
                baseQuote += 50;
            }

            // age 26 or older, add 25
            if (insuree.DateOfBirth.Year <= 1996)
            {
                baseQuote += 25;
            }

            // car year before 2000, add 25
            if (insuree.CarYear < 2000)
            {
                baseQuote += 25;
            }

            // car newer than 2015, add 25
            if (insuree.CarYear > 2015)
            {
                baseQuote += 25;
            }

            // if the make is a porsche, add 25
            if (insuree.CarMake == "Porsche")
            {
                baseQuote += 25;
            }

            // if porsche 911 carrera, add 50
            if (insuree.CarMake == "Porsche" && insuree.CarModel == "911 Carrera")
            {
                baseQuote += 50;
            }

            // add 10 per speeding ticket 
            if (insuree.SpeedingTickets > 0)
            {
                baseQuote += 10;
            }

            // add 25% if DUI is true
            if (insuree.DUI)
            {
                baseQuote = baseQuote + (baseQuote * (decimal).25);
            }

            // add 50% if full coverage
            if (insuree.CoverageType)
            {
                baseQuote = baseQuote + (baseQuote * (decimal).5); 
            }
            // do I need this return statement?
            return baseQuote;

            
        }
    }
}
