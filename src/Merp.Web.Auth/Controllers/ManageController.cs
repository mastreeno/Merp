using System;
using System.Threading.Tasks;
using Merp.Web.Auth.Models;
using Merp.Web.Auth.Models.Manage;
using Merp.Web.Auth.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Web.Auth.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ManageController : Controller
    {
        public ManageControllerWorkerServices WorkerServices { get; private set; }
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public ManageController(ManageControllerWorkerServices workerServices, UserManager<ApplicationUser> userManager)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager)); ;
        }

        #region patch
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(IdentityServerHost.Quickstart.UI.HomeController.Index), "Home");
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await UserManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await UserManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var viewModel = await WorkerServices.GetUserProfileAsync(User);
            return Ok(viewModel);
        }

        [HttpPut]
        public async Task<IActionResult> Profile([FromBody]ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await WorkerServices.SaveUserProfileAsync(model, User);
                return Ok();
            }
            catch (ApplicationException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationEmail()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await WorkerServices.SendVerificationEmailAsync(User, Url, Request.Scheme);
                return Ok();
            }
            catch (ApplicationException)
            {
                return BadRequest();
            }
        }
    }
}