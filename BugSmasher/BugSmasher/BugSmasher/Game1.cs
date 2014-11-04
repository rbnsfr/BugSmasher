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
        Sprite hand;
        Rectangle rec;
        List<Sprite> bugs = new List<Sprite>();
        Song music;
        int mood = 0; // 0 = normal, 1 = relaxed, 2 = angry, 3 = intrigued

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1024;
            graphics.IsFullScreen = true;
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
            Window.Title = "Super Smash Bugs";

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
            music = Content.Load<Song>("music");
            MediaPlayer.Play(music);

            Random a = new Random();
            int ai = a.Next(0, this.Window.ClientBounds.Height - 64);

            hand = new Sprite(Vector2.Zero, spritesheet, new Rectangle(135, 197, 48, 52), Vector2.Zero);
            SpawnBug(new Vector2(0, ai), new Vector2(150, 50));
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
            Random tokyo = new Random();
            int tokyoi = tokyo.Next(0, 6);
            switch (tokyoi)
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
            Sprite bug = new Sprite(location, spritesheet, rec, velocity);
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

            for (int i = 0; i < bugs.Count; i++)
            {
                // Bug logic goes here...
                // bugs[i].FlipHorizontal = false;

                bugs[i].Update(gameTime);

                if (bugs[i].BoundingBoxRect.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed)
                {
                    bugs.RemoveAt(i); // placeholder for splat code

                    Random a = new Random();
                    int ai = a.Next(0, Window.ClientBounds.Height - 64);

                    SpawnBug(new Vector2(0, ai), new Vector2(150, -50));
                    SpawnBug(new Vector2(0, ai), new Vector2(150, 50));
                }

                if (gameTime.IsRunningSlowly)
                {
                    if (bugs.Count > 1)
                        bugs.RemoveAt(i);
                }
            }

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
                bugs[i].Draw(spriteBatch);

            hand.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
