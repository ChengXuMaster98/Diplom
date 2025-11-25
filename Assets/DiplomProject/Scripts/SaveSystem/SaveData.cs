using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int Version = 1;

    // Player transform
    public float PlayerPosX;
    public float PlayerPosY;
    public float PlayerPosZ;
    public float PlayerRotY;

    // Player state
    public int CurrentHealth;
    public float CurrentStamina;

    // Player weapon
    public WeaponType[] WeaponSlots = new WeaponType[3];
    public int ActiveWeaponSlot;

    // Runtime upgrades (multipliers)
    public float HealthMultiplier;
    public float DamageMultiplier;
    public float SpeedMultiplier;
    public float StaminaMultiplier;

    public List<string> DeadEnemies = new();

    public List<string> CollectedPickups = new List<string>();
}