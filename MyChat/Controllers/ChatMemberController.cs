using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using MyChat.Models;
using MyChat.Services;

namespace MyChat.Controllers
{
    [ApiController]
    [Route("api/chats/{chatId:int}/members")]
    public class ChatMemberController : Controller
    {
        private ApplicationDataService dataService;

        public ChatMemberController(ApplicationDataService dataService)
        {
            this.dataService = dataService;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<User> Get(int chatId)
        {
            return dataService.Users.GetChatMembersByChatId(chatId);
        }

        [Authorize]
        [HttpGet("{id}")]
        public User Get(int chatId, int id)
        {
            return dataService.Users.GetChatMemberByChatId(id);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(int chatId, User member)
        {

            if (ModelState.IsValid)
            {
                int generatedId = dataService.Chats.AddUserToChatByUserIdAndChatId(member.Id, chatId);
                return Ok(generatedId);
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int chatId, int id)
        {
            int removedRowsCount = dataService.Chats.RemoveUserFromChatByUserIdAndChatId(id, chatId);

            return Ok(removedRowsCount);
        }
    }
}
