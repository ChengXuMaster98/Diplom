using UnityEngine;
using Zenject;

public class AttackHitBox : MonoBehaviour
{
    private bool _canHit = false;

    private IWeapon _weapon;

    public void SetOwnerWeapon(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public void EnableHitbox() => _canHit = true;
    public void DisableHitbox() => _canHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_canHit || _weapon == null) return;

        if (other.TryGetComponent<IEnemy>(out var enemy))
        {
            _weapon.Attack(enemy);    // теперь урон идёт через конкретное оружие
            Debug.Log($"Hit enemy using {_weapon.Data.WeaponName}");
            var soundCtrl = GetComponentInParent<WeaponSoundController>();
            if (soundCtrl != null && _weapon.Data.SoundData != null)
            {
                soundCtrl.PlayHit(_weapon.Data.SoundData);
            }

            _canHit = false;
        }
    }
}