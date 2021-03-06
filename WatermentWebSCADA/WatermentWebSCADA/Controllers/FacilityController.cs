﻿using System;
using System.Linq;
using System.Web.Mvc;
using WatermentWebSCADA.ViewModels;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Helpers;
using WatermentWebSCADA.Models;
using WatermentWebSCADA.CustomFilters;

//Uses the model "MainViewModel"- Same as MainSiteController//

namespace WatermentWebSCADA.Controllers
{
    public class FacilityController : Controller
    {
        Models.watermentdbEntities db = new Models.watermentdbEntities();

        string IpClient;


        // GET: Facility
        [AuthLog(Roles = "Admin, Superuser, Maintenance, User")]
        public ActionResult FacilityDetails(int? id)
        {
            if (id == null) //Error handling if "int? id" is missing from the link. 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Fetches the current row id selected in the table Client_Conection
            foreach (var item2 in db.Client_Conection.Where(c => c.user == id.ToString()))
            {
                IpClient = item2.ip;
            }
            // Fetches the current row id seleted in the table facilities and sets the facility IP to
            // the Client IP.
            foreach (var item in db.facilities.Where(c => c.Id == id))
            {

                item.IP = IpClient;
            }

            // Saves the changes to the DB
            db.SaveChanges();


            using (var db = new Models.watermentdbEntities())
            {
                var model = new MainViewModel
                {
                    //Getting desired data from the database, and returning it to the view.
                    Alarms = db.alarms.Where(x => x.equipments_facilities_Id == id).Where(o => o.Status == "Active").OrderByDescending(x => x.AlarmOccured).ToList(),
                    Facilites = db.facilities.Where(c => c.Id == id).ToList(),
                    Countries = db.countries.ToList(),
                    Locations = db.locations.ToList(),
                    Users = db.User.ToList(),
                    Equipments = db.equipments.Where(x => x.facilities_Id == id).ToList(),


                };

                return View(model);
            }
        }


        [AuthLog(Roles = "Admin, Superuser, Maintenance, User")]
        public ActionResult FacilityOverview(int? id)
        {

            using (var db = new Models.watermentdbEntities())
            {
                var model = new MainViewModel
                {

                    countries = db.countries_with_facilites_view.ToList(),
                    Facilites = db.facilities.Where(x => x.locations_countries_Id == id).ToList(),

                };


                return View(model);
            }
        }



        [AuthLog(Roles = "Admin, Superuser, Maintenance, User")]
        public ActionResult AddFacility2()
        {
            //creates dropdown lists for location, country and continents. Displays name, returns ID
            ViewBag.locations_Id = new SelectList(db.locations, "Id", "StreetAddress");
            ViewBag.locations_countries_Id = new SelectList(db.countries, "Id", "Name");
            ViewBag.locations_countries_continents_Id = new SelectList(db.continents, "Id", "Name");
            return View();
        }

