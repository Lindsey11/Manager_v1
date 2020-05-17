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
    public class CustomerController : Controller
    {
       
        private BaseRepository<CustomerModel> customerRepository = new BaseRepository<CustomerModel>();

        // GET: Customer
        public ActionResult Index()
        {
            return View(customerRepository.All());
        }

      

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerCode,Name,Web,Email,Contact,Addess")] CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
               customerRepository.Add(customerModel);
                return RedirectToAction("Index");
            }

            return View(customerModel);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerModel customerModel = customerRepository.Find(id);
            if (customerModel == null)
            {
                return HttpNotFound();
            }
            return View(customerModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerCode,Name,Web,Email,Contact,Addess")] CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                customerRepository.Update(customerModel);
                return RedirectToAction("Index");
            }
            return View(customerModel);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerModel customerModel =customerRepository.Find(id);
            if (customerModel == null)
            {
                return HttpNotFound();
            }
            return View(customerModel);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customerRepository.Remove(id);
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
