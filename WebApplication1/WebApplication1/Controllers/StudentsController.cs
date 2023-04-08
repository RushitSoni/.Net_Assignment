using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly MVCDBContext _context;

        public StudentsController(MVCDBContext context)
        {
            _context = context;
        }


        private void PopulateDeptDropDownList(object? selectedDepts = null)
        {
            var deptsQuery = from s in _context.Departments
                                orderby s.Name
                                select new { DeptId = s.ID, s.Name };

            ViewBag.DeptId = new SelectList(deptsQuery.AsNoTracking(), "DeptId", "Name", selectedDepts);

        }


        // GET: Students

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;


            var dbContext_Demo = _context.Students.Include(s => s.department);
            var students = from s in dbContext_Demo
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.department.Name.Contains(searchString));
                                       
            }
            
            
            return View(await students.AsNoTracking().ToListAsync() );
             
        }
       /*public async Task<IActionResult> Index()
        {

            var dbContext_Demo = _context.Students.Include(s => s.department);
            return View(await dbContext_Demo.ToListAsync());

        }*/

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var dbContext_Demo = _context.Students.Include(s => s.department);
            if (id == null || dbContext_Demo == null)
            {
                return NotFound();
            }

            var student = await dbContext_Demo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "DeptName");
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {

            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "DeptName");
            PopulateDeptDropDownList();
            return View();

        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Class,DeptId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "DeptName", student.DeptId);

            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "DeptName", student.DeptId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Class,DeptID")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "DeptName", student.DeptId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var dbContext_Demo = _context.Students.Include(s => s.department);
            if (id == null || dbContext_Demo == null)
            {
                return NotFound();
            }

            var student = await dbContext_Demo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
           // ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "DeptName");
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbContext_Demo = _context.Students.Include(s => s.department);
            if (dbContext_Demo == null)
            {
                return Problem("Entity set 'MVCDBContext.Students'  is null.");
            }
            var student = await dbContext_Demo
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
