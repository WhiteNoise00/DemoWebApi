using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Controllers
{
   
    public class DepartmentController : Controller
    {
        private TaskContext db;


        public DepartmentController(TaskContext context)
        {
            db = context;
        }

       [Route("Department/ViewDepartments")]
        public async Task<IActionResult> ViewDepartments(int page = 1)
        {
            IQueryable<Department> dep = db.Departments.Include(t => t.Positions);

            int pageSize = 3;
           
            var count = await dep.CountAsync();
            var items = await dep.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Departments = items
            };
            return View(viewModel);
        }

        
        [Route("Department/CreateDepartment")]
        public ActionResult CreateDepartment()
        {
            return View();
        }

        [Route("Department/CreateDepartment")]
        [HttpPost]
        public ActionResult CreateDepartment(Department dep)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(dep);
                db.SaveChanges();
                return RedirectToAction("ViewDepartments");
            }
            return View(dep);
        }

        [Route("Department/DepartmentDetails")]
        public async Task<IActionResult> DepartmentDetails(int? id)
        {
            if (id != null)
            {
                Department dep = await db.Departments.Include(e => e.Positions).FirstOrDefaultAsync(e => e.Id == id);
                if (dep != null)
                {
                    return View(dep);
                }
            }
            return NotFound();
        }

        [Route("Department/DepartmentEdit")]
        public async Task<IActionResult> DepartmentEdit(int? id)
        {
            if (id != null)
            {
                Department dep = await db.Departments.Include(e => e.Positions).FirstOrDefaultAsync(e => e.Id == id);                
                return View(dep);
            }
            return NotFound();
        }

        [Route("Department/DepartmentEdit")]
        [HttpPost]
        public async Task<IActionResult> DepartmentEdit(Department dep)
        {

            Department department = await db.Departments.Include(e => e.Positions).FirstOrDefaultAsync(e => e.Id == dep.Id);          
            department.Department_Name = dep.Department_Name;
            department.Department_Location = dep.Department_Location;

            if (ModelState.IsValid)
            {
                db.Departments.Update(department);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewDepartments");
            }

            else
            {                
                return View(department);
            }
        }

        
        [Route("Department/DepartmentDelete/{id?}")]
        [HttpGet]
        [ActionName("DepartmentDelete")]
        public async Task<IActionResult> DepartmentConfirmDelete(int? id)
        {
            if (id != null)
            {
                Department dep = await db.Departments.FirstOrDefaultAsync(p => p.Id == id);
                if (dep != null)
                    return View(dep);
            }
            return NotFound();
        }

        [Route("Department/DepartmentDelete/{id?}")]
        [HttpPost]
        public async Task<IActionResult> DepartmentDelete(int? id)
        {
            if (id != null)
            {
                Department dep = new Department { Id = id.Value };
                db.Entry(dep).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ViewDepartments");
            }
            return NotFound();
        }
    }
}
