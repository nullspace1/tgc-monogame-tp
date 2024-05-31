using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Entities;


class SimpleTree : Entity
{
    public SimpleTree() : base("simple-tree", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
    }

    public override void Initialize(Scene scene)
    {
        AddComponent(new StaticBody(Transform, new Collider(new BoxShape(200, 800, 200),new NoAction())));
        base.Initialize(scene);
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/SimpleTree");
        _renderable = new Renderable(model);
        _renderable.AddShader("color",new PhongShader(0.5f,0.5f,Color.Brown));

        base.LoadContent();
    }
}