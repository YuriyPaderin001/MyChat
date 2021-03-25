using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MyChat.Models;
using MyChat.Services;

namespace MyChat.Controllers

{
    [ApiController]
    [Route("api/messages/{messageId:int}/attachments")]
    public class MessageAttachmentController : Controller
    {
        private ApplicationDataService dataService;
        private IWebHostEnvironment appEnvironment;

        public MessageAttachmentController(ApplicationDataService dataService, IWebHostEnvironment appEnvironment)
        {
            this.dataService = dataService;
            this.appEnvironment = appEnvironment;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<Attachment> Get(int messageId)
        {
            return dataService.Attachments.GetMessageAttachmentsByMessageId(messageId);
        }

        [Authorize]
        [HttpGet("{id}")]
        public Attachment Get(int messageId, int id)
        {
            return dataService.Attachments.GetMessageAttachmentByMessageIdAndAttachmentId(messageId, id);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(int messageId, IFormFile file)
        {
            Console.WriteLine(file);
            
            int addedRowsCount = 0;
            string path;
            if (file.FileName.EndsWith(".png") || file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".jpeg"))
            {
                path = "/img/" + file.FileName;
            }
            else
            {
                path = "/doc/" + file.FileName;
            }

            FileStream fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create);
            try
            {
                await file.CopyToAsync(fileStream);
                if (ModelState.IsValid)
                {
                    Attachment attachment = new Attachment(0, messageId, path);
                    addedRowsCount += dataService.Attachments.Add(attachment);
                }

                return Ok(addedRowsCount);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                fileStream.Close();
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
