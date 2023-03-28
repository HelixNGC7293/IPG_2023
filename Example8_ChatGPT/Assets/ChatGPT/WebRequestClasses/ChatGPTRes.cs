using System;
using System.Collections.Generic;

namespace ChatGPTWrapper {
    [Serializable]
    public class ChatGPTRes
    {
        public ChatGPTUsage usage;
        public List<ChatGPTChoices> choices;
    }
}
