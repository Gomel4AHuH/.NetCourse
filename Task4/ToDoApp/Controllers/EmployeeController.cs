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

        #region API
        [HttpGet]
        [Route("GetAllEmployees")]
        public List<Employee> GetAll()
        {
            return [.. _employeeDbContext.Employees];
        }

        [HttpPost]
        [Route("AddEmployee")]
        public string AddEmployee(Employee employee)
        {
            _employeeDbContext.Employees.Add(employee);
            _employeeDbContext.SaveChanges();
            return "Employee added.";
        }

        [HttpGet]
        [Route("GetEmployee")]
        public Employee GetEmployee(int id)
        {
            return _employeeDbContext.Employees.Where(emp => emp.Id == id).FirstOrDefault();
        }

        [HttpGet]
        [Route("UpdateEmployee")]
        public string UpdateEmployee(Employee employee)
        {
            _employeeDbContext.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _employeeDbContext.SaveChanges();
            return "Employee updated.";
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public string DeleteEmployee(int id)
        {
            Employee employee = _employeeDbContext.Employees.Where(emp => emp.Id == id).FirstOrDefault();
            if (employee != null)
            {
                _employeeDbContext.Employees.Remove(employee);
                _employeeDbContext.SaveChanges();
                return "Employee deleted.";
            }
            else
            {
                return "Employee not found.";
            }

        }
        #endregion

        #region Actions
        // GET: EmployeeController
        public ActionResult Index()
        {
            List<Employee> employeeList = [.. _employeeDbContext.Employees];
            return View(employeeList);
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

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            Employee employee = _employeeDbContext.Employees.FirstOrDefault(emp => emp.Id == id);
            return View(employee);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
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
        #endregion

    }
}
