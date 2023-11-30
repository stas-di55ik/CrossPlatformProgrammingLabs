using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ClassLibraryLab4;

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
        public void Lab1(string input_file, string output_file) => ClassLibraryLab4.ClassLab1.Execute(input_file, output_file);


        [Authorize]
        public IActionResult Lab2()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public void Lab2(string input_file, string output_file) => ClassLibraryLab4.ClassLab2.Execute(input_file, output_file);


        [Authorize]
        public IActionResult Lab3()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public void Lab3(string input_file, string output_file) => ClassLibraryLab4.ClassLab3.Execute(input_file, output_file);

    }
}
