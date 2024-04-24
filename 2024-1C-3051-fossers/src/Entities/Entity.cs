using System;
using Microsoft.Xna.Framework;
using WarSteel.Common;

namespace WarSteel.Entities;

public class Entity
{
    // identifiers
    public string Id { get; }
    public string Name { get; }
    public string[] Tags { get; }

    public Transform Transform { get; }
    protected Renderable _renderable { get; set; }

    public Entity(string name, string[] tags, Transform transform)
    {
        // creates a random unique identifier
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
        _renderable = null;
    }

    public virtual void Initialize() { }
    public virtual void LoadContent() { }
    public virtual void Draw(Camera camera)
    {
        if (_renderable != null)
            _renderable.Draw(Transform.World, camera);
    }
    public virtual void Update(GameTime gameTime)
    {
        Transform.UpdateWorldMatrix();
    }
    public virtual void OnDestroy() { }
}