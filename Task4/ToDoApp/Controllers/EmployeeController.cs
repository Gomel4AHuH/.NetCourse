using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public EmployeeController(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            List<Employee> employeeList = [.. _employeeDbContext.Employees];
            return View(employeeList);
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public List<Employee> GetAll()
        {
            return [.. _employeeDbContext.Employees];
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            Employee employee = _employeeDbContext.Employees.FirstOrDefault(emp => emp.Id == id);
            return View(employee);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        //[Route("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            try
            {
                _employeeDbContext.Employees.Add(employee);
                _employeeDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Route("GetEmployee")]
        public Employee GetEmployee(int id)
        {
            return _employeeDbContext.Employees.Where(emp => emp.Id == id).FirstOrDefault();
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            Employee employee = _employeeDbContext.Employees.FirstOrDefault(emp => emp.Id == id);
            return View(employee);
        }


        // POST: EmployeeController/Edit/5
        [HttpPost]
        //[Route("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee employee)
        {
            try
            {
                _employeeDbContext.Employees.Add(employee);
                _employeeDbContext.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _employeeDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            Employee employee = _employeeDbContext.Employees.FirstOrDefault(emp => emp.Id == id);
            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        //[Route("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Employee employee = _employeeDbContext.Employees.FirstOrDefault(emp => emp.Id == id);
                if (employee != null)
                {
                    _employeeDbContext.Remove(employee);
                    _employeeDbContext.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
