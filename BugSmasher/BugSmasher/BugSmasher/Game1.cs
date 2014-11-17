using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BugSmasher
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D spritesheet, windows, buttons, background;
        Sprite hand, splat, selectionwindow, icecream, pizza, milkshake;
        Rectangle rec;
        List<Bug> bugs = new List<Bug>();
        Song music;
        Vector2 splatloc;
        int gameState = 0; // 0 = normal, 1 = paused, 2 = menu
        bool deadbug = false;
        Random a = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1024;
            graphics.IsFullScreen = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.Title = "Debuggers";

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            spritesheet = Content.Load<Texture2D>("spritesheet");
            windows = Content.Load<Texture2D>("windows");
            buttons = Content.Load<Texture2D>("buttons");
            background = Content.Load<Texture2D>("background-mac");
            music = Content.Load<Song>("music-default");
            MediaPlayer.Play(music);

            hand = new Sprite(Vector2.Zero, spritesheet, new Rectangle(135, 197, 48, 52), Vector2.Zero);
            splat = new Sprite(splatloc, spritesheet, new Rectangle(0, 132, 128, 128), Vector2.Zero);
            selectionwindow = new Sprite(new Vector2(Window.ClientBounds.Width - 600, Window.ClientBounds.Height - 300), windows, new Rectangle(64, 41, 778, 377), Vector2.Zero);
            icecream = new Sprite(new Vector2(selectionwindow.Center.X - 110, selectionwindow.Center.Y - 10), spritesheet, new Rectangle(0, 259, 32, 37), Vector2.Zero);
            pizza = new Sprite(new Vector2(selectionwindow.Center.X - 20, selectionwindow.Center.Y - 10), spritesheet, new Rectangle(32, 259, 32, 37), Vector2.Zero);
            milkshake = new Sprite(new Vector2(selectionwindow.Center.X + 70, selectionwindow.Center.Y - 10), spritesheet, new Rectangle(64, 259, 32, 37), Vector2.Zero);
            int ai = a.Next(0, this.Window.ClientBounds.Height - 64);
            SpawnBug(new Vector2(0, ai), new Vector2(200, 0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void SpawnBug(Vector2 location, Vector2 velocity)
        {
            Random typeOfBug = new Random();
            int typeOfBugInt = typeOfBug.Next(0, 6);
            switch (typeOfBugInt)
            {
                case 0:
                    rec = new Rectangle(0, 0, 64, 64);
                    break;
                case 1:
                    rec = new Rectangle(64, 0, 64, 64);
                    break;
                case 2:
                    rec = new Rectangle(128, 0, 64, 64);
                    break;
                case 3:
                    rec = new Rectangle(0, 64, 64, 64);
                    break;
                case 4:
                    rec = new Rectangle(64, 64, 64, 64);
                    break;
                case 5:
                    rec = new Rectangle(128, 64, 64, 64);
                    break;
            }
            Bug bug = new Bug(location, spritesheet, rec, velocity);
            bugs.Add(bug);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState ms = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();

            // TODO: Add your update logic here
            hand.Location = new Vector2(ms.X, ms.Y);

            Vector2 target = Vector2.Zero;

            if (ms.LeftButton == ButtonState.Pressed)
                target = new Vector2(ms.X, ms.Y);

            for (int i = 0; i < bugs.Count; i++)
            {
                // Bug logic goes here...
                // bugs[i].FlipHorizontal = false;
                Random dirx = new Random();
                int dirxi = dirx.Next(50, 200);
                Random diry = new Random();
                int diryi = diry.Next(-60, 60);

                bugs[i].Update(gameTime);
                bugs[i].Target = target;

                if (bugs[i].BoundingBoxRect.Intersects(pizza.BoundingBoxRect))
                {
                    pizza.Location = new Vector2(-300, -300);
                    bugs[i].TintColor = Color.LightGreen;
                    bugs[i].Velocity *= new Vector2(4, 4);
                }

                if (bugs[i].BoundingBoxRect.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed)
                {
                    int ai = a.Next(0, Window.ClientBounds.Height - 64);
                    int bi = a.Next(0, Window.ClientBounds.Height - 64);

                    SpawnBug(new Vector2(0, ai), new Vector2(dirxi, diryi));
                    if (bi != ai) SpawnBug(new Vector2(0, bi), new Vector2(dirxi, diryi));

                    splatloc = new Vector2(bugs[i].Location.X, bugs[i].Location.Y);
                    deadbug = true;
                    bugs.RemoveAt(i); // placeholder for splat code
                    continue;
                }
            }

            if (gameTime.IsRunningSlowly)
            {
                for (int b = 20; b < bugs.Count; b++)
                    bugs.RemoveAt(b);
            }

            if (hand.BoundingBoxRect.Intersects(icecream.BoundingBoxRect))
            {
                icecream.TintColor = Color.Yellow;
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    hand.Location = new Vector2(-300, -300);
                    icecream.Location = new Vector2(ms.X, ms.Y);
                }
                icecream.RelativeSize = 0.5f;
            }
            else
            {
                icecream.TintColor = Color.White;
                icecream.RelativeSize = 1;
            }

            if (hand.BoundingBoxRect.Intersects(milkshake.BoundingBoxRect))
            {
                milkshake.TintColor = Color.Yellow;
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    hand.Location = new Vector2(-300, -300);
                    milkshake.Location = new Vector2(ms.X, ms.Y);
                }
            }
            else milkshake.TintColor = Color.White;

            if (hand.BoundingBoxRect.Intersects(pizza.BoundingBoxRect))
            {
                pizza.TintColor = Color.Yellow;
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    hand.Location = new Vector2(-300, -300);
                    pizza.Location = new Vector2(ms.X, ms.Y);
                }
            }
            else pizza.TintColor = Color.White;

            if (!pizza.BoundingBoxRect.Intersects(selectionwindow.BoundingBoxRect) && !icecream.BoundingBoxRect.Intersects(selectionwindow.BoundingBoxRect)
                && !milkshake.BoundingBoxRect.Intersects(selectionwindow.BoundingBoxRect)) selectionwindow.Location = new Vector2(-300, -300);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkRed);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            for (int i = 0; i < bugs.Count; i++)
            {
                bugs[i].Draw(spriteBatch);

                MouseState ms = Mouse.GetState();

                if (bugs[i].BoundingBoxRect.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed)
                    splat.Draw(spriteBatch);

                if (deadbug)
                    splat.Draw(spriteBatch);
            }
            selectionwindow.RelativeSize = 0.4f;
            selectionwindow.Draw(spriteBatch);
            icecream.RelativeSize = 2;
            icecream.Draw(spriteBatch);
            pizza.RelativeSize = 2;
            pizza.Draw(spriteBatch);
            milkshake.RelativeSize = 2;
            milkshake.Draw(spriteBatch);
            hand.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
