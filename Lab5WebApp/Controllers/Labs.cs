using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ClassLibraryLab5;

namespace Lab5WebApp.Controllers
{
    public class Labs : Controller
    {
        [Authorize]
        public IActionResult Lab1()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Lab1(string inputString)
        {
            ViewBag.Input = inputString;
            List<string> result = ClassLab1.Execute(inputString);
            ViewBag.Result = result;
            return View();
        }

        [Authorize]
        public IActionResult Lab2()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Lab2(string inputString)
        {
            ViewBag.Input = inputString;
            string result = ClassLab2.Execute(inputString).ToString();
            ViewBag.Result = result;
            return View();
        }

        [Authorize]
        public IActionResult Lab3()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Lab3(string inputString)
        {
            ViewBag.Input = inputString;
            string result = ClassLab3.Execute(inputString);
            ViewBag.Result = result;
            return View();
        }
    }
}
