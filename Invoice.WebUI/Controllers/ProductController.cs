using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Invoice.Model.DataAceess;
using Invoice.Model.DataContext;
using Invoice.Model.Model;

namespace Invoice.WebUI.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private InvoiceDataContext db = new InvoiceDataContext();

        private BaseRepository<ProductModel> productRepository;

        public ProductController()
        {
            productRepository = new BaseRepository<ProductModel>();
        }

        // GET: Product
        public ActionResult Index()
        {
            return View(productRepository.All());
        }

      

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductCode,Product,Description,UnitPrice")] ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                productRepository.Add(productModel);
                return RedirectToAction("Index");
            }
            return View(productModel);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            ProductModel productModel = productRepository.Find(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            return View(productModel);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductCode,Product,Description,UnitPrice")] ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                productRepository.Update(productModel);
                return RedirectToAction("Index");
            }
            return View(productModel);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = db.Products.Find(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            return View(productModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductModel productModel = db.Products.Find(id);
            db.Products.Remove(productModel);
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
