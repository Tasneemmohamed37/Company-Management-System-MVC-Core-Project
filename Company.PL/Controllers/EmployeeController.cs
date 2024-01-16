using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.ViewModels;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    //[Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Index

        // /Employee/Index
        public async Task<IActionResult> Index(string? SearchValue)
        {
            IEnumerable<Employee> Emps;

            if (string.IsNullOrEmpty(SearchValue))
                Emps = await _unitOfWork.EmployeeRepository.GetAll();

            else
                Emps = _unitOfWork.EmployeeRepository.SearchEmployeeByName(SearchValue);

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Emps);
            return View(mappedEmps);

        }

        #endregion

        #region Create

        // /Employee/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = (List<Department>) await _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                try
                {
                    await _unitOfWork.EmployeeRepository.Add(mappedEmp);

                   int count = await _unitOfWork.Complete();
                    if (count > 0)
                        TempData["Message"] = "Employee is Created Successfully";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }


            }
            ViewBag.Departments = (List<Department>)await _unitOfWork.DepartmentRepository.GetAll();
            return View(employeeVM);
        }

        #endregion

        #region Details
        // /Employee/Details/1
        // /Employee/Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var Employee = await _unitOfWork.EmployeeRepository.GetByID(id.Value);


            if (Employee is null)
                return NotFound();

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);

            return View(viewName, mappedEmp);
        }
        #endregion

        #region Edit

        // /Employee/Edit/1
        // /Employee/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1. Log Exception
                    // 2. Friendly Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVM);
        }

        #endregion

        #region Delete

        // /Employee/Delete/1
        // /Employee/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                int count = await _unitOfWork.Complete();
                if (count > 0 && employeeVM.ImageName is not null)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Friendly Message

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        } 
        #endregion

    }
}
