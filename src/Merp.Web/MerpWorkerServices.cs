using Merp.Web.Site.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web
{
    public abstract class MerpWorkerServices
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public MerpWorkerServices(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public Guid UserId
        {
            get
            {
                return Guid.Parse(userManager.GetUserId(httpContextAccessor.HttpContext.User));
            }
        }
    }
}
