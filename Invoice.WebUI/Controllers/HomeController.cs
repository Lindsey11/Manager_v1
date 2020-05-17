using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Invoice.Model.DataAceess;
using Invoice.Model.DataContext;
using Invoice.Model.Model;
using Invoice.WebUI.Helper;
using Invoice.WebUI.ViewModel;

namespace Invoice.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private BaseRepository<UserModel> _userRepository;
        public HomeController()
        {
            _userRepository = new BaseRepository<UserModel>();
        }
        [Authorize]
        public ActionResult Index()
        {
            DashBoardViewModel Dvm = new DashBoardViewModel();
            return View(Dvm);
        }

        [AllowAnonymous]
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                _userRepository.Add(model);
                return RedirectToAction("Login");

            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserModel model)
        {
            var membershipHelper = new MemberShipHelper();
            try
            {
                if (Membership.ValidateUser(model.Email, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Session["LoginUser"] = membershipHelper.GetUserProfile(model.Email, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Msg"] = "Invalid User Name and Password!";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception e)
            {
                TempData["Msg"] = e.Message;
                return RedirectToAction("Login");
            }
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Home", "Home");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}