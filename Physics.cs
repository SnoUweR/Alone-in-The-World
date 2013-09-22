using System;
using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AITW
{
    class Physics
    {
        public World World;
        public static readonly Object TYPE_GROUND = new Object();
        public static readonly Object TYPE_PLAYER = new Object();
        public static readonly Object TYPE_COIN = new Object();
        public static readonly Object TYPE_EXIT = new Object();
        public static readonly Object TYPE_NPC = new Object();

        public Body TerrainBody;
        public Body RectangleBody;
        public Body PlayerCirlceBody;
        public Body CoinBody;
        public Body PlayerRectBody;
        public Body SomeBodyRectangle;
        public Body SomeBodyCircle;

        private const float MeterInPixels = 64f;

        public Physics()
        {
            World = new World(new Vector2(0, 20));
        }


        /// <summary>
        /// Создает «прямоугольник с физикой»
        /// </summary>
        /// <param name="x">X-координата</param>
        /// <param name="y">Y-координата</param>
        /// <param name="width">ширина</param>
        /// <param name="height">высота</param>
        /// <param name="type">тип</param>
        public void AddRect(float x, float y, float width, float height, object type)
        {

            var groundPosition = new Vector2(x / MeterInPixels, y / MeterInPixels);

            RectangleBody = BodyFactory.CreateRectangle(World, width / MeterInPixels, height / MeterInPixels, 1f, groundPosition, type);
            RectangleBody.IsStatic = true;
            RectangleBody.Restitution = 0f;
            RectangleBody.UserData = type;
            RectangleBody.Friction = 0.5f;
        }

        public void AddTerrainBody(float x, float y, float width, float height, object type)
        {
            TerrainBody.UserData = type;
            TerrainBody.IsStatic = true;
            TerrainBody.Restitution = 0f;
            TerrainBody.Friction = 0.5f;

            var groundPosition = new Vector2(x / MeterInPixels, y / MeterInPixels);
            Fixture RectangleFixture = FixtureFactory.AttachRectangle(width/MeterInPixels, height/MeterInPixels, 0f,
                                                                      groundPosition, TerrainBody, type);
        }


        public void AddTerrainPhysics(Texture2D mapTexture, Vector2 offset, bool overridePlayer = false)
        {
            Color[,] pixelColor = TextureTo2DArray(mapTexture);
            TerrainBody = BodyFactory.CreateBody(World);

            for (int x = 0; x < mapTexture.Width; x++)
            {
                for (int y = 0; y < mapTexture.Height; y++)
                {
                    if (pixelColor[x, y] == Color.Black)
                    {
                        AddTerrainBody(x + Convert.ToInt32(offset.X), y + Convert.ToInt32(offset.Y), 1, 1, TYPE_GROUND);
                    }
                    else if (pixelColor[x, y] == Color.Green)
                    {
                        Console.WriteLine("PLAYER SPAWN AT: " + x + " ," + y);
                        if (overridePlayer)
                        {
                            AddPlayerBody(new Vector2(x / MeterInPixels, y / MeterInPixels));
                        }
                    }
                    else if (pixelColor[x, y] == Color.Blue)
                    {
                        Console.WriteLine("LEVEL END TRIGGER AT: " + x + " ," + y);
                        AddRect(x, y, 32, 256, TYPE_EXIT);
                    }
                }
            }
        }

        public void AddPlayerBody(Vector2 position)
        {

            PlayerRectBody = BodyFactory.CreateRectangle(World, 32f / MeterInPixels, 64f / MeterInPixels, 1.0f, position);

            PlayerCirlceBody = BodyFactory.CreateCircle(World, 16f / MeterInPixels, 1.0f, position + new Vector2(0f, 32f / MeterInPixels));

            PlayerRectBody.UserData = TYPE_PLAYER;
            PlayerCirlceBody.UserData = TYPE_PLAYER;
            PlayerRectBody.BodyType = BodyType.Dynamic;

            PlayerCirlceBody.BodyType = BodyType.Dynamic;

            var revJoint = JointFactory.CreateRevoluteJoint(World, PlayerCirlceBody, PlayerRectBody, new Vector2(0.0f, 16f / MeterInPixels));
            JointFactory.CreateFixedAngleJoint(World, PlayerRectBody);
            JointFactory.CreateAngleJoint(World, PlayerCirlceBody, PlayerRectBody);

            revJoint.LimitEnabled = false;
            revJoint.MotorSpeed = 5.0f;
            revJoint.MotorEnabled = true;
        }

        public void AddSomeBody(Vector2 position, Object type)
        {
            SomeBodyRectangle = BodyFactory.CreateRectangle(World, 32f / MeterInPixels, 64f / MeterInPixels, 1.0f, position);

            SomeBodyCircle = BodyFactory.CreateCircle(World, 16f / MeterInPixels, 1.0f, position + new Vector2(0f, 32f / MeterInPixels));

            SomeBodyRectangle.UserData = type;
            SomeBodyCircle.UserData = type;
            SomeBodyRectangle.BodyType = BodyType.Dynamic;

            SomeBodyCircle.BodyType = BodyType.Dynamic;

            var revJoint = JointFactory.CreateRevoluteJoint(World, SomeBodyCircle, SomeBodyRectangle, new Vector2(0.0f, 16f / MeterInPixels));
            JointFactory.CreateFixedAngleJoint(World, SomeBodyRectangle);
            JointFactory.CreateAngleJoint(World, SomeBodyCircle, SomeBodyRectangle);

            revJoint.LimitEnabled = false;
            revJoint.MotorSpeed = 5.0f;
            revJoint.MotorEnabled = true;
        }

        public void AddCoinBody(Vector2 position)
        {
            CoinBody = BodyFactory.CreateCircle(World, 16f / MeterInPixels, 0f, position, TYPE_COIN);
        }

        public void Update(GameTime gameTime)
        {
            World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
        }

        private Color[,] TextureTo2DArray(Texture2D texture)
        {
            var colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            var colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

        public Vector2 DistanceBetweenBodies(Body b1, Body b2)
        {
            return b1.Position - b2.Position;
        }
    }
}
