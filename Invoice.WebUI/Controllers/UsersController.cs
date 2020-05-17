using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invoice.Model.DataAceess;
using Invoice.Model.Model;
using Invoice.WebUI.ViewModel;

namespace Invoice.WebUI.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users

        private BaseRepository<UserModel> userRepository; 
        public UsersController()
        {
            userRepository = new BaseRepository<UserModel>();
        }
        public ActionResult Index()
        {
            var user = LoginUser.GetLogin();
            if (user != null)
            {
                user.Password = "";
                user.ConfirmPassword = "";
                return View(user);
            }
                
            return View("Error");
        }

        public ActionResult Save(UserViewModel model)
        {
            var user = userRepository.All().SingleOrDefault(x=> x.Id == model.Id);
            user.Email = model.Email;
            user.FullName = model.FullName;
            userRepository.Update(user);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword(UserPassViewModel model)
        {
            var user = userRepository.All().SingleOrDefault(x => x.Id == model.Id);
            user.Password = model.Password;
            user.ConfirmPassword = model.ConfirmPassword; 
            userRepository.Update(user);
            return RedirectToAction("Index", "Home");
        }
    }
}