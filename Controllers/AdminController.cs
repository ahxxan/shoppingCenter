using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectFinal.Models;

namespace projectFinal.Controllers
{

    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        IAdminRepository repo;
        public AdminController(IAdminRepository obj)
        {
            repo = obj;
        }

        //AdminController()
        //{
            
        //}

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult validate()
        {
            Database1Entities3 db = new Database1Entities3();
            string mail = Request["email"];
            string password = Request["password"];
           // var q = db.admins.Where(x => x.email == mail).Where(y => y.password == password).Count();
           admin q1 = db.admins.First(x => x.email == mail && x.password == password);
            if (q1 != null)
            {
               // List<product> list = new List<product>();
               // Session["val"] = list;
                // Session["email"] = mail;
                Session["admin"] = q1;
                Session["valid"] = "yes";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login");
            }

        }

        public ActionResult contact()
        {
            return View();
        }
        public ActionResult AddAdmin()
        {
            if (Session["admin"] != null)
            {
                return View();
            }
            else
                return View("../Home/login");
        }
        [HttpPost]
        public ActionResult AddAdmin(admin a )
        {
            Database1Entities3 db = new Database1Entities3();
            db.admins.Add(a);
            db.SaveChanges();
            return RedirectToAction("../Home/Index");
        }

        public ActionResult AddFile()
        {
            if (Session["admin"] != null)
            {
                return View();
            }
            else
                return View("../Home/login");
        }

        [HttpPost]
        public ActionResult Save()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                file.SaveAs(Server.MapPath(@"~\Files\" + file.FileName));
            }

            return View("Index");
        }

        public ActionResult addProduct()
        {
            if (Session["admin"] != null)
            {
                return View();
            }
            else
                return View("../Home/login");
        }
         [HttpPost]
        public ActionResult addProduct(product obj)
        {
            if (ModelState.IsValid)
            {
                repo.addProduct(obj);
                
            }
            return View();
        }

    }
}
