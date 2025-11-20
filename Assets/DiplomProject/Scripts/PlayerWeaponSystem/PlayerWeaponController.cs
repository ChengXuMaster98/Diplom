using UnityEngine;
using Zenject;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket;

    private PlayerWeaponInventory _inventory;
    private Animator _animator;
    private AttackHitBox _hitbox;

    private GameObject _currentWeaponModel;

    [Inject]
    public void Construct(PlayerWeaponInventory inventory)
    {
        _inventory = inventory;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _hitbox = GetComponentInChildren<AttackHitBox>();

        EquipSlot(_inventory.ActiveSlot);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipSlot(2);

        if (Input.GetMouseButtonDown(0))
        {
            var weapon = _inventory.GetActiveWeapon();
            if (weapon == null) return;

            _animator.SetTrigger(weapon.Data.AttackTriggerName);
        }
    }

    public void EquipSlot(int index)
    {
        _inventory.ActiveSlot = index;

        var weapon = _inventory.GetActiveWeapon();
        if (weapon == null)
        {
            RemoveWeaponModel();
            return;
        }

        SpawnWeaponModel(weapon);
    }

    private void RemoveWeaponModel()
    {
        if (_currentWeaponModel != null)
            Destroy(_currentWeaponModel);

        _currentWeaponModel = null;
        _hitbox = null;
    }

    private void SpawnWeaponModel(IWeapon weapon)
    {
        RemoveWeaponModel();

        _currentWeaponModel = Instantiate(
            weapon.Data.WeaponPrefab,
            weaponSocket
        );

        _currentWeaponModel.transform.localPosition = Vector3.zero;
        _currentWeaponModel.transform.localRotation = Quaternion.identity;

        // захватываем HitBox внутри нового оружия
        _hitbox = _currentWeaponModel.GetComponentInChildren<AttackHitBox>();
        _hitbox.SetOwnerWeapon(weapon);

        Debug.Log($"[Weapon] Equipped: {weapon.Data.WeaponName}");
    }
}