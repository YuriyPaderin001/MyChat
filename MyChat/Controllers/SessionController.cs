using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

using MyChat.Models;
using MyChat.Services;

namespace MyChat.Controllers
{
    [ApiController]
    [Route("api/sessions")]
    public class SessionController : Controller
    {
        private ApplicationDataService dataService;

        public SessionController(ApplicationDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginData loginData)
        {
            if (ModelState.IsValid)
            {
                User user = dataService.Users.GetUserByLoginData(loginData);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                    };

                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", 
                        ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                    user.Login = null;
                    user.Password = null;
                }

                return Ok(user);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}
