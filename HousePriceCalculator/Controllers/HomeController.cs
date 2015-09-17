using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HousePriceCalculator.Models;

namespace HousePriceCalculator.Controllers
{
    public class HomeController : Controller
    {
        private louisemc_hpc2Entities _entities = new louisemc_hpc2Entities();
        
        public ActionResult Index()
        {
            int[] graphNumbers = { 0 };
            ViewBag.GraphNumbers = graphNumbers;
            bool graphDataReady = false;
            ViewBag.GraphDataReady = graphDataReady;
            
            //get years data for drop-down list
            int startingYear = 1995;
            int currentYear = DateTime.Now.Year;
            int length = currentYear - startingYear + 1;            //add 1 to make years range inclusive
            int[] yearsDropDown = new int[length];
            int yearToAdd = startingYear;

            for (int i = 0; i < length; i++)
            {
                yearsDropDown[i] = yearToAdd;
                yearToAdd++;
            }

            List<int> yearsDropDownList = yearsDropDown.ToList<int>();

            //get boroughs data for drop-down list
            var boroughList = from borough in _entities.Boroughs
                              select borough;

            //populate ViewModel
            var model = new BoroughYearVm
            {
                Borough = boroughList.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }),
                Year = yearsDropDownList.Select(a => new SelectListItem
                {
                    Text = a.ToString(),
                    Value = a.ToString()
                })
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(BoroughYearVm model, int priceValue)
        {
            int chosenYear = Convert.ToInt32(model.selectedYear);
            int priceEntered = priceValue;
            int chosenBorough = Convert.ToInt32(model.selectedBorough);

            //get years data for drop-down list
            int startingYear = 1995;
            int currentYear = DateTime.Now.Year;
            int length = currentYear - startingYear + 1;            //add 1 to make years range inclusive
            int[] yearsDropDown = new int[length];
            int yearToAdd = startingYear;

            for (int i = 0; i < length; i++)
            {
                yearsDropDown[i] = yearToAdd;
                yearToAdd++;
            }

            List<int> yearsDropDownList = yearsDropDown.ToList<int>();

            //get boroughs data for drop-down list
            var boroughList = from borough in _entities.Boroughs
                              select borough;

            //populate ViewModel
            var model1 = new BoroughYearVm
            {
                Borough = boroughList.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }),
                Year = yearsDropDownList.Select(a => new SelectListItem
                {
                    Text = a.ToString(),
                    Value = a.ToString()
                })
            };

            model1.selectedBorough = chosenBorough.ToString();
            model1.selectedYear = chosenYear.ToString();
            model1.price = priceEntered.ToString();

            BoroughYearVm model2 = new BoroughYearVm();
            model2 = CalculateGraphData(model1);

            bool graphDataReady = true;
            ViewBag.GraphDataReady = graphDataReady;

            return View(model2); 
        }

        public BoroughYearVm CalculateGraphData(BoroughYearVm model)
        {
            //get user input from ViewModel
            int chosenYear = Convert.ToInt32(model.selectedYear);
            int priceEntered = Convert.ToInt32(model.price);
            int chosenBorough = Convert.ToInt32(model.selectedBorough);

            int startYear = 1995;
            int endYear = 2025;

            //get price change data from DB
            var priceChangeList = from priceChange in _entities.PriceChanges
                                  where priceChange.BoroughId == chosenBorough
                                  select priceChange;

            int priceArrayLength = endYear - startYear;      //not an inclusive range because there is no change data for 1995
            int[] priceChangeYear = new int[priceArrayLength];
            decimal[] priceChangeData = new decimal[priceArrayLength];
            int i = 0;
            
            foreach (PriceChanx priceChange in priceChangeList)
            {
                priceChangeYear[i] = priceChange.Year;
                priceChangeData[i] = (decimal)priceChange.Change;

                i++;
            }


            //calculate data for graph
            int priceTimespan = endYear - chosenYear + 1;
            decimal[] priceChangesActual = new decimal[priceTimespan];
            int[] priceChangesInt = new int[priceTimespan];
            int[] priceChangeYears = new int[priceTimespan];

            priceChangesActual[0] = priceEntered;
            priceChangesInt[0] = (int)priceEntered;
            priceChangeYears[0] = chosenYear;

            int k = 1;

            for (int j = 0; j < priceArrayLength; j++)
            {
                if (priceChangeYear[j] > chosenYear)
                {
                    if(k < priceTimespan)
                    {
                        priceChangesActual[k] = priceChangesActual[k - 1] * priceChangeData[j];
                        priceChangesInt[k] = (int)priceChangesActual[k];

                        priceChangeYears[k] = priceChangeYear[j]; 
                        k++;
                    }
                }
            }
            //add data for graph to ViewModel
            model.graphNumbers = priceChangesInt.ToList();
            model.graphYears = priceChangeYears.ToList();

            return model;
        }

        public PartialViewResult _ShowGraph()
        {
            return PartialView();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}