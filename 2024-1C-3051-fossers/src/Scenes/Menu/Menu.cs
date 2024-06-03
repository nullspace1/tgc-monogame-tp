
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.UIKit;
using WarSteel.Utils;


namespace WarSteel.Scenes.Main;

public class MenuScene : Scene
{
    public MenuScene(GraphicsDeviceManager Graphics, SpriteBatch SpriteBatch) : base(Graphics, SpriteBatch)
    {

    }

    public override void Initialize()
    {
        UIProcessor uiProcessor = new UIProcessor();
        AddSceneProcessor(uiProcessor);
        Vector2 screenCenter = Screen.GetScreenCenter(Graphics);

        Func<int, Vector3> GetButtonPos = (int pos) =>
        {
            Vector2 screenCenter = Screen.GetScreenCenter(Graphics);
            int margin = 90;
            return new Vector3(screenCenter.X, screenCenter.Y - 50 + margin * pos, 0);
        };

        // ui elems
        UI background = new UI(() => new Vector3(screenCenter.X, screenCenter.Y, 0), Screen.GetScreenWidth(Graphics), Screen.GetScreenHeight(Graphics), new Image("menuBg", Color.White), new List<UIAction>());
        UI header = new UI(() => new Vector3(screenCenter.X, screenCenter.Y - 160, 0), 0, 0, new Header("WARSTEEL", Color.White), new List<UIAction>());
        UI startBtn = new UI(() => GetButtonPos(0), 300, 60, new Button(ContentRepoManager.Instance().GetTexture("UI/button"), "Start"), new List<UIAction>()
        {
            (scene, ui) => {SceneManager.Instance().SetCurrentScene(ScenesNames.MAIN);},
        });
        UI controlsBtn = new UI(() => GetButtonPos(1), 300, 60, new Button(ContentRepoManager.Instance().GetTexture("UI/button"), "Controls"), new List<UIAction>());
        UI exitBtn = new UI(() => GetButtonPos(2), 300, 60, new Button(ContentRepoManager.Instance().GetTexture("UI/button"), "Exit"), new List<UIAction>());

        // add elems
        uiProcessor.AddUi(background);
        uiProcessor.AddUi(header);
        uiProcessor.AddUi(startBtn);
        uiProcessor.AddUi(controlsBtn);
        uiProcessor.AddUi(exitBtn);


        base.Initialize();
    }


    public override void Draw()
    {
        base.Draw();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}