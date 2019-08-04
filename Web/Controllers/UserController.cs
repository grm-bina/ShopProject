using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DAL;
using System.Web.Security;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private DbManager _dbManager { get; set; }

        public UserController()
        {
            _dbManager = new DbManager();

        }

        //[Route("New_User")]
        public ActionResult Register()
        {
            return View();
        }

        //[Route("New_User")]
        [HttpPost]
        public ActionResult Register(User user)
        {
            Respone feedback = _dbManager.AddUser(user);
            if (!feedback.IsDone)
            {
                ModelState.AddModelError("", feedback.Message);
                return View(user);
            }
            else
                return RedirectToAction("Index","Product");
        }

        public ActionResult LogIn()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult LogIn(LoginViewModel model)
        {
            User current;
            if(!_dbManager.UserRepository.AutorizationCheck(model, out current))
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return PartialView(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(current.Username, true);
                return PartialView();
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {

            List<int> allUsersCart = (List<int>)(HttpContext.Application["productsCommon"]);
            List<int> currentUserCart = (List<int>)Session["productsCurrentUser"];
            if (currentUserCart != null)
            {
                foreach (var item in currentUserCart)
                {
                    allUsersCart.Remove(item);
                }
                Session["productsCurrentUser"] = null;
                HttpContext.Application["productsCommon"] = allUsersCart;

            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Product");
        }


    }
}