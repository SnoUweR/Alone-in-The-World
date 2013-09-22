using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AITW
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public TimeSpan AniSpeed { get; set; }
        private TimeSpan _currentAniSpeed;
        private int _currentFrame;
        private int _totalFrames;
        public delegate void SimpleEventHandler(object sender, EventArgs e);
        public event SimpleEventHandler AnimationIsFinished;

        public AnimatedSprite(Texture2D texture, int rows, int columns, TimeSpan anispeed)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            AniSpeed = anispeed;
            _currentAniSpeed = anispeed;
            _currentFrame = 0;
            _totalFrames = Rows*Columns;
        }

        public void Update(GameTime gameTime)
        {
            _currentAniSpeed -= gameTime.ElapsedGameTime;
            if (_currentAniSpeed <= TimeSpan.Zero)
            {
                _currentAniSpeed = AniSpeed;
                _currentFrame++;
                if (_currentFrame == _totalFrames)
                {
                    AnimationIsFinished(this, EventArgs.Empty);
                    _currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)_currentFrame / (float)Columns);
            int column = _currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
