using Merp.Web.Site.Models;
using Merp.Web.Auth.Models.Manage;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Merp.Web.Auth.Services;

namespace Merp.Web.Auth.WorkerServices
{
    public class ManageControllerWorkerServices
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IEmailSender _emailSender;

        public ManageControllerWorkerServices(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<ProfileViewModel> GetUserProfileAsync(ClaimsPrincipal userIdentity)
        {
            var user = await _userManager.GetUserAsync(userIdentity);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(userIdentity)}'.");
            }

            var model = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed
            };

            return model;
        }

        public async Task SaveUserProfileAsync(ProfileViewModel model, ClaimsPrincipal userIdentity)
        {
            var user = await _userManager.GetUserAsync(userIdentity);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(userIdentity)}'.");
            }

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }
        }

        public async Task SendVerificationEmailAsync(ClaimsPrincipal userIdentity, IUrlHelper urlHelper, string scheme)
        {
            var user = await _userManager.GetUserAsync(userIdentity);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(userIdentity)}'.");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = urlHelper.EmailConfirmationLink(user.Id, code, scheme);
            var email = user.Email;

            await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);
        }
    }
}
