﻿using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MonoEngine.Core
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class App : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// The Game's active Scene.
        /// </summary>
        public Scene Scene { get; private set; }

        Scene futureScene;

        static App _instance;

        /// <summary>
        /// The global App instance.
        /// </summary>
        public static App Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new App();

                return _instance;
            }
        }

        /// <summary>
        /// Initializes the instance to prepare it for first use.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mainScene"></param>
        public void Init(string title, int width, int height, Scene mainScene, bool isFullScreen = false, double frameRate = 60.0)
        {
            IsMouseVisible = true;

            Content.RootDirectory = "Content";

            Window.Title = title;
            
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = isFullScreen;

            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / frameRate);

            Settings.MaxPolygonVertices = 32;       

            ChangeScene(mainScene);
        }

        /// <summary>
        /// Polls a transition to a new scene.
        /// </summary>
        /// <param name="scene"></param>
        public void ChangeScene(Scene scene)
        {
            futureScene = scene;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.Instance.Refresh();

            if (futureScene != null)
            {
                Scene?.Destroy();

                Scene = futureScene;
                futureScene = null;

                Scene.Initialize();
            }
            
            Scene.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Scene.Draw(spriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
