using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Entities;
using WarSteel.Entities.Map;
using WarSteel.Managers;
using WarSteel.Scenes;
using Vector3 = Microsoft.Xna.Framework.Vector3;

class MapFactory
{

    private Scene _scene;
    private string GROUND = "ground";

    public MapFactory(Scene scene)
    {
        _scene = scene;
    }

    public Player PlayerTank(Vector3 position)
    {
        return new Player(_scene, position);
    }

    public GameObject Tree(Vector3 position)
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Trees/1");
        Renderer renderer = new(Color.Brown);
        GameObject tree = new(new string[] { GROUND }, new Transform(), model, renderer);
        tree.Transform.Dimensions = tree.Transform.Dimensions;
        tree.AddComponent(new StaticBody(new Collider(new ConvexShape(model), (c) => { }), Vector3.Zero));
        tree.Transform.Position = position - Vector3.Up * 50;
        return tree;
    }

    public GameObject Rock(Vector3 position, RockSize size)
    {
        return RockFactory.Generate(new string[] { GROUND }, position, size);
    }

    public GameObject Bush(Vector3 position)
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Bush");
        Renderer renderer = new(Color.Red);
        GameObject bush = new(new string[] { GROUND }, new Transform(), model, renderer);
        bush.Transform.Position = position;
        return bush;
    }

    public GameObject Ground(Vector3 position)
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Ground");
        Renderer renderer = new(Color.DarkGray);
        GameObject ground = new(new string[] { "SURFACE" }, new Transform(), model, renderer, true);
        ground.Transform.Position = position;
        ground.AddComponent(new StaticBody(new Collider(new BoxShape(100, 100000, 100000), (e) => { }), Vector3.Up * 70));
        return ground;
    }

}