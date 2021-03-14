using MandobX.API.Data;
using MandobX.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MandobX.Controllers
{
    public class CarTypesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CarTypesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var CarTypes = _dbContext.CarTypes.ToList();
            return View(CarTypes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CarType carType)
        {
            if (string.IsNullOrEmpty(carType.Name))
            {
                return View();
            }
            _dbContext.CarTypes.Add(carType);
            _dbContext.SaveChanges();
            return View("index", _dbContext.CarTypes.ToList());
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("index", _dbContext.CarTypes.ToList());
            }
            var carType = _dbContext.CarTypes.Find(Id);
            if (carType == null)
            {
                return View("index", _dbContext.CarTypes.ToList());
            }
            return View(carType);
        }
        [HttpPost]
        public IActionResult Edit(CarType carType)
        {
            if (string.IsNullOrEmpty(carType.Name))
            {
                return View(_dbContext.CarTypes.Find(carType.Id));
            }
            _dbContext.CarTypes.Update(carType);
            _dbContext.SaveChanges();
            return View("index", _dbContext.CarTypes.ToList());
        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("index", _dbContext.CarTypes.ToList());
            }
            var carType = _dbContext.CarTypes.Find(Id);
            if (carType == null)
            {
                return View("index", _dbContext.CarTypes.ToList());

            }
            _dbContext.CarTypes.Remove(carType);
            _dbContext.SaveChanges();
            return View("index", _dbContext.CarTypes.ToList());

        }
    }
}
