using Zenject;

public class EnemyVFXBinder : IInitializable
{
    private readonly Enemy _enemy;
    private readonly EnemyVFXController _vfx;

    public EnemyVFXBinder(Enemy enemy, EnemyVFXController vfx)
    {
        _enemy = enemy;
        _vfx = vfx;
    }

    public void Initialize()
    {
        _vfx.Construct(_enemy);
    }
}