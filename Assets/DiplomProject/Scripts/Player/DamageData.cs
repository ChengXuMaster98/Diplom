using UnityEngine;

public class DamageData
{
    public int Amount;
    public bool IsBlocked;
    public Transform Source;

    public DamageData(int amount)
    {
        Amount = amount;
        IsBlocked = false;
    }
}