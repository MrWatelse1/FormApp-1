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
        public ActionResult EditSpouse(int id)
        {
            //Edit a particular spouse
            SpouseModel spouseModel = new SpouseModel();
            using (FormDAO formDAO = new FormDAO())
            {
                List<StatutoryModel> statuses = formDAO.AccessStatusList();
                List<GenderModel>  gender = formDAO.AccessGender();
                List<HousingModel> houses = formDAO.AccessHouseUnit();
                ViewBag.housebatch = houses;
                ViewBag.status = statuses;
                ViewBag.gender = gender;

                spouseModel = formDAO.FetchSpouse(id);
            }
            return View("SpouseEdit",spouseModel);
        }
        public ActionResult ViewSpouse(FullFormModel fullFormModel)
        {
            
            //View All Spouse(s)
            List<SpouseModel> spouseModels = new List<SpouseModel>();
            using(FormDAO formDAO = new FormDAO())
            {
                int newId = fullFormModel.ID;
                if (formDAO.CheckForSpouse(fullFormModel.ID))
                {
                    spouseModels = formDAO.FetchAllSpouse(newId);

                    return View("EditSpouse", spouseModels);
                }
                else
                {
                    return View("NoSpouse");
                }
            }
        }
        public ActionResult FormCreate()
        {
            using (FormDAO formDAO = new FormDAO())
            {

                List<StatutoryModel> statuses = formDAO.AccessStatusList();
                List<GenderModel> gender = formDAO.AccessGender();
                List<HousingModel> houses = formDAO.AccessHouseUnit();
                ViewBag.housebatch = houses;
                ViewBag.status = statuses;
                ViewBag.gender = gender;
            }
            return View("FormCreate");
        }
        public ActionResult Details(int id)
        {
            List<OnlineModel> online = new List<OnlineModel>();
            using (FormDAO formDAO = new FormDAO())
            {
                online = formDAO.EditDetails(id);
            }
            return View("Details", online);
           
        }
        public ActionResult Edit(int id)
        {
            FullFormModel onlineModel = new FullFormModel();
            using (FormDAO formDAO = new FormDAO())
            {

                List<StatutoryModel> statuses = formDAO.AccessStatusList();
                List<GenderModel> gender = formDAO.AccessGender();
                List<HousingModel> houses = formDAO.AccessHouseUnit();
                ViewBag.housebatch = houses;
                ViewBag.status = statuses;
                ViewBag.gender = gender;

                onlineModel = formDAO.FetchOne(id);
            }
            return View("OccupantView", onlineModel);
        }
        public ActionResult Create()
        {
            using (FormDAO formDAO = new FormDAO())
            {

                List<StatutoryModel> statuses = formDAO.AccessStatusList();
                List<GenderModel> gender = formDAO.AccessGender();
                List<HousingModel> houses = formDAO.AccessHouseUnit();
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
        [HttpPost]
        public ActionResult SaveOccupantEdit(FullFormModel fullFormModel)
        {
            //save to db
            using (FormDAO formDAO = new FormDAO())
            {
                formDAO.UpdateOccupant(fullFormModel);
            }
            return View("ViewOccupantChanges", fullFormModel);
        }
        [HttpPost]
        public ActionResult SaveSpouseEdit(SpouseModel spouseModel)
        {
            //save to db
            using(FormDAO formDAO = new FormDAO())
            {
                formDAO.UpdateSpouse(spouseModel);
            }
            return View("ViewSpouseChanges", spouseModel);
        }
    }
}
