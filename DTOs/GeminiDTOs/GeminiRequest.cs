using EnumShared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.GeminiDTOs
{
    public class GeminiRequest
    {
        public SystemInstruction systemInstruction { get; set; } = new();
        public List<Content> contents { get; set; } = new();

        public class Content
        {
            public string role {  get; set; } = string.Empty;
            public List<Part> parts { get; set; } = new();
        }
        public class Part
        {
            public string text { get; set; } = string.Empty;
        }
        public class SystemInstruction
        {
            public List<Part> parts { get; set;} = new();
        }
    }
}
