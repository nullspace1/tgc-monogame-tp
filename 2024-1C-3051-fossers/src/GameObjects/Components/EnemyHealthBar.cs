using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Scenes;
using WarSteel.Utils;

public class EnemyHealthBar : IComponent
{
    private HealthBar _healthBar;
    private Enemy _self;

    public EnemyHealthBar()
    {
    }

    public void OnStart(GameObject self, Scene scene)
    {
        Enemy enemy = (Enemy)self;
        _self = enemy;
        _healthBar = new HealthBar(scene, new Vector2(enemy.Transform.AbsolutePosition.X, enemy.Transform.AbsolutePosition.Y + 100), 30, 10);
    }

    public void OnUpdate(GameObject self, GameTime gameTime, Scene scene)
    {
        CalculateHealthPos(self.Transform.AbsolutePosition, scene);
    }


    public void Destroy(GameObject self, Scene scene)
    {
        _healthBar.Destroy();
    }

    private Vector3 GetEnemyScreenPos(Vector3 absolutePos, Scene scene)
    {
        Vector3 screenPosition = scene.GraphicsDeviceManager.GraphicsDevice.Viewport.Project(absolutePos, scene.Camera.Projection, scene.Camera.View, Matrix.Identity);
        return screenPosition;
    }

    public void CalculateHealthPos(Vector3 absolutePos, Scene scene)
    {
        Vector3 screenPosition = GetEnemyScreenPos(absolutePos, scene);
        if (IsEnemyInView(scene, absolutePos, screenPosition) && _self.Health < 100)
        {
            Vector2 offset = new(0, -20);
            Vector2 newPosition = new Vector2(screenPosition.X, screenPosition.Y) + offset;
            _healthBar.SetPosition(newPosition);
            _healthBar.SetVisibility(true);
        }
        else
        {
            _healthBar.SetVisibility(false);
        }
    }

    private bool IsEnemyInView(Scene scene, Vector3 absolutePos, Vector3 screenPosition)
    {
        int screenWidth = Screen.GetScreenWidth(scene.GraphicsDeviceManager);
        int screenHeight = Screen.GetScreenHeight(scene.GraphicsDeviceManager);
        bool isVisible = screenPosition.Z > 0
                         && screenPosition.X >= 0 && screenPosition.X <= screenWidth
                         && screenPosition.Y >= 0 && screenPosition.Y <= screenHeight;

        Vector3 cameraForward = scene.Camera.Transform.Forward;
        Vector3 vectorToEnemy = absolutePos - scene.Camera.Transform.AbsolutePosition;
        float dotProduct = Vector3.Dot(cameraForward, vectorToEnemy);
        isVisible = isVisible && dotProduct > 0;

        return isVisible;
    }


    public void SetHealth(float health)
    {
        _healthBar.SetHealth(health);
    }
}