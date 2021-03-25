using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

using MyChat.Models;
using MyChat.Services;
using Microsoft.AspNetCore.Authorization;

namespace MyChat.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private ApplicationDataService dataService;

        public UserController(ApplicationDataService dataService)
        {
            this.dataService = dataService;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return dataService.Users.GetUsers();
        }

        [Authorize]
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User user = dataService.Users.GetUserById(id);
            user.Login = null;
            user.Password = null;

            return user;
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            if (ModelState.IsValid)
            {
                dataService.Users.Add(user);
                return Ok(user);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            if (ModelState.IsValid)
            {
                dataService.Users.Update(user);
                return Ok(user);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User user = dataService.Users.GetUserById(id);
            if (user != null)
            {
                dataService.Users.Remove(user);
            }

            return Ok(user);
        }
    }
}
