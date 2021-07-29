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
    public class PositionController : Controller
    {

        private TaskContext db;
        public PositionController(TaskContext context)
        {
            db = context;
        }


        [Route("Position/ViewPositions")]
        public async Task<IActionResult> ViewPositions(int page = 1)
        {          
            IQueryable<Position> pos = db.Positions.Include(t => t.Department);

            int pageSize = 3;

            var count = await pos.CountAsync();
            var items = await pos.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Positions = items
            };
            return View(viewModel);
        }

        [Route("Position/CreatePosition")]
        //Создание должности
        public async Task<IActionResult> CreatePosition()
        {
            var DepartmentsValue = await db.Departments.ToListAsync();           
            SelectList dep = new SelectList(DepartmentsValue, "Id", "Department_Name");
            ViewBag.DepartmentsList = dep;
            return View();
        }


        [Route("Position/CreatePosition")]
        [HttpPost]
        public ActionResult CreatePosition(Position pos, int depid)
        {
            if (ModelState.IsValid)
            {
                Department dep = db.Departments.FirstOrDefault(e => e.Id == depid);
                pos.Department = dep;
                db.Positions.Add(pos);
                db.SaveChanges();
                return RedirectToAction("ViewPositions");

            }
            return View();
        }

        [Route("Position/PositionDetails")]
        public async Task<IActionResult> PositionDetails(int? id)
        {
            if (id != null)
            {
                
                 Position  pos = await db.Positions.Include(d=>d.Employees).Include(d=>d.Department).FirstOrDefaultAsync(e => e.Id == id);
              
                if (pos!= null)
                {
                    return View(pos);
                }
            }
            return NotFound();
        }

        [Route("Position/PositionEdit")]
        public async Task<IActionResult> PositionEdit(int? id)
        {
            if (id != null)
            {
                Position pos = await db.Positions.FirstOrDefaultAsync(e => e.Id == id);
                return View(pos);
            }
            return NotFound();
        }

        [Route("Position/PositionEdit")]
        [HttpPost]
        public async Task<IActionResult> PositionEdit(Position pos)
        {

            Position position = await db.Positions.FirstOrDefaultAsync(p => p.Id == pos.Id);

            
            position.Position_Name = pos.Position_Name;          

            if (ModelState.IsValid)
            {
                db.Positions.Update(position);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewPositions");
            }

            else
            {
                return View(position);
            }
        }
        
        [Route("Position/PositionDelete/{id?}")]
        [HttpGet]
        [ActionName("PositionDelete")]
        public async Task<IActionResult> PositionConfirmDelete(int? id)
        {
            if (id != null)
            {
                Position pos = await db.Positions.FirstOrDefaultAsync(p => p.Id == id);
                if (pos != null)
                    return View(pos);
            }
            return NotFound();
        }

        [Route("Position/PositionDelete/{id?}")]
        [HttpPost]
        public async Task<IActionResult> PositionDelete(int? id)
        {
            if (id != null)
            {
                Position pos = new Position { Id = id.Value };
                db.Entry(pos).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ViewPositions");
            }
            return NotFound();
        }
    }
}
