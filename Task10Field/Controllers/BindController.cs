using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task10Field.Data;
using Task10Field.Models;
using Task10Field.Models.Binding;

namespace Task10Field.Controllers
{
    public class BindController : Controller
    {
        public ApplicationDbContext Context;
        
        public BindController(ApplicationDbContext context) {
            Context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Country()
        { 
            var data=Context.Countries.Include(x=>x.States).ThenInclude(y=>y.Cities).ToList();
            return View(data);
        }
        [HttpPost]
        public IActionResult Country(IFormCollection form)
        {
            Country data = new Country()
            {     
                
                CountryName=form["CountryName"]
              
            };
            Context.Countries.Add(data);
            Context.SaveChanges();
            return RedirectToAction("Country");
        }
       
        [HttpPost]
        public IActionResult State(IFormCollection form)
        {
            State model = new State()
            {
                CountryId = int.Parse(form["CountryId"]),
                StateName = form["StateName"]

            };
            Context.States.Add(model);
            Context.SaveChanges();
            return RedirectToAction("Country");
        }
        [HttpPost]
        public IActionResult City(IFormCollection form)
        {
            City model = new City()
            {
                StateId =int.Parse( form["StateId"]),
                CityName = form["CityName"]
            };
            Context.Cities.Add(model);
            Context.SaveChanges();
            return RedirectToAction("Country");
        }

        [HttpGet]
        public IActionResult FilterData()
        {
            var data = Context.Countries.Include(x => x.States).ThenInclude(y => y.Cities).ToList();
            return View(data);
        }
        [HttpGet]
        public IActionResult GetStates(int countryId)
        {
            var states = Context.States.Where(s => s.CountryId == countryId)
                .Select(s => new { s.StateId, s.StateName })
                .ToList();
            return Json(states);
        }

        [HttpGet]
        public IActionResult GetCities(int stateId)
        {
            var cities = Context.Cities
                .Where(c => c.StateId == stateId)
                .Select(c => new { c.CityId, c.CityName })
                .ToList();
            return Json(cities);
        }
        [HttpPost]
        public IActionResult Details(IFormCollection form)
        {
            StudentData model = new StudentData()
            {
                Name = form["Name"],
                Mobile = form["Mobile"],
                countries = form["countryName"],
                states = form["stateName"],
                cities = form["cityName"]
            };

            Context.StudentData.Add(model);
            Context.SaveChanges();
                return RedirectToAction("ShowDetails");
        }
        [HttpGet]
        public IActionResult ShowDetails()
        {
            var data = Context.StudentData.ToList();
            return View(data);
        }
        public IActionResult RatesForCommunity()
        {
            var data = from c in Context.StudentData join d in Context.Employee on c.Id equals d.Id
                      select new InnerViewModel
                      {
                          Id=c.Id,
                          Name=c.Name,
                          Designation=d.Designation,
                          countries=c.countries,
                          states = c.states,
                          cities = c.cities
                      };
            return View(data);
        }
   
   


    }

}
