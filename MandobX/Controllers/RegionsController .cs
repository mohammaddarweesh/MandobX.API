using MandobX.API.Data;
using MandobX.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MandobX.Controllers
{
    public class RegionsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public RegionsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var Regions = _dbContext.Regions.Include(r=>r.City).ToList();
            return View(Regions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var cities = _dbContext.Cities.ToList();
            ViewBag.Cities = cities;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Region region)
        {
            if (string.IsNullOrEmpty(region.Name)|| string.IsNullOrEmpty(region.CityId.ToString()))
            {
                return View();
            }
            _dbContext.Regions.Add(region);
            _dbContext.SaveChanges();
            return View("index", _dbContext.Regions.Include(r => r.City).ToList());
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("index", _dbContext.Regions.ToList());
            }
            var region = _dbContext.Regions.Find(Id);
            if (region == null)
            {
                return View("index", _dbContext.Regions.Include(r => r.City).ToList());
            }
            var cities = _dbContext.Cities.ToList();
            ViewBag.Cities = cities;
            return View(region);
        }
        [HttpPost]
        public IActionResult Edit(Region region)
        {
            if (string.IsNullOrEmpty(region.Name))
            {
                return View(_dbContext.Regions.Find(region.Id));
            }
            _dbContext.Regions.Update(region);
            _dbContext.SaveChanges();
            return View("index", _dbContext.Regions.Include(r => r.City).ToList());
        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("index", _dbContext.Regions.ToList());
            }
            var region = _dbContext.Regions.Find(Id);
            if (region == null)
            {
                return View("index", _dbContext.Regions.Include(r => r.City).ToList());

            }
            _dbContext.Regions.Remove(region);
            _dbContext.SaveChanges();
            return View("index", _dbContext.Regions.Include(r => r.City).ToList());

        }
    }
}
