using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using MyChat.Models;
using MyChat.Services;

namespace MyChat.Controllers
{
    [ApiController]
    [Route("api/chats/{chatId:int}/messages")]
    public class ChatMessageController : Controller
    {
        private ApplicationDataService dataService;
        private IHubContext<ChatHubService> hubContext;

        public ChatMessageController(ApplicationDataService dataService, IHubContext<ChatHubService> hubContext)
        {
            this.dataService = dataService;
            this.hubContext = hubContext;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<Message> Get(int chatId)
        {
            return dataService.Messages.GetChatMessagesByChatId(chatId);
        }

        [Authorize]
        [HttpGet("{id}")]
        public Message Get(int chatId, int id)
        {
            return dataService.Messages.GetChatMessageByChatIdAndMessageId(chatId, id);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(int chatId, Message message)
        {
            string login = User.Identity.Name;
            User user = dataService.Users.GetUserByLogin(login);
            
            message.ChatId = chatId;
            message.SenderId = user.Id;
            message.SendingDateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                int generatedId = dataService.Messages.Add(message);
                await hubContext.Clients.All.SendAsync("receiveMessage", message);

                return Ok(generatedId);
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put(int chatId, Chat chat)
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
        public IActionResult Delete(int chatId, int id)
        {
            int removedRowsCount = dataService.Messages.RemoveById(id);
            return Ok(removedRowsCount);
        }
    }
}
