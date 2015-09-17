using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HousePriceCalculator.Models
{
    public class BoroughYearVm
    {
        public IEnumerable<SelectListItem> Borough { get; set; }
        public string selectedBorough { get; set; }

        public IEnumerable<SelectListItem> Year { get; set; }
        public string selectedYear { get; set; }

        public List<int> graphNumbers = new List<int>();
        public List<int> graphYears = new List<int>();

        public string price;
    }
}