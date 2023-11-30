using Lab5WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab5WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}