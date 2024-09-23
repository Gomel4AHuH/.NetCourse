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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public List<Employee> GetAll()
        {
            return [.._employeeDbContext.Employees];
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
            return _employeeDbContext.Employees.Where(emp => emp.Id == id ).FirstOrDefault();
        }

        [HttpPut]
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

        /*[HttpGet]
        public IActionResult AddOrEdit(int Id = 0)
        {
            Employee employee = new Employee();

            if (Id == 0)
            {
                return View(employee);
            }
            else
            {
                return View(_employeeDbcontext.Employees.Find(Id));
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit(int Id = 0)
        {
            Employee employee = new Employee();

            if (Id == 0)
            {
                return View(employee);
            }
            else
            {
                return View(_employeeDbcontext.Employees.Find(Id));
            }
        }*/
    }
}
