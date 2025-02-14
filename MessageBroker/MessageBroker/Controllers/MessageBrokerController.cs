using MessageBroker.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageBroker.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageBrokerController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageBrokerController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                LogService.Warning("Received an invalid message.");
                return BadRequest("Invalid message.");
            }

            try
            {
                await _messageService.AddMessage(message);
                LogService.Info($"Message sent successfully. Message: {message}");
                return Ok($"Message: {message}, sent successfully.");
            }
            catch(Exception ex)
            {
                LogService.Error($"Error while sending a message. Error: {ex.Message}");
                return StatusCode(500, $"An error occurred while attempting to publish the message. Message content: '{message}'. Please try again later.");
            }
        }


        [HttpGet("Receive")]
        public async Task<IActionResult> ReceiveMessage()
        {
            try
            {
                var message = await _messageService.GetMessage();

                if (string.IsNullOrWhiteSpace(message))
                {
                    LogService.Warning("No messages available to receive.");
                    return NotFound("No messages available to receive!");
                }

                LogService.Info($"Message received. Message: {message}");
                return Ok(message);
            }
            catch (Exception ex)
            {
                LogService.Error($"Error while receiving a message. Error: {ex.Message}");
                return StatusCode(500, "An error occurred while attempting to receive a message. Please try again later.");
            }
        }
    }
}