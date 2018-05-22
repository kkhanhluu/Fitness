using Fitness.Data;
using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Fitness.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null; 

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository(); 
        }
        // GET: Entries
        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();
            double total = entries.Where(e => e.Exclude == false).Sum(e => e.Duration);
            double totalDays = entries.Select(e => e.Date).Distinct().Count();
            double average = total / totalDays;

            ViewBag.Total = total;
            ViewBag.Average = average; 
            return View(entries);
        }

        public ActionResult Add()
        {
            var entry = new Entry()
            {
                Date = DateTime.Today
            };
            setupSelectList();
            return View(entry);
        }

        private void setupSelectList()
        {
            ViewBag.ActivitiesSelectList = new SelectList(Data.Data.Activities, "Id", "Name");
        }

        [HttpPost]
        public ActionResult Add(Entry entry)
        {
            Validate(entry);
            if(ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);
                TempData["Message"] = "Your entry was successfully added!";
                return RedirectToAction("Index"); 
            }
            setupSelectList();
            return View(entry); 
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }
            Entry entry = _entriesRepository.GetEntry((int)id); 
            if(entry == null)
            {
                return HttpNotFound(); 
            }
            setupSelectList();
            return View(entry);
        }

        [HttpPost]
        public ActionResult Edit(Entry entry)
        {
            Validate(entry); 
            if (ModelState.IsValid)
            {
                _entriesRepository.UpdateEntry(entry);
                TempData["Message"] = "Your entry was successfully updated!";
                return RedirectToAction("Index"); 
            }
            setupSelectList();
            return View(entry); 
        }

        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new  HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }
            Entry entry = _entriesRepository.GetEntry((int)id); 
            if(entry == null)
            {
                return HttpNotFound(); 
            }
            return View(entry); 
        }

        [HttpPost]
        public ActionResult Delete(Entry entry)
        {
            _entriesRepository.DeleteEntry(entry);
            TempData["Message"] = "Your entry was successfully deleted!";
            return RedirectToAction("Index"); 
        }
        private void Validate(Entry entry)
        {
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "The Duration field must be greater than 0."); 
            }
        }
    }
}