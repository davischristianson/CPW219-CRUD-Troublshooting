using CPW219_CRUD_Troubleshooting.Models;
using Microsoft.AspNetCore.Mvc;

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

                TempData["Message"] = $"Student {studentModel.Name} Updated!";
                return RedirectToAction("Index");
            }
            //return view with errors
            return View(studentModel);
        }

        public IActionResult Delete(int id)
        {
            Student p = StudentDb.GetStudent(_context, id);
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            //Get Product from database
            Student p = StudentDb.GetStudent(_context, id);

            StudentDb.Delete(_context, p);

            return RedirectToAction("Index");
        }
    }
}
