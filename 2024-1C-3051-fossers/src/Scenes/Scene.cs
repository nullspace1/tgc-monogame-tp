using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSteel.Common;
using WarSteel.Entities;
using WarSteel.UIKit;


namespace WarSteel.Scenes;

public class Scene
{
    private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();
    protected GraphicsDeviceManager Graphics;
    protected SpriteBatch spriteBatch;
    protected Camera camera;
    private Dictionary<Type, ISceneProcessor> SceneProcessors = new Dictionary<Type, ISceneProcessor>();
    private List<UI> UIs = new List<UI>();


    public Scene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
    {
        Graphics = graphics;
        this.spriteBatch = spriteBatch;
    }

    public SpriteBatch GetSpriteBatch() => spriteBatch;

    public void SetCamera(Camera camera)
    {
        entities.Add(camera.Id, camera);
        this.camera = camera;
    }

    public Camera GetCamera()
    {
        return camera;
    }

    public GraphicsDeviceManager GetGraphicsDevice()
    {
        return Graphics;
    }

    public void AddSceneProcessor(ISceneProcessor p)
    {
        SceneProcessors.Add(p.GetType(), p);
    }

    public void AddEntityBeforeRun(Entity entity)
    {
        entities.Add(entity.Id, entity);
    }

    public void AddEntityDynamically(Entity entity)
    {
        entity.Initialize(this);
        entity.LoadContent();
        entities.Add(entity.Id, entity);
    }

    private void DeleteEntity(Entity entity)
    {
        entities.Remove(entity.Id);
        entity.OnDestroy(this);
    }

    public void AddUi(UI ui)
    {
        UIs.Add(ui);
    }

    public void RemoveUi(UI ui)
    {
        ui.toDestroy = true;
    }

    public T GetSceneProcessor<T>() where T : class, ISceneProcessor
    {
        return SceneProcessors.TryGetValue(typeof(T), out var processor) ? processor as T : default;
    }

    public List<Entity> GetEntities()
    {
        List<Entity> list = new List<Entity>();
        foreach (var e in entities.Values)
        {
            list.Add(e);
        }
        return list;
    }


    public Entity GetEntityByName(string name)
    {
        foreach (var entity in entities.Values)
        {
            if (entity.Name == name)
                return entity;
        }
        return null;
    }


    public virtual void Initialize()
    {
        foreach (var entity in entities.Values)
        {
            entity.Initialize(this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Initialize(this);
        }
    }

    public virtual void LoadContent()
    {
        foreach (var entity in entities.Values)
        {
            entity.LoadContent();
        }

    }

    public virtual void Draw()
    {

        UIs.RemoveAll(ui => ui.toDestroy);

        spriteBatch.Begin();
        foreach (var ui in UIs)
        {
            ui.Draw(this);
        }
        spriteBatch.End();

        foreach (var entity in entities.Values)
        {
            entity.Draw(this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Draw(this);
        }
    }

    public virtual void Update(GameTime gameTime)
    {

        foreach (var entity in entities.Values)
        {
            if (entity.ToDestroy)
            {
                DeleteEntity(entity);
            }
        }

        var copyEntities = new Dictionary<string, Entity>(entities);
        foreach (var entity in copyEntities.Values)
        {
            entity.Update(gameTime, this);
        }

        MouseState mouseState = Mouse.GetState();
        foreach (var ui in UIs)
        {
            ui.Update(gameTime, mouseState, this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Update(this, gameTime);
        }

    }

    public virtual void Unload()
    {

    }

}