using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AITW
{
    class Player : IBodyInfo
    {
        private Physics Physics { get; set; }
        private Level Level { get; set; }
        private GameTime GameTime { get; set; }

        private Vector2 Position { get; set; }

        private KeyboardState _oldKeyState;

        private Character _leo;

        private SpriteEffects _effect;
        private String _action;
        private String _facing;

        public bool OnGround;

        private const float MeterInPixels = 64f;

        public Player(Level level, Physics physics, Vector2 position)
        {
            Physics = physics;
            Position = position;
            Level = level;
        }

        public void LoadContent(ContentManager content)
        {
            _leo = new Character("Leo", content); // 32x64 -> 0.5m x 1m
        }

        public void CreatePlayer(Vector2 position)
        {
            var playerPosition = new Vector2(position.X / MeterInPixels, position.Y / MeterInPixels);
            Physics.AddPlayerBody(playerPosition);

            Physics.PlayerCirlceBody.OnCollision += PlayerCirlceCollision;
            Physics.PlayerCirlceBody.OnSeparation += PlayerCirlceBodyOnOnSeparation;

            Health = 100;
            IsAlive = true;
        }

        public Vector2 GetPosition()
        {
            try
            {
                return Physics.PlayerRectBody.Position * MeterInPixels;
            }
            catch
            {
                Console.WriteLine("Не удалось получить позицию игрока");
                return Vector2.Zero;
            }

        }

        public bool PlayerCirlceCollision(Fixture f1, Fixture f2, Contact contact)
        {
            if (f1.UserData == Physics.TYPE_COIN || f2.UserData == Physics.TYPE_COIN)
            {
                Physics.World.RemoveBody(Physics.CoinBody);
                Level.Score++;
            }
            else if (f1.Body.UserData == Physics.TYPE_GROUND || f2.Body.UserData == Physics.TYPE_GROUND)
            {
                OnGround = true;
            }
            else if (f1.UserData == Physics.TYPE_EXIT || f2.UserData == Physics.TYPE_EXIT)
            {
                if (Level.LevelNumber < Level.MaxLevels)
                {
                    Level.LevelNumber++;
                }
                else
                {
                    Level.LevelNumber = 1;
                }
                Level.Reset();
            }
            return true;
        }

        public void PlayerCirlceBodyOnOnSeparation(Fixture f1, Fixture f2)
        {
            if (f1.Body.UserData == Physics.TYPE_GROUND || f2.Body.UserData == Physics.TYPE_GROUND)
            {
                OnGround = false;
            }
        }

        public void Hurt(int damage)
        {
            Health -= damage;
            Health = (int)MathHelper.Clamp(Health, 0, 100);

            if (Health <= 0)
            {
                IsAlive = false;
                Console.WriteLine("Вы погибли");
                Level.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            HandleInput(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
            {
                _action = "walking";
                _facing = "left";
                _effect = SpriteEffects.FlipHorizontally;
                _leo.Update_Walking(gameTime);
                Physics.PlayerCirlceBody.ApplyForce(new Vector2(-10, 0), Physics.PlayerCirlceBody.Position);
            }

            else if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
            {
                _action = "walking";
                _facing = "right";
                _effect = SpriteEffects.None;
                _leo.Update_Walking(gameTime);
                Physics.PlayerCirlceBody.ApplyForce(new Vector2(10, 0), Physics.PlayerCirlceBody.Position);
            }

            else if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
            {
                if (true)
                {
                    _action = "jumping";
                    _facing = "right";
                    _effect = SpriteEffects.None;
                    _leo.Update_Jumping(gameTime);
                    Physics.PlayerCirlceBody.ApplyLinearImpulse(new Vector2(0, -5), Physics.PlayerCirlceBody.Position);
                }
            }
            else
            {
                _action = "stand";
                _leo.Update_Stand(gameTime);
            }

            _oldKeyState = state;
        }

        public void Draw(SpriteBatch batch)
        {
            var playerPos = Physics.PlayerRectBody.Position * MeterInPixels;
            _leo.Draw(batch, playerPos, _effect, _action);
        }

        //TODO: Доделать вот это всё
        //public void DrawDialog(TimeSpan seconds)
        //{
        //    _timer.DoThatSecondsInit(seconds);
        //}

        //private void DrawingDialog()
        //{
        //    var playerPos = Physics.PlayerRectBody.Position * MeterInPixels;
        //    if (_timer.DoThatSeconds(GameTime))
        //    {

        //    }
        //}
    }

}
