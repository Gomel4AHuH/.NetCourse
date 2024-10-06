﻿using Microsoft.AspNetCore.Mvc;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        //private readonly Logger _logger;

        public EmployeeController(IEmployeeService service)
        {
            _employeeService = service;
            //_logger = logger;
        }
        
        /*
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

        [HttpPost]
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
        */
        #region Actions
        
        // GET: EmployeeController
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["IdSortParm"] = sortOrder == "id" ? "id" : "";
                ViewData["LastNameSortParm"] = sortOrder == "lastName" ? "lastName_desc" : "lastName";
                ViewData["FirstNameSortParm"] = sortOrder == "firstName" ? "firstName_desc" : "firstName";
                ViewData["MiddleNameSortParm"] = sortOrder == "middleName" ? "middleName_desc" : "middleName";
                ViewData["BirthdaySortParm"] = sortOrder == "birthday" ? "birthday_desc" : "birthday";
                ViewData["SpecialitySortParm"] = sortOrder == "speciality" ? "speciality_desc" : "speciality";
                ViewData["EmploymentDateSortParm"] = sortOrder == "employmentDate" ? "employmentDate_desc" : "employmentDate";

                ViewData["CurrentFilter"] = searchString;

                List<Employee> employeeList = await _employeeService.GetAllAsync(sortOrder, searchString, pageNumber);
                if (employeeList.Count == 0)
                {
                    TempData["InfoMessage"] = "No employees available for now.";
                    //_logger.Add("No employees available for now.");                   
                }
                return View(employeeList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }           
        }

        // GET: EmployeeController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                await _employeeService.CreateAsync(employee);
                TempData["SuccessMessage"] = $"Employee created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Employee employee = await _employeeService.GetByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee details not available with the Id : " + id;
                    return RedirectToAction("Index");
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            try
            {
                await _employeeService.UpdateAsync(employee);
                TempData["SuccessMessage"] = $"Employee with id {employee.Id} updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: EmployeeController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Employee employee = await _employeeService.GetByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee details not available with the Id : " + id;
                    return RedirectToAction("Index");
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);
            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _employeeService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Employee with id {id} deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
        #endregion
    }
}
