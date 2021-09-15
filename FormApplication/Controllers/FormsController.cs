using FormApplication.Data;
using FormApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormApplication.Controllers
{
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult Index()
        {
            List<FullFormModel> fullFormModels = new List<FullFormModel>();
            using (FormDAO formDAO = new FormDAO())
            {
                fullFormModels = formDAO.FetchAll();
            }
            return View("Index", fullFormModels);
        }
        public ActionResult FormCreate()
        {

            List<StatutoryModel> statuses = new List<StatutoryModel>();
            List<GenderModel> gender = new List<GenderModel>();
            List<HousingModel> houses = new List<HousingModel>();
            using (FormDAO formDAO = new FormDAO())
            {
                statuses = formDAO.AccessStatusList();
                gender = formDAO.AccessGender();
                houses = formDAO.AccessHouseUnit();
                ViewBag.housebatch = houses;
                ViewBag.status = statuses;
                ViewBag.gender = gender;
            }
            return View("FormCreate");
        }
        public ActionResult Details(int id)
        {
            FullFormModel fullFormModel = new FullFormModel();
            using (FormDAO formDAO = new FormDAO())
            {
                fullFormModel = formDAO.FetchOne(id);
            }
            return View("Details", fullFormModel);
           
        }
        public ActionResult Edit(int id)
        {
            List<StatutoryModel> statuses = new List<StatutoryModel>();
            List<GenderModel> gender = new List<GenderModel>();
            List<HousingModel> houses = new List<HousingModel>();
            FullFormModel fullFormModel = new FullFormModel();
            using (FormDAO formDAO = new FormDAO())
            {
                statuses = formDAO.AccessStatusList();
                gender = formDAO.AccessGender();
                houses = formDAO.AccessHouseUnit();
                ViewBag.housebatch = houses;
                ViewBag.status = statuses;
                ViewBag.gender = gender;
                fullFormModel = formDAO.FetchOne(id);
            }
            return View("OnlineForm", fullFormModel);
            
        }
        public ActionResult Create()
        {
            List<StatutoryModel> statuses = new List<StatutoryModel>();
            List<GenderModel> gender = new List<GenderModel>();
            List<HousingModel> houses = new List<HousingModel>();
            //List<HousingModel> types = new List<HousingModel>();

            using (FormDAO formDAO = new FormDAO())
            {
                statuses = formDAO.AccessStatusList();
                gender = formDAO.AccessGender();
                houses = formDAO.AccessHouseUnit();
                ViewBag.housebatch = houses;
                ViewBag.status = statuses;
                ViewBag.gender = gender;
            }
            return View("OnlineForm");
        }
        public ActionResult Delete(int id)
        {
            List<FullFormModel> form = new List<FullFormModel>();
            using (FormDAO formDAO = new FormDAO())
            {
                formDAO.Delete(id);
                form = formDAO.FetchAll();
            }
            return View("Index", form);
        }
        [HttpPost]
        public ActionResult ProcessCreate (FullFormModel fullFormModel)
        {
            //save to the db
            using (FormDAO formDAO = new FormDAO())
            {
                formDAO.CreateOrUpdate(fullFormModel);
            }
            return View("Details", fullFormModel);
        }
        [HttpPost]
        public ActionResult ProcessNewForm(FormCollection formCollection)
        {
            //save to the db
            using (FormDAO formDAO = new FormDAO())
            {
                if(formCollection.Count <= 8)
                {
                    formDAO.CreateOccupants(formCollection);
                }
                else
                {
                    formDAO.CreateOccupants(formCollection);
                    formDAO.CreateSpouses(formCollection);
                }
            }

            return View("ProcessNewForm");
        }
        public ActionResult ViewDetails() 
        {
            List<FormModel> form = new List<FormModel>();
            using (FormDAO formDAO = new FormDAO())
            {
                form = formDAO.AccessFormView();
            }
            return View("ViewDetails", form);
        }
    }
}


//string spouse = "";
////Request.Form[]
//ViewData["Names"] = spouse;