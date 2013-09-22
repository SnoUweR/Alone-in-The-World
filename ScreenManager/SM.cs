using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AITW.ScreenManager
{
    internal static class SM
    {
        static List<Screen> screens = new List<Screen>();
        static bool isStarted = false;
        static Screen prevScreen = null;
        internal static Screen ActiveScreen = null;

        internal static void AddScreen(Screen screen)
        {
            foreach (Screen scr in screens)
                if (scr.Name == screen.Name)
                    return;

            screens.Add(screen);
        }

        internal static int GetScreensCount()
        {
            return screens.Count;
        }

        internal static Screen GetScreenByIndex(int index)
        {
            return screens[index];
        }

        internal static Screen GetScreenByName(string name)
        {
            foreach (Screen scr in screens)
                if (scr.Name == name)
                    return scr;

            return null;
        }

        internal static void ActivateScreen(Screen screen)
        {
            prevScreen = ActiveScreen;

            if (ActiveScreen != null)
                ActiveScreen.Remove();

            ActiveScreen = screen;

            if (isStarted)
                ActiveScreen.Initialize();
        }

        internal static void ActivateScreenByIndex(int index)
        {
            prevScreen = ActiveScreen;

            if (ActiveScreen != null)
                ActiveScreen.Remove();

            ActiveScreen = GetScreenByIndex(index);

            if (isStarted)
                ActiveScreen.Initialize();
        }

        internal static void ActivateScreenByName(string name)
        {
            prevScreen = ActiveScreen;

            ActiveScreen = GetScreenByName(name);

            if (isStarted)
                ActiveScreen.Initialize();
        }

        internal static void ActivatePreviousScreen()
        {
            if (prevScreen != null)
                ActivateScreenByName(prevScreen.Name);
        }

        internal static void Initialize()
        {
            isStarted = true;

            if (ActiveScreen != null)
                ActiveScreen.Initialize();
        }

        internal static void LoadContent()
        {
            if (!isStarted)
                return;

            if (ActiveScreen != null)
                ActiveScreen.LoadContent();
        }

        internal static void Update(GameTime gameTime)
        {
            if (!isStarted)
                return;

            if (ActiveScreen != null)
                ActiveScreen.Update(gameTime);
        }

        internal static void Draw(GameTime gameTime)
        {
            if (!isStarted)
                return;

            if (ActiveScreen != null)
                ActiveScreen.Draw(gameTime);
        }

    }
}