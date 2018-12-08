using System;
using System.Threading.Tasks;
using Merp.Web.Auth.Models.Manage;
using Merp.Web.Auth.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Web.Auth.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ManageController : ControllerBase
    {
        public ManageControllerWorkerServices WorkerServices { get; private set; }

        public ManageController(ManageControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

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