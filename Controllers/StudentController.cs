using Microsoft.AspNetCore.Mvc;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly IGenericRepository<Student> _repository;

        public StudentController(IGenericRepository<Student> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _repository.GetAllAsync();
            return View(students);
        }

        /*public async Task<IActionResult> Details(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                return NotFound();
            return View(student);
        }*/

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "Student not found." });
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        studentId = student.StudentId,
                        name = student.Name,
                        age = student.Age,
                        email = student.Email
                    }
                });
            }
            return View(student);
        }



        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            await _repository.AddAsync(student);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            await _repository.UpdateAsync(student);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                return NotFound();
            return View(student);
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


     


    }
}
