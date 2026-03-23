using GiftRide.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GiftRide.Controllers
{
    public class ChatController : Controller
    {

        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Този метод ще се вика с AJAX
        [HttpPost]
        public async Task<IActionResult> GetResponse([FromForm] string message)
        {
            var answer = await _chatService.GetAnswerAsync(message);
            return Json(new { reply = answer });
        }       
    }
}
