using MandobX.API.Data;
using MandobX.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MandobX.Controllers
{
    public class CarBrandsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CarBrandsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var CarBrands = _dbContext.CarBrands.ToList();
            return View(CarBrands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CarBrand carBrand)
        {
            if (string.IsNullOrEmpty(carBrand.Name))
            {
                return View();
            }
            _dbContext.CarBrands.Add(carBrand);
            _dbContext.SaveChanges();
            return View("index", _dbContext.CarBrands.ToList());
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("index", _dbContext.CarBrands.ToList());
            }
            var carBrand = _dbContext.CarBrands.Find(Id);
            if (carBrand == null)
            {
                return View("index", _dbContext.CarBrands.ToList());
            }
            return View(carBrand);
        }
        [HttpPost]
        public IActionResult Edit(CarBrand carBrand)
        {
            if (string.IsNullOrEmpty(carBrand.Name))
            {
                return View(_dbContext.CarBrands.Find(carBrand.Id));
            }
            _dbContext.CarBrands.Update(carBrand);
            _dbContext.SaveChanges();
            return View("index", _dbContext.CarBrands.ToList());
        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("index", _dbContext.CarBrands.ToList());
            }
            var carBrand = _dbContext.CarBrands.Find(Id);
            if (carBrand == null)
            {
                return View("index", _dbContext.CarBrands.ToList());

            }
            _dbContext.CarBrands.Remove(carBrand);
            _dbContext.SaveChanges();
            return View("index", _dbContext.CarBrands.ToList());

        }
    }
}
