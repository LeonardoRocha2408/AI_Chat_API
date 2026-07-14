using ChatBot.Entities;
using DTOs;
using DTOs.GeminiDTOs;
using EnumShared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using static DTOs.GeminiDTOs.GeminiRequest;
namespace ChatBot.Methods
{
    public class GeminiService
    {
        private readonly string _apikey;
        private readonly HttpClient _httpClient;
        private readonly DbEntity _context;

        public GeminiService(
            IConfiguration configuration,
            HttpClient httpClient,
            DbEntity context)
        {
            _apikey = configuration["Gemini:APIkey"]
                ?? throw new Exception("The API key Gemini not found");

            _httpClient = httpClient;

            _context = context;
        }

        public async Task<ChatResponse> SendMessage(ChatRequest dto, Guid userId)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite:generateContent?key={_apikey}";

            var request = new GeminiRequest()
            {
                systemInstruction = new SystemInstruction
                {
                    parts =
                    [
                        new Part
                        {
                            text = """
                            You are a assistent. 
                            Always answer following the model:

                            {
                                "title": "short title (maximum 5 words",
                                "content": "your answer for user"
                            }

                            Rules:
                            - Return only JSON
                            - Don't use markdown
                            """
                        }
                    ]
                },
            };

            var messages = await _context.Messages
                .Where(m => m.ConversationId == dto.ConversationId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(5)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            foreach (var message in messages)
            {
                request.contents.Add(new Content
                {
                    role = message.Role == Role.User ? "user" :"model",
                    parts =
                    [
                        new Part
                        {
                            text = message.Content
                        }
                    ]
                });
            }

            var currentMessage =
                    new Content
                    {
                        role = "user",
                        parts =
                        [
                            new Part
                            {
                                text = dto.Content
                            }
                        ]
                    };

            request.contents.Add(currentMessage);

            string json = JsonSerializer.Serialize(request);
            
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var responseApi = await _httpClient.PostAsync(url, content);
            var contentBody = await responseApi.Content.ReadAsStringAsync();


            if (!responseApi.IsSuccessStatusCode)
            {
                throw new Exception(contentBody);
            }

            GeminiResponse? response = JsonSerializer.Deserialize<GeminiResponse>(contentBody);

            if (response is null)
            {
                throw new Exception("Failed to deserialize Gemini response");
            }

            if (response.candidates.Count == 0)
            {
                throw new Exception("Gemini returned no candidates.");
            }
            var jsonResponse =
                response.candidates[0]
                .content
                .parts[0]
                .text;

            
            ChatResponse? chatResponse = JsonSerializer.Deserialize<ChatResponse>(jsonResponse);

            if (chatResponse is null)
            {
                throw new Exception("Failed to deserialize chat response");
            }

            string? UserName = await _context.Users
                .Where(u => u.Id == userId).Select(u => u.UserName).SingleOrDefaultAsync();

            if (UserName is null)
            {
                    throw new Exception("Failed to search user on database");
            }

            ConversationEntity conversation;

            if(dto.ConversationId == Guid.Empty)
            {
                conversation = new ConversationEntity()
                {
                    Id = Guid.NewGuid(),
                    Title = chatResponse.title,
                    UserName = UserName,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Conversations.AddAsync(conversation);
            }
            else
            {
                conversation = await _context.Conversations
                    .SingleAsync(c => c.Id == dto.ConversationId);

            }

            var userMessages = new MessagesEntity()
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation.Id,
                Role = Role.User,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Messages.AddAsync(userMessages);


            var assistantMessages = new MessagesEntity()
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation.Id,
                Role = Role.Assistant,
                Content = chatResponse.content,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Messages.AddAsync(assistantMessages);

            await _context.SaveChangesAsync();

            chatResponse.ConversationId = conversation.Id;
            chatResponse.UserMessageId = userMessages.Id;
            chatResponse.AssistantMessageId = assistantMessages.Id;
            return chatResponse;
        }
    }
}
