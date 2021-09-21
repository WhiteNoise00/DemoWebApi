using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Controllers
{
   
    public class TaskController : Controller
    {

        private TaskContext db;
        public TaskController(TaskContext context)
        {
            db = context;
        }

        [Route("Task/ViewTask")]
        public async Task<IActionResult> ViewTasks(int page = 1)
        {

            int pageSize = 3;
            IQueryable<Models.Task> source = db.Tasks;
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Tasks = items
            };
            return View(viewModel);

        }

        [Route("Task/CreateTask")]
        public IActionResult CreateTask()
        {
            return View();
        }

        [Route("Task/CreateTask")]
        [HttpPost]
        public async Task<IActionResult> CreateTask(Models.Task ts)
        {
            if (ModelState.IsValid)
            {

                db.Tasks.Add(ts);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewTasks");
            }

            return View();
        }

        public async Task<IActionResult> TaskDetails(int? id)
        {
            if (id != null)
            {
                Models.Task ts = await db.Tasks.Include(e => e.Employees).FirstOrDefaultAsync(e => e.Id == id);
                if (ts != null)
                    return View(ts);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("Task/TaskEdit")]
        public async Task<IActionResult> TaskEdit(int? id)
        {
            if (id != null)
            {
                Models.Task ts = await db.Tasks.Include(t => t.Employees).FirstOrDefaultAsync(t => t.Id == id);
                ViewBag.Employees = db.Employees.ToList();
                if (ts != null)
                {
                    return View(ts);
                }
            }
            return NotFound();
        }

        [Route("Task/TaskEdit")]       
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

            if (ModelState.IsValid)
            {
                db.Tasks.Update(task);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewTasks");
            }
            else
            {
                ViewBag.Employees = db.Employees.ToList();
                return View(task);
            }

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
                return RedirectToAction("ViewTasks");
            }
            return NotFound();
        }


    }
}
