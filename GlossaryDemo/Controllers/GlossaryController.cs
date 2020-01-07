using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlossaryDemo.Models;
using System.Data.Entity;


namespace GlossaryDemo.Controllers
{
    public class GlossaryController : Controller
    {
        public ActionResult Index(string sortOrder)
        {
            using (var context = new GlossaryDBEntities())
            {
                ViewBag.SortTermParm = String.IsNullOrEmpty(sortOrder) ? "term_desc" : "";
                ViewBag.DefinitionParm = sortOrder == "Definition" ? "definition_desc" : "Definition";

                var glossary = from g in context.GlossaryMains select g;

                switch (sortOrder)
                {
                    case "term_desc":
                        glossary = glossary.OrderByDescending(s => s.Term);
                        break;
                    case "Definition":
                        glossary = glossary.OrderBy(s => s.Definition);
                        break;
                    case "definition_desc":
                        glossary = glossary.OrderByDescending(s => s.Definition);
                        break;
                    default:
                        glossary = glossary.OrderBy(s => s.Term);
                        break;
                }
                return View(glossary.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(GlossaryMain glossary)
        {
            try
            {
                using (GlossaryDBEntities gEntity = new GlossaryDBEntities())
                {
                    gEntity.GlossaryMains.Add(glossary);
                    gEntity.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            using (GlossaryDBEntities gEntity = new GlossaryDBEntities())
            {
                return View(gEntity.GlossaryMains.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, GlossaryMain glossary)
        {
            try
            {
                using (GlossaryDBEntities gEntity = new GlossaryDBEntities())
                {
                    gEntity.Entry(glossary).State = EntityState.Modified;
                    gEntity.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            using (GlossaryDBEntities gEntity = new GlossaryDBEntities())
            {
                return View(gEntity.GlossaryMains.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (GlossaryDBEntities gEntity = new GlossaryDBEntities())
                {
                    GlossaryMain gMain = gEntity.GlossaryMains.Where(x => x.Id == id).FirstOrDefault();
                    gEntity.GlossaryMains.Remove(gMain);
                    gEntity.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

