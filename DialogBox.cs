using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace AITW
{
    class DialogBox
    {
        [Flags]
        public enum Alignment { Center = 0, Left = 1, Right = 2, Top = 4, Bottom = 8, TopLeft = 16 }
        public enum DialogPosition
        {
            Center,
            Top,
            Bottom
        }

        public GraphicsDevice GraphicsDevice { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private Texture2D _dialogTexture;
        private Timer _timer;

        public DialogBox(GraphicsDevice graphicsDevice, int width, int height)
        {
            GraphicsDevice = graphicsDevice;
            Width = width;
            Height = height;
            _timer = new Timer(new TimeSpan(0,0,0,10)) {Enabled = false};
        }

        public Texture2D CreateRectangle(int width, int height)
        {
            var rectangleTexture = new Texture2D(GraphicsDevice, width, height, false, SurfaceFormat.Color);
            var color = new Color[width * height];
            for (var i = 0; i < color.Length; i++)
            {
                color[i] = new Color(0, 0, 0, 100);
            }
            rectangleTexture.SetData(color);
            return rectangleTexture;
        }

        public void LoadContent()
        {
            _dialogTexture = CreateRectangle(1020, 128);
            _timer.TimerTick += TimerOnTimerTick;
            _timer.TimerIsFinished += TimerOnTimerIsFinished;
        }

        private void TimerOnTimerIsFinished(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Прошёл таймер");
        }

        void TimerOnTimerTick(object sender, EventArgs e)
        {
            
            Console.WriteLine("Прошла секунда");
        }

        

        public void Update(GameTime gameTime)
        {
            _timer.Update(gameTime, Timer.TimerType.Second);
        }



        public void DrawString(SpriteBatch batch, SpriteFont font, string text, Rectangle bounds, Alignment align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X, bounds.Center.Y);
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(Alignment.Left))
                origin.X += bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Right))
                origin.X -= bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Top))
                origin.Y += bounds.Height / 2 - size.Y / 2;

            if (align.HasFlag(Alignment.Bottom))
                origin.Y -= bounds.Height / 2 - size.Y / 2;

            if (align.HasFlag(Alignment.TopLeft))
            {
                origin.X += bounds.Width/2 - size.X/2;
                origin.Y += bounds.Height/2 - size.Y/2;
            }

            batch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch batch, DialogPosition dialogPosition, Alignment alignment, SpriteFont spriteFont, Color color, string text)
        {
            Vector2 position = Vector2.Zero;
            batch.Begin();

            switch (dialogPosition)
            {
                case DialogPosition.Top:
                    position = new Vector2(2, 2);
                    break;
                case DialogPosition.Bottom:
                    position = new Vector2(Width - 2 - 1020, Height - 2 - 128);
                    break;
                case DialogPosition.Center:
                    position = new Vector2(Width / 2 - 510, Height / 2 - 64);
                    break;
            }

            batch.Draw(_dialogTexture, position, Color.White);
            DrawString(batch, spriteFont, text, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), 1020, 128), alignment, color);
            batch.End();
        }
    }
}
