using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AITW
{
    internal class GUI
    {

        public GraphicsDevice GraphicsDevice { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool ShowHealthBar = true;
        private Player Player { get; set; }
        private SpriteBatch Batch { get; set; }

        private Texture2D _mHealthBar;

        public GUI(GraphicsDevice graphicsDevice, Player player, int width, int height)
        {
            GraphicsDevice = graphicsDevice;
            Player = player;
            Width = width;
            Height = height;
        }

        public void LoadContent(ContentManager contentManager, SpriteBatch batch)
        {

            _mHealthBar = contentManager.Load<Texture2D>("Sprites\\Objects\\HealthBar");
            Batch = batch;
        }

        public void Update(GameTime gameTime)
        {

        }

        private int GetHealth()
        {
            return Player.Health;
        }

        public void Draw()
        {
            if (ShowHealthBar)
            {
                DrawHealthBar();
            }
        }

        public void DrawHealthBar()
        {
            Batch.Begin();

            Batch.Draw(_mHealthBar, new Rectangle(Width / 2 - _mHealthBar.Width / 2,

                                                   30, _mHealthBar.Width, 44),
                        new Rectangle(0, 45, _mHealthBar.Width, 44), Color.Gray);


            Batch.Draw(_mHealthBar, new Rectangle(Width / 2 - _mHealthBar.Width / 2,
                                                   30, (int)(_mHealthBar.Width * ((double)GetHealth() / 100)), 44),
                        new Rectangle(0, 45, _mHealthBar.Width, 44), Color.Red);

            Batch.Draw(_mHealthBar, new Rectangle(Width / 2 - _mHealthBar.Width / 2,

                                                   30, _mHealthBar.Width, 44),
                        new Rectangle(0, 0, _mHealthBar.Width, 44), Color.White);


            Batch.End();
        }

        public void DrawQuest(SpriteBatch batch, SpriteFont font, string text)
        {
            batch.Begin();
            batch.DrawString(font, text, new Vector2(16, 16), Color.White);
            batch.End();
        }
    }
}
