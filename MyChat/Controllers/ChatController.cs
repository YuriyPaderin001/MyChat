using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MyChat.Models;
using MyChat.Services;

namespace MyChat.Controllers
{
    [ApiController]
    [Route("api/chats")]
    public class ChatController : Controller
    {
        private ApplicationDataService dataService;

        public ChatController(ApplicationDataService dataService)
        {
            this.dataService = dataService;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<Chat> Get()
        {
            string login = User.Identity.Name;
            User user = dataService.Users.GetUserByLogin(login);

            return dataService.Chats.GetUserChats(user);
        }

        [Authorize]
        [HttpGet("{id}")]
        public Chat Get(int id)
        {
            return dataService.Chats.GetChatById(id);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(Chat chat)
        {
            string login = User.Identity.Name;
            User user = dataService.Users.GetUserByLogin(login);

            chat.CreatorId = user.Id;
            
            if (ModelState.IsValid)
            {
                int generatedId = dataService.Chats.Add(chat);
                return Ok(generatedId);
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put(Chat chat)
        {
            if (ModelState.IsValid)
            {
                dataService.Chats.Update(chat);
                return Ok(chat);
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int removedRowsCount = dataService.Chats.RemoveById(id);
            
            return Ok(removedRowsCount);
        }
    }
}
