// TODO: Дописать данный класс

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AITW
{
    public class MovingSprite
    {
        public Texture2D Texture { get; set; }
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }
        public Vector2 OriginPosition { get; set; }
        public int XMax { get; set; }
        public int YMax { get; set; }
        public bool Inversely { get; set; }
        private Vector2 _position;
        private TimeSpan _updateRate = TimeSpan.FromSeconds(1);
        private TimeSpan _currentUpdateRate = TimeSpan.FromSeconds(1);

        public MovingSprite(Texture2D texture, Vector2 originPosition, int xspeed, int yspeed)
        {
            Texture = texture;
            OriginPosition = originPosition;
            XSpeed = xspeed;
            YSpeed = yspeed;
        }

        /// <summary>
        /// Двигает спрайт
        /// </summary>
        /// <param name="texture">Текстура</param>
        /// <param name="originPosition">Позиция с которой начинать</param>
        /// <param name="xspeed">Скорость X</param>
        /// <param name="yspeed">Скорость Y</param>
        /// <param name="inversely">Возвращаться обратно?</param>
        /// <param name="xmax">Координата X, после которой спрайт либо движется обратно (inversely), либо останавливается</param>
        /// <param name="ymax">Координата Y, после которой спрайт либо движется обратно (inversely), либо останавливается</param>
        public MovingSprite(Texture2D texture, Vector2 originPosition, int xspeed, int yspeed, bool inversely, int xmax, int ymax)
        {
            Texture = texture;
            OriginPosition = originPosition;
            XSpeed = xspeed;
            YSpeed = yspeed;
            Inversely = inversely;
            XMax = xmax;
            YMax = ymax;
        }

        public void Update(GameTime gameTime)
        {
            _currentUpdateRate -= gameTime.ElapsedGameTime;
            if (_currentUpdateRate <= TimeSpan.Zero)
            {
                _position.X += XSpeed;
                _position.Y += YSpeed;
                _currentUpdateRate = _updateRate;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            Vector2 cPosition = OriginPosition + _position;
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, cPosition, Color.White);
            spriteBatch.End();
        }
    }
}
