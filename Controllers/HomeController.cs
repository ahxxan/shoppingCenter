using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectFinal.Models;
namespace projectFinal.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductDetail()
        {
            Database1Entities3 db = new Database1Entities3();
            int id = 1;
            //Database1Entities db = new Database1Entities();
            return View(db.products.First(x => x.prodid == 1));
            //return View();
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult validate()
        {
            Database1Entities3 db = new Database1Entities3();
            string mail = Request["email"];
            string password = Request["password"];
            var q = db.users.Where(x => x.email == mail).Where(y => y.password == password).Count();
            user q1 = db.users.First(x => x.email == mail && x.password == password);
            if (q != 0)
            {
                List<product> list = new List<product>();
                Session["val"] = list;
               // Session["email"] = mail;
                Session["id"] = q1.Id;
                Session["valid"] = "yes";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login");
            }

        }
        public ActionResult register()
        {
            return View();
        }

        public JsonResult Check()
        {
            string u = Request["u"];
            Database1Entities3 db = new Database1Entities3();
            var q = db.users.Where(x => x.email == u).Count();

            if (q == 0)
            {


                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {


                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult saveTodb()
        {
            Database1Entities3 db = new Database1Entities3();
            user u = new user();
            u.username = Request["name"];
            u.email = Request["email"];
            u.password = Request["password"];
            u.phone = Request["phone"];
            u.address = Request["address"];
            db.users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult addToCart(int id)
        {
            // int ide = obj.prodid;
            //Book obj = cx.Books.Find(ide);
            if (Session["val"] != null)
            {
                Database1Entities3 db = new Database1Entities3();
                // int id = int.Parse(Request["prodid"]);
                product obj = db.products.Find(id);
                List<product> l = (List<product>)Session["val"];
                l.Add(obj);
                return View("index");
            }else
            return View("login");

        }

        public ActionResult showCart()
        {

            if (Session["val"] != null)
            {
                List<product> l = (List<product>)Session["val"];
                return View(l);
            }
            else
            {

                return View("Index");
            }
        }
         public ActionResult logout()
        {
            Session["valid"] = null;
            return RedirectToAction("index");
        }

         public ActionResult buy()
         {

             if (Session["val"] != null)
             {
                 product obj = new product();
                 float sum = 0.0f;
                 List<product> l = (List<product>)Session["val"];
                 foreach (var i in l)
                 {
                     sum += (float)i.price;

                 }
                 obj.price = sum;

                 return View(obj);
             }
             else
             {
                 return View("Index");
             }
              
         }

         public ActionResult checkOut()
         {
             if (Session["val"] != null)
             {
                 Database1Entities3 db= new Database1Entities3();
               shopping shopObj=null;
               int id = (int)Session["id"];
                 List<product> l1 = (List<product>)Session["val"];
                 foreach (var i in l1)
                 {
                    shopObj = new shopping();
                    shopObj.userid = id;
                    shopObj.prodid = i.prodid;
                    shopObj.prodname = i.prodname;
                    shopObj.amount = i.price;
                         
                 }
                 //shopObj.userid = 2;
                 //shopObj.prodid = 2;
                 //shopObj.prodname = "bata";
                 //shopObj.amount = 1000.0f;
                 db.shoppings.Add(shopObj);
                 db.SaveChanges();
                 
                 return View("Index");
             }
             else
             {
                 return View("login");
             }
         }

    }
}
