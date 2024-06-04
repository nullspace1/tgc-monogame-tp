using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public interface IUIRenderable
{
    public void Draw(Scene scene, UI ui);
}

public class UI
{
    public bool toDestroy = false;
    public Vector3 Position;
    public float Height;
    public float Width;

    private IUIRenderable _renderable;
    private List<UIAction> _actions;

    public UI(Vector3 position, float width, float height, IUIRenderable renderable, List<UIAction> actions)
    {
        _renderable = renderable;
        _actions = actions;
        Position = position;
        Height = height;
        Width = width;
    }


    public UI(Vector3 position, float width, float height, IUIRenderable renderable)
    {
        _renderable = renderable;
        Position = position;
        Height = height;
        Width = width;
        _actions = new List<UIAction>();
    }

    public UI(Vector3 position, IUIRenderable renderable)
    {
        _renderable = renderable;
        _actions = new List<UIAction>();
        Position = position;
        Height = 0;
        Width = 0;
    }


    public void Draw(Scene scene)
    {
        _renderable.Draw(scene, this);
    }

    public bool IsBeingClicked(Vector2 mousePosition)
    {
        Vector3 position = Position;

        return mousePosition.X <= position.X + Width / 2
               && mousePosition.X >= position.X - Width / 2
               && mousePosition.Y <= position.Y + Height / 2
               && mousePosition.Y >= position.Y - Height / 2;
    }

    public void OnClick(Scene scene)
    {
        _actions.ForEach(action => action.Invoke(scene, this));
    }

    public virtual void Update(Scene scene) { }
}