using MandobX.API.Data;
using MandobX.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace MandobX.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CitiesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            //try
            //{
                var Cities = _dbContext.Cities.Include(c => c.Regions).ToList();
                return View(Cities);
            //}
            //catch (System.Exception e)
            //{
            //    StreamWriter writer = new StreamWriter("C:\\inetpub\\wwwroot\\test\\Debugging.txt");
            //    writer.WriteLine(e.Message);
            //    if (e.InnerException!=null)
            //    {
            //        writer.WriteLine(e.InnerException.Message);
            //    }
            //    writer.Close();
            //    throw;
            //}
            
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(City city)
        {
            if (string.IsNullOrEmpty(city.Name))
            {
                return View();
            }
            _dbContext.Cities.Add(city);
            _dbContext.SaveChanges();
            return View("index", _dbContext.Cities.Include(c => c.Regions).ToList());
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("index", _dbContext.Cities.Include(c => c.Regions).ToList());
            }
            var city = _dbContext.Cities.Find(Id);
            if (city == null)
            {
                return View("index", _dbContext.Cities.Include(c => c.Regions).ToList());
            }
            return View(city);
        }
        [HttpPost]
        public IActionResult Edit(City city)
        {
            if (string.IsNullOrEmpty(city.Name))
            {
                return View(_dbContext.Cities.Find(city.Id));
            }
            _dbContext.Cities.Update(city);
            _dbContext.SaveChanges();
            return View("index", _dbContext.Cities.Include(c => c.Regions).ToList());
        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("index", _dbContext.Cities.Include(c => c.Regions).ToList());
            }
            var city = _dbContext.Cities.Find(Id);
            if (city == null)
            {
                return View("index", _dbContext.Cities.Include(c => c.Regions).ToList());

            }
            _dbContext.Cities.Remove(city);
            _dbContext.SaveChanges();
            return View("index", _dbContext.Cities.Include(c => c.Regions).ToList());

        }
    }
}
