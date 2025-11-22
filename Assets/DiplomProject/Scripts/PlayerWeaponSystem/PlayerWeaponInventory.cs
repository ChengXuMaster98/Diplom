using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponInventory
{
    private readonly WeaponFactory _factory;

    public IWeapon[] Slots = new IWeapon[3];
    public int ActiveSlot = 0;

    public PlayerWeaponInventory(WeaponFactory factory)
    {
        _factory = factory;
    }

    public bool TryAddWeapon(IWeapon weapon)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i] == null)
            {
                Slots[i] = weapon;
                return true;
            }
        }
        return false;
    }

    public IWeapon GetActiveWeapon()
    {
        return Slots[ActiveSlot];
    }

    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
            Slots[i] = null;

        ActiveSlot = 0;
    }
}