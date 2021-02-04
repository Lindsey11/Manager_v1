using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Invoice.Model.DataAceess;
using Invoice.Model.DataContext;
using Invoice.Model.Model;
using Invoice.WebUI.CustomAttritube;
using Invoice.WebUI.Helper;
using Invoice.WebUI.ViewModel;
using Microsoft.Reporting.WebForms;
using Postal;


namespace Invoice.WebUI.Controllers
{
    //[CookieEnableChecker]
    [Authorize]
    public class InvoiceController : Controller
    {
        // GET: Invoice
        private InvoiceDataContext db;
        private BaseRepository<InvoiceModel> invoiceRepository;
        private InvoiceService _invoiceService;
        public InvoiceController()
        {
            _invoiceService = new InvoiceService();
            db = new InvoiceDataContext();
            invoiceRepository = new BaseRepository<InvoiceModel>();
        }
        public ActionResult Index()
        {
            return View(invoiceRepository.All());
        }
        public ActionResult Add(ProductList model)
        {
            var products = _invoiceService.Add(model, HttpContext);
            //   = _invoiceService.ProductList(HttpContext);
            // products.Add(model);
            return PartialView("_Products", products.ToList());
        }
        public ActionResult ProductDelete(string id)
        {
            int productId = Convert.ToInt32(id);
            var products = _invoiceService.Remove(productId, HttpContext);
            return PartialView("_Products", products);
        }
        public JsonResult GetCustomer()
        {
            return Json(db.Customer.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerById(string id)
        {
            if (id != "")
            {
                int customerId = Convert.ToInt32(id);
                var customer = db.Customer.Where(x => x.Id == customerId);
                return Json(customer);
            }
            else
            {
                CustomerModel model = new CustomerModel();
                model.Name = "";
                model.Addess = "";
                return Json(model);
            }
        }
        public JsonResult GetProductById(string id)
        {
            if (id != "")
            {
                int productId = Convert.ToInt32(id);
                var product = db.Products.Where(x => x.Id == productId);
                return Json(product);
            }
            else
            {
                var product = new ProductModel();
                product.Description = "";
                product.UnitPrice = 0.0;
                return Json(product);
            }
        }
        public JsonResult GetProduct()
        {
            return Json(db.Products.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            LoadStatus();
            InvoiceViewModel vm = new InvoiceViewModel();
            vm.ProductList = _invoiceService.ProductList(HttpContext);
            return View(vm);
        }
        [HttpPost]
        public ActionResult Create(InvoiceModel invoice)
        {
            if (ModelState.IsValid)
            {
                var products = _invoiceService.ProductList(HttpContext);
                var invoicemodel = new InvoiceModel
                {
                    InvoiceCode = invoice.InvoiceCode,
                    InvoiceDate = DateTime.Parse(invoice.InvoiceDate.ToString("d")),
                    DueDate = DateTime.Parse(invoice.InvoiceDate.ToString("d")),

                    Notes = invoice.Notes,
                    Status = invoice.Status,

                    TotalAmount = invoice.TotalAmount,
                    BalanceDue = invoice.BalanceDue,
                    AmountPaid = invoice.AmountPaid,

                    CustomerId = invoice.CustomerId
                };
                foreach (var p in products)
                {
                    invoicemodel.Products.Add(new ProductForInvoice
                    {
                        ProductId = p.ProductId,
                        Quantity = p.Quantity,
                        Amount = p.Amount
                    });
                }
                invoiceRepository.Add(invoicemodel);
                _invoiceService.Clear(HttpContext);
                return RedirectToAction("Index", "Invoice");
            }
            else
            {
                LoadStatus();
                InvoiceViewModel vm = new InvoiceViewModel();
                vm.Invoice = invoice;
                vm.ProductList = _invoiceService.ProductList(HttpContext);
                return View(vm);
            }

        }
        public ActionResult Details(int id)
        {
            var invoice = db.Invoices.Include(x => x.CustomerModel)
                .Include(x => x.Products)
                .Include("Products.Product")
                .FirstOrDefault(x => x.Id == id);

            var company = db.Company.FirstOrDefault();

            InvoiceDetailsViewModel vm = new InvoiceDetailsViewModel();
            vm.Company = company;
            vm.Invoice = invoice;

            if (invoice != null && company != null)
                return View(vm);
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            LoadStatus();
            var invoice = db.Invoices.Include(x => x.CustomerModel)
                 .Include(x => x.Products)
                 .Include("Products.Product")
                 .FirstOrDefault(x => x.Id == id);
            return View(invoice);
        }
        [HttpPost]
        public ActionResult Edit(InvoiceModel model)
        {
            var lastinvoice = db.Invoices.Include(x => x.CustomerModel)
                .Include(x => x.Products)
                .Include("Products.Product")
                .FirstOrDefault(x => x.Id == model.Id);

            if (lastinvoice != null)
            {
                lastinvoice.InvoiceCode = model.InvoiceCode;
                lastinvoice.Status = model.Status;
                lastinvoice.Notes = model.Notes;

                lastinvoice.TotalAmount = model.TotalAmount;
                lastinvoice.AmountPaid = model.AmountPaid;
                lastinvoice.BalanceDue = model.BalanceDue;

                lastinvoice.DueDate = model.DueDate;
                lastinvoice.InvoiceDate = model.InvoiceDate; 
            
                invoiceRepository.Update(lastinvoice);
            }
            return RedirectToAction("Index", "Invoice");
        }
        public ActionResult Delete(int id)
        {
            var invoice = db.Invoices.Include(x => x.CustomerModel)
                  .Include(x => x.Products)
                  .Include("Products.Product")
                  .FirstOrDefault(x => x.Id == id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index", "Invoice");
        }
        private void LoadStatus()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Paid", Value = "Paid" });
            li.Add(new SelectListItem { Text = "Due", Value = "Due" });
            ViewData["country"] = li;
        }

        public FileResult ExportTo(string fileType, int id)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/Invoice.rdlc");

            var currentInvoice = db.Invoices.Include(x => x.CustomerModel)
                  .Include(x => x.Products)
                  .Include("Products.Product")
                  .FirstOrDefault(x => x.Id == id);

            var company = db.Company.ToList();
            
                var products = currentInvoice.Products.ToList();
                var customer = new List<CustomerModel>
                {
                    currentInvoice.CustomerModel
                };

                var invoice = new List<InvoiceModel>
                {
                    new InvoiceModel
                    {
                        InvoiceCode = currentInvoice.InvoiceCode,
                        InvoiceDate = currentInvoice.InvoiceDate,
                        DueDate = currentInvoice.DueDate,
                        Notes = currentInvoice.Notes,
                        Status = currentInvoice.Status,
                        TotalAmount = currentInvoice.TotalAmount,
                        BalanceDue = currentInvoice.BalanceDue,
                        AmountPaid = currentInvoice.AmountPaid
                    }
                };

                var productsInvoice = products.Select(x => new
                {
                    Product = x.Product.Product,
                    Description = x.Product.Description,
                    UnitPrice = x.Product.UnitPrice,
                    Amount = x.Amount,
                    Quantity = x.Quantity
                });
          

            var rd1 = new ReportDataSource("Company", company);
            var rd2 = new ReportDataSource("Products", productsInvoice);
            var rd3 = new ReportDataSource("Customer", customer);
            var rd4 = new ReportDataSource("Invoice", invoice);
            localReport.DataSources.Add(rd1);
            localReport.DataSources.Add(rd2);
            localReport.DataSources.Add(rd3);
            localReport.DataSources.Add(rd4);

            string reportType = fileType;
            string mimeType;
            string encoding;
            string fileNameExtension = (fileType == "Excel") ? "xlsx" : (fileType == "Word" ? "doc" : "pdf");
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType, "", out mimeType, out encoding,
                                                out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=Urls." + fileNameExtension);
            Response.AddHeader("content-disposition", "attachment; filename=invoice-"+id+"." + fileNameExtension);
         
            return File(renderedBytes, fileNameExtension);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SendAllInvoices()
        {
            //Get all users 
            var AllUsers = db.Customer.ToList();
            
            int count = 0;
            foreach (var item in AllUsers)
            {
                var email = new EmailViewModel();
                var InvoiceId = db.Invoices.Where(x => x.CustomerId == item.Id).FirstOrDefault().Id;
                var invoiceToSend = Stream("pdf", InvoiceId);
                var attachmentStream = new MemoryStream(invoiceToSend);
                var contentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                email.To = item.Email;
                email.Message = "June Invoice";
                email.Attach(new Attachment(attachmentStream, "The Grind Invoice", contentType.ToString()));
                email.Send();
               
                count++;
            }
            return Json(count.ToString(), JsonRequestBehavior.AllowGet);
            // return View();
        }

        public ActionResult SendSingleInvoices()
        {
            var email = new EmailViewModel
            {
                To = "drewlindsey017@gmail.com",
                Message = "June Invoice"
            };
          
            //Get user invoice
            var invoiceToSend = Stream("pdf",2);
            var attachmentStream = new MemoryStream(invoiceToSend);
            var contentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
            email.Attach(new Attachment(attachmentStream, "Lindsey Drew Invoice" , contentType.ToString()));
            email.Send();

            return Json("test", JsonRequestBehavior.AllowGet);
        }

        public byte[] Stream(string fileType, int id)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/Invoice.rdlc");

            var currentInvoice = db.Invoices.Include(x => x.CustomerModel)
                  .Include(x => x.Products)
                  .Include("Products.Product")
                  .FirstOrDefault(x => x.Id == id);

            var company = db.Company.ToList();

            var products = currentInvoice.Products.ToList();
            var customer = new List<CustomerModel>
                {
                    currentInvoice.CustomerModel
                };

            var invoice = new List<InvoiceModel>
                {
                    new InvoiceModel
                    {
                        InvoiceCode = currentInvoice.InvoiceCode,
                        InvoiceDate = currentInvoice.InvoiceDate,
                        DueDate = currentInvoice.DueDate,
                        Notes = currentInvoice.Notes,
                        Status = currentInvoice.Status,
                        TotalAmount = currentInvoice.TotalAmount,
                        BalanceDue = currentInvoice.BalanceDue,
                        AmountPaid = currentInvoice.AmountPaid
                    }
                };

            var productsInvoice = products.Select(x => new
            {
                Product = x.Product.Product,
                Description = x.Product.Description,
                UnitPrice = x.Product.UnitPrice,
                Amount = x.Amount,
                Quantity = x.Quantity
            });


            var rd1 = new ReportDataSource("Company", company);
            var rd2 = new ReportDataSource("Products", productsInvoice);
            var rd3 = new ReportDataSource("Customer", customer);
            var rd4 = new ReportDataSource("Invoice", invoice);
            localReport.DataSources.Add(rd1);
            localReport.DataSources.Add(rd2);
            localReport.DataSources.Add(rd3);
            localReport.DataSources.Add(rd4);

            string reportType = fileType;
            string mimeType;
            string encoding;
            string fileNameExtension = (fileType == "Excel") ? "xlsx" : (fileType == "Word" ? "doc" : "pdf");
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType, "", out mimeType, out encoding,
                                                out fileNameExtension, out streams, out warnings);
            return renderedBytes;
        }
    }
}