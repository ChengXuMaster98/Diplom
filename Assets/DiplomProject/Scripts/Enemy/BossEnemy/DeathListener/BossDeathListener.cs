using UnityEngine;
using Zenject;

public class BossDeathListener : MonoBehaviour
{
    private Enemy _enemy;
    private IFinalBoss _bossTag;
    private GameWonUI _gameWonUI;

    [Inject]
    public void Construct(GameWonUI gameWonUI)
    {
        _gameWonUI = gameWonUI;
    }

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _bossTag = GetComponent<IFinalBoss>();

        
        if (_bossTag == null)
            return;
    }

}