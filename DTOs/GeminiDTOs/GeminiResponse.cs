using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.GeminiDTOs
{
    public class GeminiResponse
    {
        public List<Candidate> candidates { get; set; } = [];
    }

    public class Candidate
    {
        public GeminiContent content { get; set; } = new();
    }

    public class GeminiContent
    {
        public List<GeminiPart> parts { get; set; } = [];
    }

    public class GeminiPart
    {
        public string text { get; set; } = string.Empty;
    }
}
