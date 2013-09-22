using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AITW.ScreenManager;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.DebugViews;
using Microsoft.Xna.Framework.Media;

// TODO: Добавить партикли дыма для пламени, звук пламени и, желательно, шейдер для пламени

namespace AITW
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    internal class Level : Screen
    {
        #region Переменные

        private Physics _physics;
        private DialogBox _dialogBox;
        private Player _player;
        private Timer _timer;
        private LevelParser _levelParser;
        private LevelInfo _levelInfo;
        private NPC _npc;
        private GUI _gui;
        private QuestEngine _qE;
        private SpriteBatch Batch { get; set; }

        private Texture2D _levelPhysics;
        private Texture2D _levelTexture;
        private Texture2D _levelGradient;

        private readonly string[] _arrayTutorText = new string[]
            {
                "Добро пожаловать в Alone in The World",
                "На данный момент, игра из себя ничего не представляет, так как она не завершена",
                "Для управления используйте стрелки, для прыжка - клавишу Пробел",
                "Удачного тестирования!"
            };

        private int _currentTutorText;

        internal const int MaxLevels = 2;
        internal int LevelNumber = 1;
        private SpriteFont _mainFont;
        private SpriteFont _pixelFont;
        private KeyboardState _oldKeyState;
        private ContentManager Content { get; set; }
        private GraphicsDeviceManager Graphics { get; set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }

        private Song _music;

        private Vector2 _logoStoryPosition = new Vector2(0, 125);
        internal bool DEBUG = true;

        private bool _isStarting;
        private bool _isStarted;
        private bool _tutorialPassed;

        internal int Score;

        private Texture2D _logoStoryTexture;
        private Texture2D _pressSpaceTexture;

        // Simple camera controls
        private Matrix _view;
        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;

        private Vector2 _starPosition = new Vector2(10, 10);

        private int _mAlphaValue = 1;
        private int _mFadeIncrement = 9;
        private double _mFadeDelay = .035;

        // physics simulator debug view
        DebugViewXNA _debugView;
        private const float MeterInPixels = 64f;

        #endregion

        internal Level(int width, int height) : base("Level")
        {
            Content = Commons.Content;
            Batch = Commons.SpriteBatch;
            Graphics = Commons.GraphicsDeviceManager;
            GraphicsDevice = Commons.GraphicsDevice;
            Width = width;
            Height = height;

            _cameraPosition.Y = 0;
            _cameraPosition.X = -16;

            _screenCenter = new Vector2(Graphics.GraphicsDevice.Viewport.Width / 2f, Graphics.GraphicsDevice.Viewport.Height / 2f);

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f))*
                    Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));

            _physics = new Physics();
            _dialogBox = new DialogBox(GraphicsDevice, Width, Height);
            _levelParser = new LevelParser(Content);
            _player = new Player(this, _physics, Vector2.Zero);
            _timer = new Timer();
            _npc = new NPC(this, _physics, _player, 15, Vector2.Zero);
            _gui = new GUI(GraphicsDevice, _player, Width, Height);
            _qE = new QuestEngine(this);
        }

        internal Level() : base("Level")
        {
            
        }

        internal override void LoadContent()
        {
            _player.LoadContent(Content);
            _npc.LoadContent(Content, "Dummy");
            _gui.LoadContent(Content, Batch);

            _mainFont = Content.Load<SpriteFont>("Media\\Fonts\\MainFont");
            _pixelFont = Content.Load<SpriteFont>("Media\\Fonts\\PixelFont");

            _qE.LoadContent(Content, _gui, _mainFont);

            Reset();

            _dialogBox.LoadContent();

            _logoStoryTexture  = Content.Load<Texture2D>("Sprites\\Objects\\logoStory0");
            _pressSpaceTexture = Content.Load<Texture2D>("Sprites\\Objects\\logoStory1");

            _debugView = new DebugViewXNA(_physics.World) { DefaultShapeColor = Color.White, SleepingShapeColor = Color.LightGray };
            _debugView.LoadContent(GraphicsDevice, Content);

            base.LoadContent();
        }

        internal void Reset()
        {
            _levelParser.Parse(LevelNumber);
            _levelInfo = _levelParser.GetInfo();
            _levelGradient = Content.Load<Texture2D>("Maps\\Gradient");
            _levelPhysics = Content.Load<Texture2D>("Maps\\Level" + LevelNumber + "C");
            _levelTexture = Content.Load<Texture2D>("Maps\\Level" + LevelNumber);
            _music = Content.Load<Song>("Media\\Music\\" + _levelInfo.Music);
            //_helpers.PlayMusic(_music);

            foreach (Body b in _physics.World.BodyList)
            {
                if (b.UserData == Physics.TYPE_GROUND || b.UserData == Physics.TYPE_PLAYER || b.UserData == Physics.TYPE_EXIT || b.UserData == Physics.TYPE_NPC)
                {
                    _physics.World.RemoveBody(b);
                }
            }
            _physics.AddTerrainPhysics(_levelPhysics, new Vector2(0, DEBUG ? 0 : 768));
            Vector2 playerPosition = new Vector2(32f, DEBUG ? 500f : 1000f);
            _player.CreatePlayer(playerPosition);

            Vector2 npcPosition = new Vector2(128f, 500f);
            _npc.CreateBody(npcPosition);

            _qE.CreateQuest(QuestEngine.QuestTypes.Enemies, "Соберите все монеты");
        }

        internal int RemainedCoinsOnLevel()
        {
            int coins = 1;
            foreach (Body b in _physics.World.BodyList)
            {
                if (b.UserData == Physics.TYPE_COIN)
                {
                    coins++;
                }
            }
            return coins;
        }

        internal int RemainedEnemiesOnLevel()
        {
            int enemies = 0;
            foreach (Body b in _physics.World.BodyList)
            {
                if (b.UserData == Physics.TYPE_NPC)
                {
                    enemies++;
                }
            }
            return enemies;
        }

        internal override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (_tutorialPassed && _isStarted || DEBUG)
            {

            }

            else if (!_tutorialPassed && _isStarted)
            {
                if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                {
                    if (_currentTutorText + 1 >= _arrayTutorText.Length)
                    {
                        _tutorialPassed = true;
                    }
                    else
                    {
                        _currentTutorText++;
                    }
                }
                _oldKeyState = state;
            }

            if (!DEBUG)
            {
                if (!_isStarted)
                {
                    if (state.IsKeyDown(Keys.Space))
                    {
                        _isStarting = true;
                    }
                }

                if (_isStarting)
                {
                    if (_cameraPosition.Y >= -768)
                    {
                        _logoStoryPosition = _logoStoryPosition - new Vector2(0, 1);
                        _cameraPosition.Y -= 1;

                        Console.WriteLine("Logo Position: " + _logoStoryPosition.Y);
                        _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                                Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));
                    }
                    else
                    {
                        _cameraPosition.Y = -768;

                        _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                                Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));
                        _isStarting = false;
                        _isStarted = true;
                    }
                }


            }

            if (state.IsKeyDown(Keys.R))
            {
                Reset();
            }
            else if (state.IsKeyDown(Keys.K))
            {
                foreach (Body b in _physics.World.BodyList)
                {
                    if (b.UserData == Physics.TYPE_NPC)
                    {
                        _physics.World.RemoveBody(b);
                    }
                }
            }

            _mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;

            if (_mFadeDelay <= 0)
            {
                _mFadeDelay = .035;

                _mAlphaValue += _mFadeIncrement;

                if (_mAlphaValue >= 255 || _mAlphaValue <= 0)
                {
                    _mFadeIncrement *= -1;
                }
            }
            _dialogBox.Update(gameTime);
            _player.Update(gameTime);
            _npc.Update(gameTime);
            _physics.Update(gameTime);
            _qE.Update(gameTime);
            _gui.Update(gameTime);

            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var ms = Mouse.GetState();

            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);
            Batch.Draw(_levelTexture, new Vector2(16, DEBUG ? 0 : 768), Color.White);
            _player.Draw(Batch);
            _npc.Draw(Batch);
            Batch.End();

            Batch.Begin();
            Batch.Draw(_levelGradient, Vector2.Zero, Color.White);
            Batch.End();

            if (DEBUG)
            {
                Batch.Begin();
                Batch.DrawString(_mainFont, String.Concat("Mouse X: ", ms.X.ToString(CultureInfo.InvariantCulture)), new Vector2(10, 10), Color.Red);
                Batch.DrawString(_mainFont, String.Concat("Mouse Y: ", ms.Y.ToString(CultureInfo.InvariantCulture)), new Vector2(10, 40), Color.Red);
                Batch.DrawString(_mainFont, String.Concat("Player X: ", _physics.PlayerCirlceBody.Position.X.ToString(CultureInfo.InvariantCulture)), new Vector2(10, 70), Color.Green);
                Batch.DrawString(_mainFont, String.Concat("Player Y: ", _physics.PlayerCirlceBody.Position.Y.ToString(CultureInfo.InvariantCulture)), new Vector2(10, 100), Color.Green);
                Batch.DrawString(_mainFont, String.Concat("Score: ", Score.ToString(CultureInfo.InvariantCulture)), new Vector2(10, 160), Color.Yellow);
                Batch.End();
            }


            if (!_tutorialPassed && _isStarted)
            {
                _dialogBox.Draw(Batch, DialogBox.DialogPosition.Center, DialogBox.Alignment.Center, _mainFont, Color.White, _arrayTutorText[_currentTutorText]);
            }


            if (!DEBUG)
            {
                Batch.Begin();
                Batch.Draw(_logoStoryTexture, _logoStoryPosition, Color.White);
                if (!_isStarting)
                {
                    Batch.Draw(_pressSpaceTexture, _logoStoryPosition + new Vector2(0, 356), new Color((byte)MathHelper.Clamp(_mAlphaValue, 0, 255), (byte)MathHelper.Clamp(_mAlphaValue, 0, 255), (byte)MathHelper.Clamp(_mAlphaValue, 0, 255), (byte)MathHelper.Clamp(_mAlphaValue, 0, 255)));
                }
                Batch.End();
            }

            if (_timer.DoThatSeconds(gameTime))
            {
                Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);
                Batch.DrawString(_pixelFont, "Что-то тут не так...", _player.GetPosition() + new Vector2(16, -32), Color.Black);
                Batch.End();
            }

            _qE.Draw(Batch);
            _gui.Draw();


            Matrix projection = Matrix.CreateOrthographicOffCenter(0f,
                                                                   Graphics.GraphicsDevice.Viewport.Width / MeterInPixels,
                                                                   Graphics.GraphicsDevice.Viewport.Height /
                                                                   MeterInPixels, 0f, 0f,
                                                                   1f);
            Matrix view = Matrix.CreateTranslation(new Vector3((_cameraPosition / MeterInPixels) - (_screenCenter / MeterInPixels), 0f)) * Matrix.CreateTranslation(new Vector3((_screenCenter / MeterInPixels), 0f));
            if (DEBUG) _debugView.RenderDebugData(ref projection, ref view);

            base.Draw(gameTime);
        }
    }
}