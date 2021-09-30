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

                onlineModel = formDAO.FetchIndividualData(id);
            }
            return View("FullView", onlineModel);
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
        public ActionResult SaveAllEdits(FormCollection formCollection)
        {
            //save to the db
            using (FormDAO formDAO = new FormDAO())
            {
                if(formCollection.Count <= 9)
                {
                    formDAO.SaveEditedOccupant(formCollection); 
                }
                else
                {
                    formDAO.SaveEditedOccupant(formCollection);
                    formDAO.SaveEditedSpouses(formCollection);
                    formDAO.CheckForNewSpouse(formCollection);
                }
            }
            return View("SaveAllEdits");
        }

    }
}
