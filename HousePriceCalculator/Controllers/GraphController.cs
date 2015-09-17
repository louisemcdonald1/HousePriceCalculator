using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HousePriceCalculator.Models;


namespace HousePriceCalculator.Controllers
{
    public class GraphController : Controller
    {
        public PartialViewResult _ShowGraph(louisemc_hpc2Entities _entities, int chosenYear, int priceEntered, int chosenBorough)
        {
            int startYear = 1995;
            int endYear = 2025;

            //get price change data from DB
            var priceChangeList = from priceChange in _entities.PriceChanges
                                  where priceChange.BoroughId == chosenBorough
                                  select priceChange;

            int priceArrayLength = endYear - startYear + 1;
            int[] priceChangeYear = new int[priceArrayLength];
            decimal[] priceChangeData = new decimal[priceArrayLength];
            int i = 0;
            int priceYear = 1995;

            foreach (PriceChanx priceChange in priceChangeList)
            {
                priceChangeYear[i] = priceYear;
                priceChangeData[i] = (decimal)priceChange.Change;

                i++;
                priceYear++;
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

            for (int j = 1; j < priceArrayLength; j++)
            {
                if (priceChangeYear[j] > chosenYear)
                {
                    priceChangesActual[k] = priceChangesActual[k - 1] * priceChangeData[j];
                    priceChangesInt[k] = (int)priceChangesActual[k];

                    priceChangeYears[k] = chosenYear + k;
                    k++;
                }
            }

            bool graphDataReady = true;

            ViewBag.GraphNumbers = priceChangesInt;
            ViewBag.GraphYears = priceChangeYears;
            ViewBag.GraphDataReady = graphDataReady;

            return PartialView();
        }
    }
}