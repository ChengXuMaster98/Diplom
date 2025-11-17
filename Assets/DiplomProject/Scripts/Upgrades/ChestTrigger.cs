using UnityEngine;
using Zenject;

public class ChestTrigger : MonoBehaviour
{
    [SerializeField] private UpgradeUI _upgradeUIPrefab;
    private DiContainer _container;
    private bool _collected = false;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collected) return;
        if (!other.CompareTag("Player")) return;

        
        var ui = _container.InstantiatePrefabForComponent<UpgradeUI>(_upgradeUIPrefab, Vector3.zero, Quaternion.identity, null);
        ui.ShowRandomOptions(() =>
        {
            _collected = true;
            
            Destroy(gameObject);
        });
    }
}