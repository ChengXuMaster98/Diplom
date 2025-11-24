using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket;

    private PlayerWeaponInventory _inventory;
    private Animator _animator;
    private AttackHitBox _hitbox;

    private AttackAnimationEventReceiver _attackEventReceiver;
    private WeaponSoundController _soundController;

    private GameObject _currentWeaponModel;


    [Inject]
    public void Construct(PlayerWeaponInventory inventory, SaveService save)
    {
        _inventory = inventory;
        save.OnLoadFinished += () => EquipSlot(_inventory.ActiveSlot);
    }

    private IEnumerator Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _attackEventReceiver = GetComponentInChildren<AttackAnimationEventReceiver>();
        _soundController = GetComponentInChildren<WeaponSoundController>();

        yield return null;

        EquipSlot(_inventory.ActiveSlot);

        if (_attackEventReceiver != null)
            _attackEventReceiver.OnSwing += PlaySwingSound;
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

    private void PlaySwingSound()
    {
        var weapon = _inventory.GetActiveWeapon();
        if (weapon == null) return;

        _soundController.PlayLightAttack(weapon.Data.SoundData);
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

    public void RefreshEquippedWeapon()
    {
        EquipSlot(_inventory.ActiveSlot);
    }

    private void SpawnWeaponModel(IWeapon weapon)
    {
        RemoveWeaponModel();

        _currentWeaponModel = Instantiate(
            weapon.Data.WeaponPrefab,
            weaponSocket
        );

        _inventory.GetActiveWeapon().SetTip(
    _currentWeaponModel.transform.Find("TipPoint"));

        _currentWeaponModel.transform.localPosition = weapon.Data.PositionOffset;
        _currentWeaponModel.transform.localEulerAngles = weapon.Data.RotationOffset;


        // захватываем HitBox внутри нового оружия
        _hitbox = _currentWeaponModel.GetComponentInChildren<AttackHitBox>();
        _hitbox.SetOwnerWeapon(weapon);

        // 2) передаём этот хитбокс обработчику анимации
        _attackEventReceiver.SetHitBox(_hitbox);

        Debug.Log($"[Weapon] Equipped: {weapon.Data.WeaponName}");
    }
}