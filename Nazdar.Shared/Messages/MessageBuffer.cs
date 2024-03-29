﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Messages
{
    public class MessageBuffer
    {
        private List<Message> messages = new List<Message>();
        private Message superMessage = null;
        private int messagesLimit = 7;
        private bool superMessageSmall = false;

        public void AddMessage(string text, MessageType? messageType = MessageType.Info, double? ttl = 7)
        {
            this.messages.Add(new Message(text, (double)ttl, (MessageType)messageType));
        }

        public void SetSuperMessage(string text, double ttl = 7, bool small = false)
        {
            this.superMessage = new Message(text, ttl);
            this.superMessageSmall = small;
        }

        public void DeleteSuperMessage()
        {
            this.superMessage = null;
        }

        public void Update(double deltaTime)
        {
            foreach (var message in this.messages)
            {
                message.Ttl -= deltaTime;
            }

            // too old?
            this.messages.RemoveAll(message => message.Ttl <= 0);

            // too much messages?
            if (this.messages.Count > this.messagesLimit)
            {
                this.messages.RemoveAt(0);
            }

            // supermessage
            if (superMessage != null)
            {
                superMessage.Ttl -= deltaTime;
                if (superMessage.Ttl <= 0)
                {
                    this.DeleteSuperMessage();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float translationX = 0, bool drawSupperMessage = false)
        {
            for (int i = 0; i < this.messages.Count; i++)
            {
                spriteBatch.DrawString(
                    Assets.Fonts["Small"],
                    this.messages[i].Text,
                    new Vector2(Offset.MessagesX - translationX, 10 + 15 * i),
                    this.messages[i].Color
                );
            }

            if (this.superMessage != null && drawSupperMessage)
            {
                var fontSize = this.superMessageSmall ? "Small" : "Medium";
                float textWidth = Assets.Fonts[fontSize].MeasureString(this.superMessage.Text).X;

                spriteBatch.DrawString(
                    Assets.Fonts[fontSize],
                    this.superMessage.Text,
                    new Vector2((int)((Enums.Screen.Width - textWidth) / 2 - translationX), Enums.Offset.SuperMessageY),
                    this.superMessage.Color
                );
            }
        }
    }
}
