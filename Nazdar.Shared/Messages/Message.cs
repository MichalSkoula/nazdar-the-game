using Microsoft.Xna.Framework;
using static Nazdar.Enums;

namespace Nazdar.Messages
{
    public class Message
    {
        public string Text { get; set; }
        public double Ttl;
        public Color Color;

        public Message(string text, MessageType? messageType, double ttl)
        {
            this.Text = text;
            this.Ttl = ttl;

            this.Color = messageType switch
            {
                MessageType.Info => Color.White,
                MessageType.Success => Color.LightGreen,
                MessageType.Fail => Color.IndianRed,
                MessageType.Danger => Color.Orange,
                MessageType.Opportunity => Color.LightBlue,
                _ => Color.White,
            };
        }
    }
}
