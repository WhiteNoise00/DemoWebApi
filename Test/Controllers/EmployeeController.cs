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
  
    public class EmployeeController : Controller
    {

        private TaskContext db;
        public EmployeeController(TaskContext context)
        {
            db = context;
        }
        
        [Route("Employee/ViewEmployees")]
        public async Task<IActionResult> ViewEmployees(string name, int? id, int page=1)
        {                      
            IQueryable<Employee> em = db.Employees.Include(t => t.Tasks).Include(t => t.Position);

            int pageSize = 3;

            if (id != null && id != 0)
            {
                em = em.Where(e => e.Tasks.Any(t => t.Id == id));
            }

            if (!String.IsNullOrEmpty(name))
            {
                em = em.Where(x => EF.Functions.Like(x.Employee_Last_Name, $"%{name}%"));
            }

            var tasksValue = await db.Tasks.ToListAsync();
            tasksValue.Insert(0, new Models.Task { Task_Name = "Все", Id = 0 });
            SelectList tasks = new SelectList(tasksValue, "Id", "Task_Name");
            ViewBag.TasksList = tasks;            

            //Постраничный вывод
            var count = await em.CountAsync();
            var items = await em.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Employees = items
            };
            return View(viewModel);

        }

        [Route("Employee/CreateEmployee")]
        public async Task<IActionResult> CreateEmployee()
        {
            var positionsValue = await db.Positions.ToListAsync();
            var DepartmentsValue = await db.Departments.ToListAsync();
            if (positionsValue != null)
            {
                List<Position> pos = await db.Positions.ToListAsync();
                SelectList tasks = new SelectList(pos, "Id", "Position_Name");                
                ViewBag.PositionsList = tasks;
                return View();
            }
            else
            {
                return NotFound();
            }

        }

        [Route("Employee/CreateEmployee")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee em, int posid)
        {
            Position pos = await db.Positions.FirstOrDefaultAsync(p => p.Id == posid);
            

            if (ModelState.IsValid)
            {
                //Position pos = await db.Positions.FirstOrDefaultAsync(p => p.Id == posid);
                em.Position = pos;
                db.Employees.Add(em);
                db.SaveChanges();
                return RedirectToAction("ViewEmployees");
            }

            return View();
        }

        [Route("Employee/EmployeeDetails/{id?}")]
        public async Task<IActionResult> EmployeeDetails(int? id)
        {
            if (id != null)
            {
                Employee em = await db.Employees.Include(e => e.Tasks).FirstOrDefaultAsync(e => e.Id == id);
                if (em != null)
                {
                    return View(em);
                }
            }
            return NotFound();
        }

        
        [HttpGet]
        [Route("Employee/EmployeeEdit")]
        public async Task<IActionResult> EmployeeEdit(int? id)
        {
            if (id != null)
            {
                Employee em = await db.Employees.Include(e => e.Tasks).FirstOrDefaultAsync(e => e.Id == id);
                List<Position> pos = await db.Positions.ToListAsync();
                SelectList tasks = new SelectList(pos, "Id", "Position_Name");
                ViewBag.PositionsList = tasks;
                ViewBag.Tasks = db.Tasks.ToList();
                return View(em);
            }
            return NotFound();
        }


        [Route("Employee/EmployeeEdit")]
        [HttpPost]
        public async Task<IActionResult> EmployeeEdit(Employee em, int[] selectedTasks)
        {

            Employee employee = await db.Employees.Include(e => e.Tasks).FirstOrDefaultAsync(e => e.Id == em.Id);

            employee.Employee_Last_Name = em.Employee_Last_Name;
            employee.Employee_Name = em.Employee_Name;
            employee.Employee_Email = em.Employee_Email;
            employee.Employee_Middle_Name = em.Employee_Middle_Name;
            employee.Employee_Phone = em.Employee_Phone;
            
            employee.Tasks.Clear();

            if (selectedTasks != null)
            {
                foreach (var c in db.Tasks.Where(co => selectedTasks.Contains(co.Id)))
                {
                    employee.Tasks.Add(c);
                }
            }

            if (ModelState.IsValid)
            {
                db.Employees.Update(employee);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewEmployees");
            }

            else
            {
                ViewBag.Tasks = db.Tasks.ToList();
                return View(employee);
            }
        }

     
        [Route("Employee/EmployeeDelete/{id?}")]
        [HttpGet]
        public async Task<IActionResult> EmployeeConfirmDelete(int? id)
        {
            if (id != null)
            {
                Employee em = await db.Employees.FirstOrDefaultAsync(p => p.Id == id);
                if (em != null)
                    return View(em);
            }
            return NotFound();
        }

        [Route("Employee/EmployeeDelete/{id?}")]
        [HttpPost]
        public async Task<IActionResult> EmployeeDelete(int? id)
        {
            if (id != null)
            {
                Employee em = new Employee { Id = id.Value };
                db.Entry(em).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ViewEmployees");
            }
            return NotFound();
        }
    }
}
