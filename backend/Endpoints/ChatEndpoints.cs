using ChatBot.Methods;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ChatBot.Endpoints
{
    public class ChatEndpoints
    {
        public void MapEndpoits(WebApplication app)
        {
            app.MapPost("/chat", async ([FromBody] ChatRequest request, HttpContext context, GeminiService gemini) =>
            {
                string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return await gemini.SendMessage(request, Guid.Parse(userId!));
            })
                .RequireAuthorization();
        }
    }
}
