using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DAL;

namespace Web.Controllers
{
    //[RoutePrefix("")]
    public class ProductController : Controller
    {
        private DbManager _dbManager { get; set; }

        public ProductController()
        {
            _dbManager = new DbManager();


            //////For test!
            //_dbManager.ProductRepository.GetItemByID(1).IsSold = false;
            //_dbManager.Save();
            //////!!!!!!!!!!!!!!!!!
             
        }

        //[Route("")]
        public ActionResult Index(string sortingType)
        {

            SortBy sort;
            if( !Enum.TryParse<SortBy>(sortingType, out sort))
            {
                sort = SortBy.Date;
            }

            ReturnProductsToShowFromDelatedSession();

            List<int> allUsersCart = (List<int>)(HttpContext.Application["productsCommon"]);
            var products = _dbManager.ProductRepository.GetAviableItems(allUsersCart);
            switch (sort)
            {
                case SortBy.Date:
                    products = products.OrderByDescending(i => i.PublicationDate);
                    break;
                case SortBy.Price:
                    products = products.OrderBy(i => i.Price);
                    break;
            }
            return View(products);
        }

        //[Route("Shopping_Cart")]
        public ActionResult ShoppingCart()
        {
            ReturnProductsToShowFromDelatedSession();


            List<int> currentUserCart = (List<int>)Session["productsCurrentUser"];
            var products = _dbManager.ProductRepository.GetItems(currentUserCart);
            ShoppingCartViewModel model;
            if (HttpContext.User.Identity.IsAuthenticated)
                model = new ShoppingCartViewModel(10);
            else
                model = new ShoppingCartViewModel(0);
            model.Products = products;
            return View(model);
        }

        //[Route("Upload_Product_for_Sale")]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Details(object id, string error)
        {
            int prodId = Convert.ToInt32(id);
            if(error!=null)
                ModelState.AddModelError("", error);
            return View(_dbManager.ProductRepository.GetItemByID(prodId));
        }


        //[Route("Upload_Product_for_Sale")]
        [Authorize]
        [HttpPost]
        public ActionResult Create(ProductCreateViewModel model, HttpPostedFileBase pic1, HttpPostedFileBase pic2, HttpPostedFileBase pic3)
        {
            model.Files = new PicturesViewModel() { MainImage = pic1, Image2 = pic2, Image3 = pic3 };

            Respone feedback = _dbManager.AddProduct(model, HttpContext.User.Identity.Name);
            if (feedback.IsDone)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", feedback.Message);
                return View(model);
            }
        }

        public ActionResult AddToCart(object id)
        {
            ReturnProductsToShowFromDelatedSession();


            int currentProd = Convert.ToInt32(id);
            var product = _dbManager.ProductRepository.GetItemByID(currentProd);
            if(HttpContext.User.Identity.IsAuthenticated && product.UploadedByUser.Username == HttpContext.User.Identity.Name)
            {
                return RedirectToAction("Details", new{ id= product.ID, error= "You can't buy your own product" });
            }
            List<int> allUsersCart = (List<int>)(HttpContext.Application["productsCommon"]);
            if (allUsersCart == null)
                allUsersCart = new List<int>();
            if (!allUsersCart.Contains(currentProd))
            {
                List<int> currentUserCart = (List<int>)Session["productsCurrentUser"];
                if (currentUserCart == null)
                    currentUserCart = new List<int>();
                currentUserCart.Add(currentProd);
                Session["productsCurrentUser"] = currentUserCart;
                allUsersCart.Add(currentProd);
                HttpContext.Application["productsCommon"] = allUsersCart;
                return RedirectToAction("ShoppingCart");
            }
            else
            {
                ModelState.AddModelError("", "Sorry, the product is unaviable now");
                return RedirectToAction("Details", id);
            }
        }

        public ActionResult RemoveFromCart(object id)
        {
            int currentProd = Convert.ToInt32(id);
            List<int> currentUserCart = (List<int>)Session["productsCurrentUser"];
            currentUserCart.Remove(currentProd);
            Session["productsCurrentUser"] = currentUserCart;
            List<int> allUsersCart = (List<int>)(HttpContext.Application["productsCommon"]);
            allUsersCart.Remove(currentProd);
            HttpContext.Application["productsCommon"] = allUsersCart;
            return RedirectToAction("ShoppingCart");
        }

        public ActionResult Buy()
        {
            ReturnProductsToShowFromDelatedSession();


            List<int> allUsersCart = (List<int>)(HttpContext.Application["productsCommon"]);
            List<int> currentUserCart = (List<int>)Session["productsCurrentUser"];
            IEnumerable<Product> products;
            if (currentUserCart != null)
                products = _dbManager.ProductRepository.GetItems(currentUserCart);
            else
            {
                TempData.Add("error", "It seems you have been thinking too long and your shopping Cart has been reset and cleared :(");
                return RedirectToAction("ShoppingCart");
            }
            

            foreach (var product in products)
            {
                if(HttpContext.User.Identity.IsAuthenticated && product.UploadedByUser.Username == HttpContext.User.Identity.Name)
                {
                    TempData.Add("error", String.Format($"You can't buy your own product: {product.Name}"));
                    return RedirectToAction("ShoppingCart");
                }
            }

            foreach (var product in products)
            {
                _dbManager.ProductRepository.GetItemByID(product.ID).IsSold = true;
                allUsersCart.Remove(product.ID);
            }


            Session["productsCurrentUser"] = null;
            HttpContext.Application["productsCommon"] = allUsersCart;
            _dbManager.Save();

            TempData.Add("success", "Thank you for your purchase!");
            return RedirectToAction("ShoppingCart");
        }

        public ActionResult About()
        {
            return View();
        }

        public void ReturnProductsToShowFromDelatedSession()
        {
            List<int> allUsersCart = (List<int>)(HttpContext.Application["productsCommon"]);
            while (allUsersCart != null && allUsersCart.Count > 0 && DelatedSessionData.ShoppingCart.Count > 0)
            {
                int temp;
                if (DelatedSessionData.ShoppingCart.TryDequeue(out temp))
                    allUsersCart.Remove(temp);
                else
                    break;
            }

        }

    }
}