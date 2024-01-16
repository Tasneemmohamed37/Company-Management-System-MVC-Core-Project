using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using AutoMapper;

namespace Company.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
			_userManager = userManager;
			_mapper = mapper;
		}

		#region Index

		// /User/Index
		[HttpGet]
		public async Task<IActionResult> Index(string SearchValue)
		{
			List<ApplicationUser> users = new List<ApplicationUser>();

			if (string.IsNullOrEmpty(SearchValue))
				users.AddRange(_userManager.Users.ToList());

			else
			{
				var user = await _userManager.FindByNameAsync(SearchValue); // UserName
				users.Add(user);

			}

			var mappedUsers = _mapper.Map<List<ApplicationUser>, List<UserViewModel>>(users);
			return View(mappedUsers);


		} 
		#endregion


		#region Details

		// /user/Details/1
		// /user/Details
		[HttpGet]
		public async Task<IActionResult> Details(string id, string viewName = "Details")
		{
			if (id is null)
				return BadRequest();
			var user = await _userManager.FindByIdAsync(id);


			if (user is null)
				return NotFound();

			var mappeduser = _mapper.Map<ApplicationUser, UserViewModel>(user);

			return View(viewName, mappeduser);
		} 
		#endregion


		#region Edit
		// /User/Edit/1
		// /User/Edit
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			return await Details(id, "Edit");

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel updatedUser)
		{
			if (id != updatedUser.Id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
					var user = await _userManager.FindByIdAsync(id);

					user.FName = updatedUser.FName;
					user.LName = updatedUser.LName;
					user.PhoneNumber = updatedUser.PhoneNumber;
					user.Email = updatedUser.Email;



					await _userManager.UpdateAsync(user);

					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					// 1. Log Exception
					// 2. Friendly Message

					ModelState.AddModelError(string.Empty, ex.Message);
				}

			}
			return View(updatedUser);
		}

		#endregion

		#region Delete

		// /user/Delete/1
		// /user/Delete
		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			return await Details(id, "Delete");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel deletedUser)
		{
			if (id != deletedUser.Id)
				return BadRequest();
			try
			{
				var user = await _userManager.FindByIdAsync(id);
				await _userManager.DeleteAsync(user);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// 1. Log Exception
				// 2. Friendly Message

				ModelState.AddModelError(string.Empty, ex.Message);
				return View(deletedUser);
			}
		} 
		#endregion
	}
}
