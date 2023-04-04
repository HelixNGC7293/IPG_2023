using System;

namespace ChatGPTWrapper
{
    [Serializable]
    public class ChatGPTUsage
    {
        public int completion_tokens;
        public int total_tokens;
    }
}
