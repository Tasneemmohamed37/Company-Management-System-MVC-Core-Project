using Company.DAL.Models;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Company.PL.Controllers
{
    [Authorize("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        #region Index

        // /Role/Index
        public async Task<IActionResult> Index(string SearchValue)
        {
            List<IdentityRole> roles = new List<IdentityRole>();

            if (string.IsNullOrEmpty(SearchValue))
                roles.AddRange(_roleManager.Roles.ToList());

            else
            {
                var role = await _roleManager.FindByNameAsync(SearchValue); // RoleName
                roles.Add(role);

            }

            var mappedroles = _mapper.Map<List<IdentityRole>, List<RoleViewModel>>(roles);
            return View(mappedroles);


        }

        #endregion

        #region Create

        // /Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(role);

                await _roleManager.CreateAsync(mappedRole);

                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        #endregion

        #region Details

        // /Role/Details/1
        // /Role/Details
        [HttpGet]
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);


            if (role is null)
                return NotFound();

            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);

            return View(viewName, mappedRole);
        }

        #endregion

        #region Edit

        // /Role/Edit/1
        // /Role/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel updatedRole)
        {
            if (id != updatedRole.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);

                    role.Name = updatedRole.RoleName;


                    await _roleManager.UpdateAsync(role);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1. Log Exception
                    // 2. Friendly Message

                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(updatedRole);
        }

        #endregion

        #region Delete

        // /Role/Delete/1
        // /Role/Delete
        //[HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel deletedRole)
        {
            if (id != deletedRole.Id)
                return BadRequest();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Friendly Message

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(deletedRole);
            }
        } 
        #endregion

    }
}
