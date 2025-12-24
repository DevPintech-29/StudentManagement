using Microsoft.AspNetCore.Mvc;
using StudentManagement.Data;
using StudentManagement.Models;

public class EmployeeController : Controller
{
    private readonly IGenericRepository<Employee> _repository;
    private readonly IGenericRepository<District> _districtRepo;
    private readonly IGenericRepository<Thana> _thanaRepo;
    private readonly IGenericRepository<Village> _villageRepo;
    private readonly IGenericRepository<Interest> _interestRepo;
    private readonly IWebHostEnvironment _env;

    public EmployeeController(
        IGenericRepository<Employee> repository,
        IGenericRepository<District> districtRepo,
        IGenericRepository<Thana> thanaRepo,
        IGenericRepository<Village> villageRepo,
        IGenericRepository<Interest> interestRepo,
        IWebHostEnvironment env)
    {
        _repository = repository;
        _districtRepo = districtRepo;
        _thanaRepo = thanaRepo;
        _villageRepo = villageRepo;
        _interestRepo = interestRepo; 
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _repository.GetAllAsync();
        var interests = await _interestRepo.GetAllAsync();

        foreach (var emp in employees)
        {
            if (!string.IsNullOrEmpty(emp.InterestNames))
            {
                var names = emp.InterestNames
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(n => n.Trim().ToLower())
                                .ToList();

                var matchedIds = interests
                                 .Where(i => names.Contains(i.Name.ToLower()))
                                 .Select(i => i.Id);

                emp.InterestIDs = string.Join(",", matchedIds);
            }
            else
            {
                emp.InterestIDs = "";
            }
        }
        return View(employees);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Districts = await _districtRepo.GetAllAsync();
        ViewBag.Thanas = new List<Thana>();
        ViewBag.Villages = new List<Village>();
        var model = new Employee();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee, IFormFile? ImageFile)
    {
        if (ImageFile != null && ImageFile.Length > 0)
        {
            var uploads = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }
            employee.ImagePath = "/images/" + fileName;
        }

        employee.Id = 0;
        await _repository.AddAsync(employee);
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<JsonResult> GetThanasByDistrict(int districtId)
    {
        var allThanas = await _thanaRepo.GetAllAsync();
        var result = allThanas.Where(t => t.DistrictId == districtId)
                           .Select(t => new { id = t.Id, name = t.Name })
                           .ToList();
        return Json(result);
    }

    [HttpGet]
    public async Task<JsonResult> GetVillagesByThana(int thanaId)
    {
        var allVillages = await _villageRepo.GetAllAsync();
        var result = allVillages.Where(v => v.ThanaId == thanaId)
                             .Select(v => new { id = v.Id, name = v.Name })
                             .ToList();
        return Json(result);
    }
    [HttpGet]
    public async Task<JsonResult> GetInterests()
    {
        var allInterests = await _interestRepo.GetAllAsync();
        var result = allInterests
            .Where(i => i.IsActive)
            .Select(i => new { id = i.Id, name = i.Name })
            .ToList();
        return Json(result);
    }


    [HttpPost]
    public async Task<IActionResult> SaveEmployee([FromForm] Employee employee)
    {
        try
        {
            if (employee == null)
                return Json(new { success = false, message = "No employee data!" });

            employee.Id = 0;

            if (employee.ImageFile != null && employee.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(employee.ImageFile.FileName);
                var filePath = Path.Combine("wwwroot/images/employees", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employee.ImageFile.CopyToAsync(stream);
                }

                employee.ImagePath = "/images/employees/" + fileName;
            }

            var props = typeof(Employee).GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(employee);
            }
            employee.ImageFile = null;

            await _repository.AddAsync(employee);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
                return Json(new { success = false, message = "Employee not found!" });

            await _repository.DeleteAsync(id);

            if (!string.IsNullOrEmpty(employee.ImagePath))
            {
                string filePath = employee.ImagePath;

                if (!string.IsNullOrEmpty(employee.ImagePath))
                {
                    filePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        employee.ImagePath.TrimStart('/', '\\')
                    );
                }
                filePath = Path.GetFullPath(filePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine($"Deleted file: {filePath}");
                }
                Console.WriteLine($"File not found: {filePath}");
            }
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }


    [HttpGet]
    public async Task<IActionResult> CreatePrac()
    {
        ViewBag.Districts = await _districtRepo.GetAllAsync();
        ViewBag.Thanas = new List<Thana>();
        ViewBag.Villages = new List<Village>();
        var model = new Employee();
        return View(model);
    }

}