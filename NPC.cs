using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AITW
{
    internal class NPC : IBodyInfo
    {
        private Physics Physics { get; set; }
        private Level Level { get; set; }
        private Player Player { get; set; }
        private GameTime GameTime { get; set; }
        private int Damage { get; set; }
        private int Health { get; set; }

        private Vector2 Position { get; set; }

        private Character _character;

        private SpriteEffects _effect;
        private String _action;
        private String _facing;

        private Timer _timer;
        private Helpers _helpers;

        private const float MeterInPixels = 64f;

        public NPC(Level level, Physics physics, Player player, int damage, Vector2 position)
        {
            Physics = physics;
            Position = position;
            Player = player;
            Damage = damage;
            Level = level;
            _timer = new Timer(new TimeSpan(0, 1, 0, 0));
            _timer.TimerTick += TimerOnTimerTick;
            _helpers = new Helpers();
        }

        private void TimerOnTimerTick(object sender, EventArgs eventArgs)
        {
            try
            {
                Physics.SomeBodyCircle.ApplyLinearImpulse(new Vector2(0, -5), Physics.SomeBodyCircle.Position);

                _action = "stand";
                _facing = "right";
                _effect = SpriteEffects.None;
                _character.Update_Stand(GameTime);
            }
            catch
            {
            }

        }

        public Vector2 GetPosition()
        {
            try
            {
                return Physics.SomeBodyRectangle.Position*MeterInPixels;
            }
            catch
            {
                Console.WriteLine("Не удалось получить позицию NPC");
                return Vector2.Zero;
            }

        }

        public void LoadContent(ContentManager content, string characterName = "Dummy")
        {
            _character = new Character(characterName, content); // 32x64 -> 0.5m x 1m
        }

        public void CreateBody(Vector2 position)
        {
            var bodyPosition = new Vector2(position.X/MeterInPixels, position.Y/MeterInPixels);
            Physics.AddSomeBody(bodyPosition, Physics.TYPE_NPC);

            Physics.SomeBodyRectangle.OnCollision += SomeBodyRectangleOnOnCollision;
            Physics.SomeBodyCircle.OnSeparation += SomeBodyCircleOnOnSeparation;
        }

        private bool SomeBodyRectangleOnOnCollision(Fixture f1, Fixture f2, Contact contact)
        {
            if (f1.Body.UserData == Physics.TYPE_PLAYER || f2.Body.UserData == Physics.TYPE_PLAYER)
            {
                Player.Hurt(Damage);
            }
            return true;
        }


        public void SomeBodyCircleOnOnSeparation(Fixture f1, Fixture f2)
        {

        }

        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            var xDist = Physics.DistanceBetweenBodies(Physics.SomeBodyRectangle, Physics.PlayerRectBody).X * MeterInPixels;
            if (Math.Abs(xDist) >= 128)
            {
                Console.WriteLine(xDist);
                if (xDist < 0)
                {
                    Physics.SomeBodyCircle.ApplyForce(new Vector2(10, 0),
                  Physics.SomeBodyCircle.Position);
                }
                else if (xDist > 0)
                {
                    Physics.SomeBodyCircle.ApplyForce(new Vector2(-10, 0),
                  Physics.SomeBodyCircle.Position);
                }


                _action = "stand";
                _facing = "right";
                _effect = SpriteEffects.None;
                _character.Update_Stand(GameTime);
            }

            _timer.Update(gameTime, Timer.TimerType.TenSeconds);
        }

        public void Draw(SpriteBatch batch)
        {
            var npcPos = Physics.SomeBodyRectangle.Position*MeterInPixels;
            _character.Draw(batch, npcPos, _effect, _action);
        }
    }
}
