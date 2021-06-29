using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
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
            return View(await db.Tasks.ToListAsync());
        }

        public async Task<IActionResult> ViewEmployees(string name, int? id)
        {
            IQueryable<Employee> em = db.Employees.Include(p => p.Tasks);

            if (id != null && id != 0)
            {
                em = em.Where(e => e.Tasks.Any(t => t.Id == id));
            }

            if (!String.IsNullOrEmpty(name))
            {
                em = em.Where(x => EF.Functions.Like(x.Employee_Last_Name, $"%{name}%"));
            }

            SelectList tasks = new SelectList(db.Tasks, "Id", "Task_Name");
            ViewBag.TasksList = tasks;
            return View(em);
        }

        public IActionResult CreateTask()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Models.Task ts)
        {
            if (ModelState.IsValid)
            {

                db.Tasks.Add(ts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult CreateEmployee()
        {
            ViewBag.Tasks = db.Tasks.ToList();
            return View();
        }

        [HttpPost]
        public  ActionResult CreateEmployee(Employee em)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(em);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();            
        }

        public async Task<IActionResult> TaskDetails(int? id)
        {
            if (id != null)
            {
                Models.Task ts =  await db.Tasks.Include(e => e.Employees).FirstOrDefaultAsync(e => e.Id == id);
                if (ts != null)
                return View(ts);
            }
            return NotFound();            
        }

        public async Task<IActionResult> EmployeeDetails(int? id)
        {           
           Employee em = await db.Employees.Include(e => e.Tasks).FirstOrDefaultAsync(e => e.Id == id);
            if (em == null) { return NotFound(); } 
            return View(em);
        }

        public async Task<IActionResult> TaskEdit(int? id)
        {           
            if (id != null)
            {
                
                Models.Task ts = await db.Tasks.Include(t => t.Employees).FirstOrDefaultAsync(t => t.Id == id);
                ViewBag.Employees = db.Employees.ToList();
                if (ts != null)
                    return View(ts);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> TaskEdit(Models.Task ts, int[] selectedEmployees) 
        {
            Models.Task task = await db.Tasks.Include(t => t.Employees).FirstOrDefaultAsync(t => t.Id == ts.Id);
           
            task.Task_Name = ts.Task_Name;
            task.Task_Priority = ts.Task_Priority;
            task.Task_Description = ts.Task_Description;
            task.Task_Beginning_Date = ts.Task_Beginning_Date;
            task.Task_Ending_Date = ts.Task_Ending_Date;
            task.Employees.Clear();

            if (selectedEmployees != null)
            {
                foreach (var c in db.Employees.Where(co => selectedEmployees.Contains(co.Id)))
                {
                    task.Employees.Add(c);
                }
            }

            db.Tasks.Update(task);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");        

        }

        [HttpGet]
        [ActionName("TaskDelete")]
        public async Task<IActionResult> TaskConfirmDelete(int? id)
        {
            if (id != null)
            {
                Models.Task ts = await db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
                if (ts != null)
                return View(ts);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> TaskDelete(int? id)
        {
            if (id != null)
            {
                Models.Task ts = new Models.Task { Id = id.Value };
                db.Entry(ts).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> EmployeeEdit(int? id)
        {
            if (id != null)
            {
                Employee em = await db.Employees.Include(e => e.Tasks).FirstOrDefaultAsync(e => e.Id == id);                
                ViewBag.Tasks = db.Tasks.ToList();
                return View(em);
            }
            return NotFound();
        }
   
        [HttpPost]
        public async Task<IActionResult> EmployeeEdit(Employee em, int[] selectedTasks)
        {
           
            Employee employee = await db.Employees.Include(e => e.Tasks).FirstOrDefaultAsync(e => e.Id == em.Id);

            employee.Employee_Last_Name = em.Employee_Last_Name;
            employee.Employee_Name = em.Employee_Name;
            employee.Employee_Email = em.Employee_Email;
            employee.Employee_Middle_Name = em.Employee_Middle_Name;
            employee.Tasks.Clear();

            if (selectedTasks!=null)
            {
                foreach (var c in db.Tasks.Where( co=> selectedTasks.Contains(co.Id)))
                {
                    employee.Tasks.Add(c);
                }
            }
          
            db.Employees.Update(employee);            
            await db.SaveChangesAsync();          
            return RedirectToAction("ViewEmployees");       
        }

        [HttpGet]
        [ActionName("EmployeeDelete")]
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

        [HttpPost]
        public async Task<IActionResult> EmployeeDelete(int? id)
        {
            if (id != null)
            {
                Employee em = new Employee { Id = id.Value };
                db.Entry(em).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    } 
}



