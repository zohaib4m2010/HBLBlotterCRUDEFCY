using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Models;
using WebBlotter.Classes;
namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BranchesController : Controller
    {
        // GET: Branches
        public ActionResult Branches()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/Branches/GetAllBranches");
                response.EnsureSuccessStatusCode();
                List<Models.Branches> Branches = response.Content.ReadAsAsync<List<Models.Branches>>().Result;
                
                return View(Branches);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Branches Branches)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Branches.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/Branches/InsertBranches", Branches);
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction("Branches");
                }
            }
            catch (Exception ex) { }
            return View(Branches);
        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/Branches/GetBranches?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.Branches Branches = response.Content.ReadAsAsync<Models.Branches>().Result;
            return View(Branches);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Branches Branches)
        {
            Branches.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/Branches/UpdateBranches", Branches);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Branches");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/Branches/DeleteBranches?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Branches");
        }
    }
}