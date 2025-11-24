
using UnityEngine;

public interface IWeapon
{
    WeaponData Data { get; }

    void Attack(IEnemy enemy);

    void SetTip(Transform tip);
}