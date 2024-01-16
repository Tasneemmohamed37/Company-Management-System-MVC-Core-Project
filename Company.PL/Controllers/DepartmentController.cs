using Company.BLL.Interfaces;
using Company.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace Company.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region Index

        // /Department/Index
        public async Task<IActionResult> Index(string? name)
        {
            List<Department> departments = new List<Department>();

            if (string.IsNullOrEmpty(name))
                departments = (List<Department>)await _unitOfWork.DepartmentRepository.GetAll();
            else
                departments = (List<Department>)_unitOfWork.DepartmentRepository.SearchDepartmentByName(name);

            return View(departments);
        }

        #endregion

        #region Create

        // /Department/Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) // Server-Side Validation
            {

                try
                {
                    await _unitOfWork.DepartmentRepository.Add(department);

                    int count = await _unitOfWork.Complete();

                    if (count > 0)
                        TempData["Message"] = "Department is Created Successfully";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            return View(department);
        }

        #endregion

        #region Details
        // /Department/Details/1
        // /Department/Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest(); // 400

            var department = await _unitOfWork.DepartmentRepository.GetByID(id.Value);
            if (department is null)
                return NotFound(); // 404

            return View(viewName, department);
        }

        #endregion


        #region Edit

        // /Department/Edit/1
        // /Department/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {
            if (id != department.Id)
                return BadRequest();


            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);

                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1. Log Exception
                    // 2. Show Friendly Message => /Home/Error

                    ModelState.AddModelError("", ex.Message);

                }

            }
            return View(department);
        }


        #endregion

        #region Delete

        // /Department/Delete/1
        // /Department/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            if (id != department.Id)
                return BadRequest();
            try
            {
                _unitOfWork.DepartmentRepository.Delete(department);

                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Show Friendly Message => /Home/Error

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }
        } 
        #endregion

    }
}
