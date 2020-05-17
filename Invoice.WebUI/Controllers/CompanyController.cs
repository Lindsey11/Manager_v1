using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invoice.Model.DataAceess;
using Invoice.Model.DataContext;
using Invoice.Model.Model;

namespace Invoice.WebUI.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private BaseRepository<CompanyModel> companyRepository;

        public CompanyController()
        {
            companyRepository = new BaseRepository<CompanyModel>();
        }

        // GET: Company
        [HttpGet]
        public ActionResult Index()
        {
            int count = companyRepository.All().Count();
            if (count > 0)
            {
                var company = companyRepository.All().FirstOrDefault();
                return View(company);
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult Index(CompanyModel company)
        {
            int count = companyRepository.All().Count();
            if (count > 0)
            {
                CompanyUpdate(company);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                companyRepository.Add(company);
                return RedirectToAction("Index", "Home");
            }
        }

        private void CompanyUpdate(CompanyModel company)
        {
            var model = companyRepository.Find(company.CompanyId);
            model.CompanyName = company.CompanyName;
            model.CompanyCode = company.CompanyCode;
            model.Phone = company.Phone;
            model.Web = company.Web;
            model.Email = company.Email;
            model.Address = company.Address;
            companyRepository.Update(model);
        }
    }
}