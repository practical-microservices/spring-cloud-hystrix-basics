using BillboardClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace BillboardClient.Controllers
{
    public class MessageController : Controller
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [Route("/")]
        public IActionResult GetMessage(string billboardId)
        {
            return Json(_messageService.GetMessage(billboardId));
        }

        [Route("/slow")]
        public IActionResult Slow() => Json(_messageService.SlowOperation("test"));
    }
}