using DemoProject.Data;
using DemoProject.Models;
using DemoProject.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DemoProject.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DemoDbContext demoDbContext;

        public EmployeesController(DemoDbContext demoDbContext)
        {
            this.demoDbContext = demoDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await demoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeViewModel)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeViewModel.Name,
                Email = addEmployeeViewModel.Email,
                Salary = addEmployeeViewModel.Salary,
                DateOfBirth = addEmployeeViewModel.DateOfBirth,
                Department = addEmployeeViewModel.Department
            };

            await demoDbContext.Employees.AddAsync(employee);
            await demoDbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await demoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };
              
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");   
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel updateEmployeeViewModel) 
        {
            var employee = await demoDbContext.Employees.FindAsync(updateEmployeeViewModel.Id);
            if (employee != null)
            {
                employee.Name = updateEmployeeViewModel.Name;
                employee.Email = updateEmployeeViewModel.Email;
                employee.Salary = updateEmployeeViewModel.Salary;
                employee.DateOfBirth = updateEmployeeViewModel.DateOfBirth;
                employee.Department = updateEmployeeViewModel.Department;

                await demoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel updateEmployeeViewModel) 
        {
            var employee = await demoDbContext.Employees.FindAsync(updateEmployeeViewModel.Id);
            if (employee != null) 
            {
                demoDbContext.Employees.Remove(employee);
                await demoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }













            
    }   
}
