using Microsoft.Xna.Framework;
using static Nazdar.Enums;

namespace Nazdar.Messages
{
    public class Message
    {
        public string Text { get; set; }
        public double Ttl;
        public Color Color;
        public MessageType Type;

        public Message(string text, double ttl, MessageType? messageType = MessageType.Info)
        {
            this.Text = text;
            this.Ttl = ttl;
            this.Type = (MessageType)messageType;

            this.Color = messageType switch
            {
                MessageType.Info => MyColor.White,
                MessageType.Success => MyColor.Green,
                MessageType.Fail => MyColor.Red,
                MessageType.Danger => MyColor.Orange,
                MessageType.Opportunity => MyColor.Turquoise,
                MessageType.Super => MyColor.Violet,
                _ => MyColor.White,
            };
        }
    }
}
