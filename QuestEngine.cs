using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AITW
{
    class QuestEngine
    {
        private GUI GUI { get; set; }
        public bool IsDone;

        public enum QuestTypes
        {
            Coins,
            Enemies,
            Exit
        }

        private QuestTypes QuestType { get; set; }
        private String QuestText { get; set; }
        private Level Level { get; set; }
        private SpriteFont Font { get; set; }

        public QuestEngine(Level level)
        {
            Level = level;
        }

        public void LoadContent(ContentManager content, GUI gui, SpriteFont font)
        {
            Font = font;
            GUI = gui;
        }

        public void CreateQuest(QuestTypes questType, string questText)
        {
            QuestType = questType;
            QuestText = questText;
        }

        public void Update(GameTime gameTime)
        {
            if (QuestType.HasFlag(QuestTypes.Coins))
            {
                if(Level.RemainedCoinsOnLevel() <= 0)
                {
                    IsDone = true;
                }
            }
            if (QuestType.HasFlag(QuestTypes.Enemies))
            {
                if (Level.RemainedEnemiesOnLevel() <= 0)
                {
                    IsDone = true;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if(QuestType != null && QuestText != null)
            {
                if (!IsDone)
                {
                    GUI.DrawQuest(batch, Font, QuestText);
                }
            }
        }
    }
}
