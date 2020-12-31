using CarApp.Models;
using System.Linq;
using System.Web.Mvc;

namespace CarApp.Controllers
{
    public class CarController : Controller
    {
        private CarAppEntities carAppEntities;

        public CarController() : this(new CarAppEntities())
        {
        }

        public CarController(CarAppEntities carAppEntities)
        {
            this.carAppEntities = carAppEntities;
        }

        // GET: Car
        public ActionResult Index()
        {
            return View(carAppEntities);
        }

        public ActionResult GetCarByYear(int yearMade)
        {
            return View(carAppEntities.Cars.Where(x => x.YearMade.Year == yearMade));
        }

        public ActionResult GetCar(int id)
        {
            var result = carAppEntities.Cars.FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                return View("No car description for the car id");
            }
            else
            {
                return View(carAppEntities.Cars.FirstOrDefault(x => x.Id == id));
            }
            
        }

        // GET: /Car/AddCar
        public ActionResult AddCar()
        {
            return View();
        }

        // POST: /Car/AddCar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCar(Car model)
        {
            if (ModelState.IsValid)
            {
                var car = new Car { Name = model.Name, Color = model.Color, YearMade = model.YearMade };

                carAppEntities.Cars.Add(car);
                carAppEntities.SaveChanges();

                return RedirectToAction("Index", "Car");
            }

            return View(model);
        }
    }
}