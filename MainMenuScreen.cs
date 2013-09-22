using System;
using System.Collections.Generic;
using AITW.ScreenManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace AITW
{
    internal class MainMenuScreen : Screen
    {

        //TODO: Реализовать нормальное меню

        internal MainMenuScreen()
            : base("MainMenuScreen")
        {
        }

        internal override bool Initialize()
        {
            return base.Initialize();
        }

        internal override void Remove()
        {
            base.Remove();
        }

        internal override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space))
            {
                SM.ActivateScreenByName("Level");
                SM.LoadContent();
            }
            base.Update(gameTime);
        }

        internal override void LoadContent()
        {
            base.LoadContent();
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}