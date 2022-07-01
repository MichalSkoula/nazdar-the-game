using System;
using System.Collections.Generic;
using System.Text;

namespace SiberianAnabasis.Shared.Messages
{
    public class Message
    {
        public string Text { get; set; }
        public double Ttl;

        public Message(string text, double ttl)
        {
            this.Text = text;
            this.Ttl = ttl;
        }
    }
}
