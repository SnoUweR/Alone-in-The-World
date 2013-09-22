using AITW.ScreenManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AITW
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private Level _level;

        public int Width;
        public int Height;

        public SpriteBatch Batch;

        private readonly GraphicsDeviceManager _graphics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Width = _graphics.PreferredBackBufferWidth = 1024;
            Height = _graphics.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Commons.Game = this;
            Commons.Content = Content;
            Commons.GraphicsDeviceManager = _graphics;
            Commons.GraphicsDevice = _graphics.GraphicsDevice;
            Commons.SpriteBatch = new SpriteBatch(_graphics.GraphicsDevice);

            SM.AddScreen(new MainMenuScreen());
            SM.AddScreen(new Level(Width, Height));
            SM.ActivateScreenByName("MainMenuScreen");
            SM.Initialize();
            SM.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            SM.Update(gameTime);
            //_level.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(190, 213, 254));
            SM.Draw(gameTime);
            //_level.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}