using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Managers.Gizmos;
using WarSteel.Scenes;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace WarSteel.Entities;

public abstract class RigidBody : IComponent
{
    private Transform _transform;
    private GameObject _entity;
    protected Collider _collider;
    public Vector3 Offset;

    public Collider Collider
    {
        get => _collider;
    }

    public Transform Transform
    {
        get => _transform;
    }

    public GameObject Entity
    {
        get => _entity;
    }

    public RigidBody(Collider collider, Vector3 offset)
    {
        _collider = collider;
        Offset = offset;
    }

    public virtual void OnStart(GameObject self, Scene scene)
    {
        _entity = self;
        _transform = self.Transform;
        PhysicsProcessor processor = scene.GetSceneProcessor<PhysicsProcessor>();
        processor?.AddBody(this);
    }

    public virtual void LoadContent(GameObject self) { }

    public virtual void OnUpdate(GameObject self, GameTime gameTime, Scene scene)
    {
    }

    public virtual void DrawGizmos(Gizmos gizmos)
    {
        _collider.ColliderShape.DrawGizmos(_transform.Position + Vector3.Transform(Offset,Matrix.CreateFromQuaternion(Transform.Orientation)), gizmos);
    }

    public void Destroy(GameObject self, Scene scene)
    {
        PhysicsProcessor processor = scene.GetSceneProcessor<PhysicsProcessor>();
        RemoveSelf(processor);
    }

    public abstract void Build(PhysicsProcessor processor);

    public abstract void RemoveSelf(PhysicsProcessor processor);
}


public class StaticBody : RigidBody
{

    public StaticHandle Handle;
    public StaticBody(Collider collider, Vector3 offset) : base(collider, offset) { }

    public override void Build(PhysicsProcessor processor)
    {
        TypedIndex index = processor.AddShape(_collider);
        Vector3 position = Transform.Position + Offset;
        StaticDescription staticDescription = new(new System.Numerics.Vector3(position.X, position.Y, position.Z),new System.Numerics.Quaternion(Transform.Orientation.X,Transform.Orientation.Y,Transform.Orientation.Z,Transform.Orientation.W),index);
        processor.AddStatic(this, staticDescription);
    }

    public override void RemoveSelf(PhysicsProcessor processor)
    {
        processor.RemoveStaticBody(this);
    }

}

public class DynamicBody : RigidBody
{
    public BodyHandle Handle;
    private Vector3 _velocity;
    private Vector3 _angularVelocity;
    private float _mass;

    private float _dragCoeff = 0.2f;

    private float _angularDragCoeff = 0.3f;

    public Vector3 Forces = Vector3.Zero;

    public Vector3 Torques = Vector3.Zero;

    public Vector3 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    public Vector3 AngularVelocity
    {
        get => _angularVelocity;
        set => _angularVelocity = value;
    }

    public float Mass
    {
        get => _mass;
    }

    public float AngularDrag
    {
        get => _angularDragCoeff;
    }

    public float Drag
    {
        get => _dragCoeff;
    }

    public Vector3 Force
    {
        get => Forces;
    }

    public Vector3 Torque
    {
        get => Torques;
    }

    public DynamicBody(Collider collider, Vector3 offset, float mass, float dragCoeff, float angularDragCoeff) : base(collider, offset)
    {
        _mass = mass;
        _dragCoeff = dragCoeff;
        _angularDragCoeff = angularDragCoeff;
        _velocity = Vector3.Zero;
        _angularVelocity = Vector3.Zero;
    }

    public override void OnUpdate(GameObject self, GameTime time, Scene scene)
    {
        PhysicsProcessor processor = scene.GetSceneProcessor<PhysicsProcessor>();

        BodyReference desc = processor.Simulation.Bodies[Handle];

        Transform.Position = desc.Pose.Position - Vector3.Transform(Offset,Matrix.CreateFromQuaternion(Transform.Orientation));
        Transform.Orientation = desc.Pose.Orientation;
        Velocity = desc.Velocity.Linear;
        AngularVelocity = desc.Velocity.Angular;

        base.OnUpdate(self, time, scene);
    }

    public void ApplyForce(Vector3 force)
    {
        Forces += force;
    }

    public void ApplyTorque(Vector3 torque)
    {
        Torques += torque;
    }


    public override void Build(PhysicsProcessor processor)
    {
        TypedIndex index = processor.AddShape(_collider);

        Vector3 position = Transform.Position;

        BodyDescription bodyDescription = BodyDescription.CreateDynamic(

            new System.Numerics.Vector3(position.X, position.Y, position.Z),
            _collider.ColliderShape.GetInertia(this),
            new CollidableDescription(index, 0.01f),

            new BodyActivityDescription(1000f)
        );

        bodyDescription.Velocity.Linear = new(_velocity.X, _velocity.Y, _velocity.Z);
        bodyDescription.Velocity.Angular = new(_angularVelocity.X, _angularVelocity.Y, _angularVelocity.Z);

        bodyDescription.Velocity.Linear = new(_velocity.X, _velocity.Y, _velocity.Z);
        bodyDescription.Velocity.Angular = new(_angularVelocity.X, _angularVelocity.Y, _angularVelocity.Z);

        processor.AddDynamic(this, bodyDescription);
    }

    public override void RemoveSelf(PhysicsProcessor processor)
    {
        processor.RemoveDynamicBody(this);
    }


}







