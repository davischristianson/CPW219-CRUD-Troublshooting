using CPW219_CRUD_Troubleshooting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace CPW219_CRUD_Troubleshooting.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext dbContext)
        {
            _context = dbContext;
        }

        public IActionResult Index()
        {
            List<Student> products = StudentDb.GetStudents(_context);
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student studentToCreate)
        {
            if (ModelState.IsValid)
            {
                StudentDb.Add(studentToCreate, _context);

                await _context.SaveChangesAsync();

                ViewData["Message"] = $"{studentToCreate.Name} was added!";
                return View();
            }

            //Show web page with errors
            return View(studentToCreate);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //get the product by id
            Student studentToEdit = StudentDb.GetStudent(_context, id);

            if(studentToEdit == null)
            {
                return NotFound();
            }

            //show it on web page
            return View(studentToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student studentModel)
        {
            if (ModelState.IsValid)
            {
                StudentDb.Update(_context, studentModel);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Student {studentModel.Name} was updated!"; 
                return RedirectToAction("Index");
            }
            //return view with errors
            return View(studentModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Student studentToDelete = StudentDb.GetStudent(_context, id);

            if(studentToDelete == null)
            {
                return NotFound();
            }

            return View(studentToDelete);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            //Get Product from database
            Student studentToDelete = StudentDb.GetStudent(_context, id);

            if(studentToDelete != null)
            {
                StudentDb.Delete(_context, studentToDelete);
                await _context.SaveChangesAsync();

                TempData["Message"] = studentToDelete.Name + " was deleted successfully!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
