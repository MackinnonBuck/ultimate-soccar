using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Components;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using FarseerPhysics.Controllers;
using MonoEngine.ResourceManagement;

namespace UltimateSocCar.Scenes
{
    public class GameScene : Scene
    {
        GameObject car;

        public GameScene(string mapPath) : base(mapPath)
        {
        }

        protected override void OnInitialize()
        {
            Debug.Clear();
            DebugDrawEnabled = true;

            TextureManager.Instance.Load("Textures/Car", "Car");
            TextureManager.Instance.Load("Textures/Wheel", "Wheel");

            car = GameObject.Get(null, "MainCar");

            Body groundBody = GameObject.Get(null, "Ground").GetComponent<BodyComponent>().Body;
            groundBody.Friction = 1.0f;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Input.Instance.GamePads[0].IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Start))
                App.Instance.ChangeScene(new GameScene("test_arena_0"));

            Camera.Position = car.Position - Camera.Origin;
            Camera.Scale *= new Vector2(1.0f + Input.Instance.GamePads[0].ThumbSticks.Right.Y * 0.025f);
        }

        protected override void OnPreDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            App.Instance.GraphicsDevice.Clear(Color.DarkSlateGray);
        }
    }
}
