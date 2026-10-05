using Microsoft.AspNetCore.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class InsuranceController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new InsuranceViewModel());
        }

        [HttpPost]
        public IActionResult Index(InsuranceViewModel model)
        {
            if (model.EngineCapacity <= 0 || model.DriverExperienceYears < 0)
            {
                model.ErrorMessage = "Об'єм двигуна повинен бути більше 0, а стаж водіння не може бути від'ємним.";
                return View(model);
            }

            decimal baseRate = 1000m;

            decimal engineCoef = 1.0m;
            if (model.EngineCapacity > 2000)
            {
                engineCoef = 1.6m;
            }
            else if (model.EngineCapacity > 1600)
            {
                engineCoef = 1.3m;
            }

            decimal cityCoef = model.CityType switch
            {
                "Kyiv" => 1.8m,
                "LargeCity" => 1.3m,
                _ => 1.0m
            };

            decimal totalPrice = baseRate * engineCoef * cityCoef;

            if (model.DriverExperienceYears > 3)
            {
                totalPrice *= 0.90m;
            }

            if (model.HasDiscountCategory)
            {
                totalPrice *= 0.80m;
            }

            model.CalculatedPrice = totalPrice;
            model.ErrorMessage = null;

            return View(model);
        }
    }
}
