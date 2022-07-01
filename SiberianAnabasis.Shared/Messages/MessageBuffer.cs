using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiberianAnabasis.Shared.Messages
{
    public class MessageBuffer
    {
        public List<Message> Messages = new List<Message>();
        private int messagesLimit = 5;

        public void AddMessage(string text, double? ttl = 5)
        {
            this.Messages.Add(new Message(text, (double)ttl));
        }

        public void Update(double deltaTime)
        {
            foreach (var message in this.Messages)
            {
                message.Ttl -= deltaTime;
            }

            // too old?
            this.Messages.RemoveAll(message => message.Ttl <= 0);

            // too much messages?
            if (this.Messages.Count > this.messagesLimit)
            {
                this.Messages.RemoveAt(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch, float translationX = 0)
        {
            for (int i = 0; i < this.Messages.Count; i++)
            {
                spriteBatch.DrawString(Assets.FontSmall, this.Messages[i].Text, new Vector2(Enums.Offset.MessagesX - translationX, 10 + 15 * i), Color.White);
            }
        }
    }
}
