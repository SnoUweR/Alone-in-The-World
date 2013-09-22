using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Globalization;
using Microsoft.Xna.Framework.Graphics;

namespace AITW
{
    class Character
    {
        #region Fields
        // The texture with animation frames
        Texture2D animationTexture_Stand;
        Texture2D animationTexture_Walking;
        Texture2D animationTexture_Jumping;
        // The size and structure of whole frames sheet in animationTexture. The animationTexture could
        // hold animaton sequence organized in multiple rows and multiple columns, that's why animation 
        // engine should know how the frames are organized inside a frames sheet
        Point sheetSize_Stand;
        Point sheetSize_Walking;
        Point sheetSize_Jumping;
        // Amount of time between frames
        TimeSpan frameInterval_Stand;
        TimeSpan frameInterval_Walking;
        TimeSpan frameInterval_Jumping;
        // Time passed since last frame
        TimeSpan nextFrame_Stand;
        TimeSpan nextFrame_Walking;
        TimeSpan nextFrame_Jumping;

        // Current frame in the animation sequence
        public Point currentFrame_Stand;
        public Point currentFrame_Walking;
        public Point currentFrame_Jumping;
        // The size of single frame inside the animationTexture
        public Point frameSize_Stand;
        public Point frameSize_Walking;
        public Point frameSize_Jumping;
        public int Width = 32;
        public int Height = 64;

        // character file
        System.Xml.Linq.XDocument doc;

        #endregion

        #region Initialization
        /// <summary>
        /// Constructor of a character class
        /// </summary>
        /// <param name="characterName">the name of the xml file of the character without .xml</param>
        /// <param name="content">ContentManager instance</param>
        public Character(String characterName, ContentManager content)
        {

            doc = System.Xml.Linq.XDocument.Load("Content/Sprites/Characters/" + characterName + "/" + characterName + ".xml");
            // Get the first (and only in this case) animation from the XML definition
            var stand = doc.Root.Element("stand");
            var walking = doc.Root.Element("walking");
            var jumping = doc.Root.Element("jumping");

            animationTexture_Stand = content.Load<Texture2D>(stand.Attribute("SheetName").Value);
            animationTexture_Walking = content.Load<Texture2D>(walking.Attribute("SheetName").Value);
            animationTexture_Jumping = content.Load<Texture2D>(jumping.Attribute("SheetName").Value);


            frameSize_Stand = new Point();
            frameSize_Stand.X = int.Parse(stand.Attribute("FrameWidth").Value, NumberStyles.Integer);
            frameSize_Stand.Y = int.Parse(stand.Attribute("FrameHeight").Value, NumberStyles.Integer);
            frameSize_Walking = new Point();
            frameSize_Walking.X = int.Parse(walking.Attribute("FrameWidth").Value, NumberStyles.Integer);
            frameSize_Walking.Y = int.Parse(walking.Attribute("FrameHeight").Value, NumberStyles.Integer);
            frameSize_Jumping = new Point();
            frameSize_Jumping.X = int.Parse(jumping.Attribute("FrameWidth").Value, NumberStyles.Integer);
            frameSize_Jumping.Y = int.Parse(jumping.Attribute("FrameHeight").Value, NumberStyles.Integer);


            sheetSize_Stand = new Point();
            sheetSize_Stand.X = int.Parse(stand.Attribute("SheetColumns").Value, NumberStyles.Integer);
            sheetSize_Stand.Y = int.Parse(stand.Attribute("SheetRows").Value, NumberStyles.Integer);
            sheetSize_Walking = new Point();
            sheetSize_Walking.X = int.Parse(walking.Attribute("SheetColumns").Value, NumberStyles.Integer);
            sheetSize_Walking.Y = int.Parse(walking.Attribute("SheetRows").Value, NumberStyles.Integer);
            sheetSize_Jumping = new Point();
            sheetSize_Jumping.X = int.Parse(jumping.Attribute("SheetColumns").Value, NumberStyles.Integer);
            sheetSize_Jumping.Y = int.Parse(jumping.Attribute("SheetRows").Value, NumberStyles.Integer);

            frameInterval_Stand = TimeSpan.FromSeconds(1.0f / int.Parse(stand.Attribute("Speed").Value, NumberStyles.Integer));
            frameInterval_Walking = TimeSpan.FromSeconds(1.0f / int.Parse(walking.Attribute("Speed").Value, NumberStyles.Integer));
            frameInterval_Jumping = TimeSpan.FromSeconds(1.0f / int.Parse(jumping.Attribute("Speed").Value, NumberStyles.Integer));
        }
        #endregion