        [AuthLog(Roles = "Admin, Superuser, Maintenance, User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFacility2([Bind(Include = "Id,Name,IP,Domain,SerialNumber, ProgramVersion,locations_Id,locations_countries_Id,locations_countries_continents_Id")] facilities facilities)
        {
            if (ModelState.IsValid)
            {
                
                //Saves data from view to database
                db.facilities.Add(facilities);
                try
                {
                    db.SaveChanges();
                }
                catch (InvalidOperationException e)
                {

                    throw new InvalidOperationException("Data could not be saved", e);
                }
                //Returns Facility overview when data has been saved
                return RedirectToAction("FacilityOverview");
            }
            //writes data from dropdown lists to facilities table(collects data from locations, countries and continents tables)
            ViewBag.locations_Id = new SelectList(db.locations, "Id", "StreetAddress", facilities.locations_Id);
            ViewBag.locations_countries_Id = new SelectList(db.countries, "Id", "Name", facilities.locations_countries_Id);
            ViewBag.locations_countries_continents_Id = new SelectList(db.continents, "Id", "Name", facilities.locations_countries_continents_Id);

            return View(facilities);//opens facility overview
        }

        [AuthLog(Roles = "Admin, Superuser")]
        public ActionResult AddLocation()
        {
            //creates dropdown lists for country and continents. Displays name, returns ID
            ViewBag.countries_Id = new SelectList(db.countries, "Id", "Name");
            ViewBag.countries_continents_Id = new SelectList(db.continents, "Id", "Name");
            return View(); //returns add location view
        }

        [AuthLog(Roles = "Admin, Superuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLocation([Bind(Include = "Id,StreetAddress,Postcode,County,City,countries_Id,countries_continents_Id")] locations locations)
        {
            if (ModelState.IsValid)
            {
                //writes input from view to database
                db.locations.Add(locations);
                db.SaveChanges();
                return RedirectToAction("AddFacility2");// redirect user to add facility view when data has been added to db
            }
            //writes data from dropdown lists to location table(collects data from countries and continents tables)
            ViewBag.locations_Id = new SelectList(db.countries, "Id", "Name", locations.countries_Id);
            ViewBag.locations_continents_id = new SelectList(db.continents, "Id", "Name", locations.countries_continents_Id);
            return View(locations);//returns add facility
        }
        [AuthLog(Roles = "Admin, Superuser")]
        public ActionResult EditFacilities(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facilities facilities = db.facilities.Find(id);//gets id from opened facility
            if (facilities == null)
            {
                return HttpNotFound();
            }
            //creates dropdown lists for location, country, continents and user. Displays name, returns ID
            ViewBag.locations_Id = new SelectList(db.locations, "Id", "StreetAddress");
            ViewBag.locations_countries_Id = new SelectList(db.countries, "Id", "Name");
            ViewBag.locations_countries_continents_Id = new SelectList(db.continents, "Id", "Name");
            ViewBag.User_Id = new SelectList(db.User, "Id", "UserName");
            return View(facilities);
        }
        [AuthLog(Roles = "Admin, Superuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFacilities([Bind(Include = "Id,Name,IP,Domain,SerialNumber,ProgramVersion, locations_Id, locations_countries_Id,locations_countries_continents_Id,User_Id")] facilities facilities)
        {
            if (ModelState.IsValid)
            {
                //writes the user input data to database
                db.Entry(facilities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FacilityOverview");
            }
            //writes data from dropdown lists to facility table(collects data from locations, countries, continents and user tables)
            ViewBag.locations_Id = new SelectList(db.locations, "Id", "StreetAddress", facilities.locations_Id);
            ViewBag.locations_countries_Id = new SelectList(db.countries, "Id", "Name", facilities.locations_countries_Id);
            ViewBag.locations_countries_continents_Id = new SelectList(db.continents, "Id", "Name", facilities.locations_countries_continents_Id);
            ViewBag.User_Id = new SelectList(db.User, "Id", "UserName", facilities.User_Id);

            return View(facilities);
        }




        public static class Theme
        {

            public const string Green = "<Chart BackColor=\"#ABC27A\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"229, 217, 148\" BorderColor=\"#ABC27A\" BorderlineDashStyle=\"Solid\" BorderWidth=\"5\" Palette=\"Chocolate\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"#ABC27A\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"229, 217, 148\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"171,194,122\" /> \r\n    </ChartAreas>\r\n    <Legends>\r\n        <Legend _Template_=\"All\" BackColor=\"229, 217, 148\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" IsTextAutoFit=\"False\" /> \r\n    </Legends>\r\n   <BorderSkin SkinStyle=\"none\" /> \r\n   </Chart>";

        }


        public ActionResult TempChart(int? id)
        {
            //Code to fill the Temperature Chart with values from the database.

            float?[] Measurement = db.measurements.Where(i => i.equipments_facilities_Id == id).Where(x => x.equipments.Description == "Temperature Reactor").Select(x => x.ProcessValue).ToArray();
            DateTime?[] Date = db.measurements.Where(i => i.equipments_facilities_Id == id).Where(x => x.equipments.Description == "Temperature Reactor").Select(x => x.Recorded).ToArray();



            var myChart = new Chart(width: 1100, height: 350, theme: Theme.Green)


            .SetYAxis("Temp", 0, 50)

            .AddTitle("Reactor Core Temperature Chart")
            .AddSeries(
                chartType: "Line",
                name: "Measurements",
                xValue: Date,
                yValues: Measurement)

            .Write();

            return File(myChart.ToWebImage().GetBytes(), "image/jpeg");
        }

        public ActionResult ValueChart(int? id)
        {
            //Code to fill the optional Chart with values from the database.


            DateTime? From = Convert.ToDateTime(Request.Form["txtFrom"].ToString());
            DateTime? To = Convert.ToDateTime(Request.Form["txtTo"].ToString());
            String Tag = Convert.ToString(Request.Form["txtTags"].ToString());

            float?[] Measurement = db.measurements.Where(i => i.equipments_facilities_Id == id).Where(x => x.equipments.Tag == Tag).Where(x => x.Recorded > From && x.Recorded < To).Select(x => x.ProcessValue).ToArray();
            DateTime?[] Date = db.measurements.Where(i => i.equipments_facilities_Id == id).Where(x => x.equipments.Tag == Tag).Where(x => x.Recorded > From && x.Recorded < To).Select(x => x.Recorded).ToArray();

            var myChart = new Chart(width: 1100, height: 350, theme: Theme.Green)

            .SetYAxis("Value", 0, 50)
            .AddTitle(Tag)
            .AddSeries(
                chartType: "Line",
                name: "Measurements",
                xValue: Date,
                yValues: Measurement)

            .Write();


            return File(myChart.ToWebImage().GetBytes(), "image/jpeg");


        }
    }


}