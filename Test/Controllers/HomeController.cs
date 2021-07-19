using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;


namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private TaskContext db;
        public HomeController(TaskContext context)
        {
            db = context;
        }
          
        public async Task<IActionResult> Index()
        {           
            
            return View();
        }            

    }
}