        public bool Update_Stand(GameTime gameTime)
        {
            bool progressed;

            // Check is it is a time to progress to the next frame
            if (nextFrame_Stand >= frameInterval_Stand)
            {
                // Progress to the next frame in the row
                currentFrame_Stand.X++;
                // If reached end of the row advance to the next row 
                // and start form the first frame there
                if (currentFrame_Stand.X >= sheetSize_Stand.X)
                {
                    currentFrame_Stand.X = 0;
                    currentFrame_Stand.Y++;
                }
                // If reached last row in the frame sheet jump to the first row again - produce endless loop
                if (currentFrame_Stand.Y >= sheetSize_Stand.Y)
                    currentFrame_Stand.Y = 0;

                // Reset interval for next frame
                progressed = true;
                nextFrame_Stand = TimeSpan.Zero;
            }
            else
            {
                // Wait for the next frame 
                nextFrame_Stand += gameTime.ElapsedGameTime;
                progressed = false;
            }

            return progressed;
        }

        public bool Update_Walking(GameTime gameTime)
        {
            bool progressed;

            // Check is it is a time to progress to the next frame
            if (nextFrame_Walking >= frameInterval_Walking)
            {
                // Progress to the next frame in the row
                currentFrame_Walking.X++;
                // If reached end of the row advance to the next row 
                // and start form the first frame there
                if (currentFrame_Walking.X >= sheetSize_Walking.X)
                {
                    currentFrame_Walking.X = 0;
                    currentFrame_Walking.Y++;
                }
                // If reached last row in the frame sheet jump to the first row again - produce endless loop
                if (currentFrame_Walking.Y >= sheetSize_Walking.Y)
                    currentFrame_Walking.Y = 0;

                // Reset interval for next frame
                progressed = true;
                nextFrame_Walking = TimeSpan.Zero;
            }
            else
            {
                // Wait for the next frame 
                nextFrame_Walking += gameTime.ElapsedGameTime;
                progressed = false;
            }

            return progressed;
        }

        public bool Update_Jumping(GameTime gameTime)
        {
            bool progressed;

            // Check is it is a time to progress to the next frame
            if (nextFrame_Jumping >= frameInterval_Jumping)
            {
                // Progress to the next frame in the row
                currentFrame_Jumping.X++;
                // If reached end of the row advance to the next row 
                // and start form the first frame there
                if (currentFrame_Jumping.X >= sheetSize_Jumping.X)
                {
                    currentFrame_Jumping.X = 0;
                    currentFrame_Jumping.Y++;
                }
                // If reached last row in the frame sheet jump to the first row again - produce endless loop
                if (currentFrame_Jumping.Y >= sheetSize_Jumping.Y)
                    currentFrame_Jumping.Y = 0;

                // Reset interval for next frame
                progressed = true;
                nextFrame_Jumping = TimeSpan.Zero;
            }
            else
            {
                // Wait for the next frame 
                nextFrame_Jumping += gameTime.ElapsedGameTime;
                progressed = false;
            }

            return progressed;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffect, String action)
        {
            if (action == "stand")
                Draw_Stand(spriteBatch, position, 1.0f, spriteEffect);
            else if (action == "walking")
                Draw_Walking(spriteBatch, position, 1.0f, spriteEffect);
            else if (action == "jumping")
                Draw_Jumping(spriteBatch, position, 1.0f, spriteEffect);
        }

        /// <summary>
        /// Rendering of the animation
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch in which current frame will be rendered</param>
        /// <param name="position">The position of the current frame</param>
        /// <param name="scale">Scale factor to apply on the current frame</param>
        /// <param name="spriteEffect">SpriteEffect to apply on the current frame</param>
        public void Draw_Stand(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects spriteEffect)
        {
            Vector2 origin = new Vector2(animationTexture_Stand.Width / 2f, animationTexture_Stand.Height / 2f);
            spriteBatch.Draw(animationTexture_Stand, position - new Vector2(0,0), new Rectangle(
                  frameSize_Stand.X * currentFrame_Stand.X,
                  frameSize_Stand.Y * currentFrame_Stand.Y,
                  frameSize_Stand.X,
                  frameSize_Stand.Y),
                  Color.White, 0f, origin, scale, spriteEffect, 0);
        }

        public void Draw_Walking(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects spriteEffect)
        {
            Vector2 origin = new Vector2(animationTexture_Walking.Width / 2f, animationTexture_Walking.Height / 2f);
            spriteBatch.Draw(animationTexture_Walking, position - new Vector2(0,0), new Rectangle(
                  frameSize_Walking.X * currentFrame_Walking.X,
                  frameSize_Walking.Y * currentFrame_Walking.Y,
                  frameSize_Walking.X,
                  frameSize_Walking.Y),
                  Color.White, 0f, origin, scale, spriteEffect, 0);
        }

        public void Draw_Jumping(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects spriteEffect)
        {
            Vector2 origin = new Vector2(animationTexture_Jumping.Width / 2f, animationTexture_Jumping.Height / 2f);
            spriteBatch.Draw(animationTexture_Jumping, position - new Vector2(0,0), new Rectangle(
                  frameSize_Jumping.X * currentFrame_Jumping.X,
                  frameSize_Jumping.Y * currentFrame_Jumping.Y,
                  frameSize_Jumping.X,
                  frameSize_Jumping.Y),
                  Color.White, 0f, origin, scale, spriteEffect, 0);
        }


    }
}
