using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class SaveService : ISaveService
{
    private const string FileName = "save.json";

    private readonly Player _player;
    private readonly PlayerHealth _health;
    private readonly IStaminaSystem _stamina;
    private readonly IUpgradeService _upgrades;
    private readonly DiContainer _container; // А зачем мне здесь вообще контейнер? Нигде не реализую вроде, потом посмотреть.
    private readonly PlayerWeaponInventory _inventory;
    private readonly WeaponFactory _factory;
    private readonly WeaponDatabase _weaponDatabase;
    private readonly EnemySaveSystem _enemySave;
    private readonly PickupSaveSystem _pickupSave;
    // Для автосохранения / отладки можно добавить флаг
    private readonly string _path;


    public event Action OnLoadFinished;

    [Inject]
    public SaveService(Player player, PlayerHealth health, IStaminaSystem stamina, IUpgradeService upgrades, DiContainer container, EnemySaveSystem enemySave,
        PlayerWeaponInventory inventory, WeaponFactory factory, WeaponDatabase weaponDatabase, PickupSaveSystem pickupSave)
    {
        _player = player;
        _health = health;
        _stamina = stamina;
        _upgrades = upgrades;
        _container = container;
        _enemySave = enemySave;
        _inventory = inventory;
        _factory = factory;
        _weaponDatabase = weaponDatabase;
        _pickupSave = pickupSave;



        _path = Path.Combine(Application.persistentDataPath, FileName);
    }

    public string GetSavePath() => _path;

    public bool HasSave()
    {
        return File.Exists(_path);
    }

    public void Save()
    {
        var data = new SaveData();

        // === Позиция игрока ===
        var pos = _player.transform.position;
        data.PlayerPosX = pos.x;
        data.PlayerPosY = pos.y;
        data.PlayerPosZ = pos.z;
        data.PlayerRotY = _player.transform.eulerAngles.y;

        // === ХП / стамина текущие===
        data.CurrentHealth = _health.CurrentHealth;
        data.CurrentStamina = _stamina.CurrentStamina;

        // === Апгрейды === Увеличители урона, подтягиваются из UpgradeService
        data.HealthMultiplier = _upgrades.HealthMultiplier;
        data.DamageMultiplier = _upgrades.DamageMultiplier;
        data.SpeedMultiplier = _upgrades.SpeedMultiplier;
        data.StaminaMultiplier = _upgrades.StaminaMultiplier;

        // === Слоты оружия ===
        for (int i = 0; i < 3; i++)
        {
            data.WeaponSlots[i] = _inventory.Slots[i] != null ? 
                _inventory.Slots[i].Data.Type 
                :WeaponType.None; // Оружие=none?
        }
        data.ActiveWeaponSlot = _inventory.ActiveSlot;


        // === Враги ===
        var states = _enemySave.GetAllStates();
        data.DeadEnemies.Clear();
        foreach (var kv in states)
        {
            if (kv.Value) // Dead == true
                data.DeadEnemies.Add(kv.Key);
        }

        // === Пикапы (сундуки / оружие на сцене) ===
        data.CollectedPickups.Clear();
        foreach (var kv in _pickupSave.GetAllStates())
        {
            if (kv.Value)
                data.CollectedPickups.Add(kv.Key);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_path, json);
        Debug.Log($"[SaveService] Saved to {_path}");
    }

    public void Load()
    {
        if (!File.Exists(_path))
        {
            Debug.LogWarning("[SaveService] No save file to load.");
            return;
        }

        string json = File.ReadAllText(_path);
        var data = JsonUtility.FromJson<SaveData>(json);

        if (data == null)
        {
            Debug.LogError("[SaveService] Failed to parse save file.");
            return;
        }


        // === Оружие в слотах ===
        _inventory.ActiveSlot = data.ActiveWeaponSlot;


        // Чистит слот именно с подобранным оружием
        // Если сохранился, взял шмотку из сундука, загрузил сохранение
        // То шмотка не останется в инвентаре
        _inventory.Clear();

        for (int i = 0; i < 3; i++)
        {
            if (data.WeaponSlots[i] != WeaponType.None) // Оружие может быть "none"

                _inventory.Slots[i] = _factory.Create(
                    _weaponDatabase.GetData(data.WeaponSlots[i]));
        }



        // === Враги ===
        var loadedStates = new Dictionary<string, bool>();
        foreach (string id in data.DeadEnemies)
            loadedStates[id] = true;

        _enemySave.LoadStates(loadedStates);


        // === Пикапы ===
        var loadedPickupStates = new Dictionary<string, bool>();
        foreach (string id in data.CollectedPickups)
            loadedPickupStates[id] = true;

        _pickupSave.LoadStates(loadedPickupStates);


        // === Применение Апгрейдов ===
        // 1) Apply upgrades first so MaxHealth/MaxStamina become correct
        ApplyUpgradesFromSave(data);

        // 2) Apply runtime values (health / stamina)
        _health.ForceSetHealth(data.CurrentHealth);
        if (_stamina is IForceSetStamina setter)
            setter.ForceSetStamina(data.CurrentStamina);
        else
            Debug.LogWarning("[SaveService] IStaminaSystem does not implement ForceSetStamina. Stamina may not be restored.");

        // 3) Restore player transform


        // === Позиция / поворот игрока ===
        var controller = _player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;

            _player.transform.position = new Vector3(
                data.PlayerPosX,
                data.PlayerPosY,
                data.PlayerPosZ
            );

            _player.transform.rotation = Quaternion.Euler(
                0,
                data.PlayerRotY,
                0
            );

            controller.enabled = true;
        }
        else
        {
            _player.transform.position = new Vector3(
               data.PlayerPosX,
               data.PlayerPosY,
               data.PlayerPosZ);

            _player.transform.rotation = Quaternion.Euler(
                0,
                data.PlayerRotY,
                0
            );
        }

        Debug.Log("[SaveService] Save loaded.");

        OnLoadFinished?.Invoke();


    }

    public void DeleteSave()
    {
        if (File.Exists(_path))
        {
            File.Delete(_path);
            Debug.Log($"[SaveService] Deleted save {_path}");
        }
    }

    public void NewGame()
    {
        // Полностью удаляем сейв
        DeleteSave();

        // Очищаем список убитых врагов
        _enemySave.LoadStates(new Dictionary<string, bool>());

        // Сбрасываем пикапы
        _pickupSave.LoadStates(new Dictionary<string, bool>());

        // Чистим инвентарь
        _inventory.Clear();

        // ДОБАВЛЯЕМ ТОПОР В СЛОТ 0
        var axe = _factory.Create(_weaponDatabase.GetData(WeaponType.Axe));
        _inventory.Slots[0] = axe;
        _inventory.ActiveSlot = 0;


        Debug.Log("[SaveService] NewGame: прогресс сброшен.");
    }

    private void ApplyUpgradesFromSave(SaveData data)
    {
        // UpgradeService implements SetMultipliers (added it below)
        if (_upgrades is UpgradeService concrete)
        {
            concrete.SetMultipliers(
                data.HealthMultiplier,
                data.DamageMultiplier,
                data.SpeedMultiplier,
                data.StaminaMultiplier);
        }
        else
        {
            // fallback: apply deltas relative to 1.0
            Debug.LogWarning("[SaveService] UpgradeService concrete type not found. Trying to apply via ApplyUpgrade...");
            float h = data.HealthMultiplier - 1f;
            float d = data.DamageMultiplier - 1f;
            float s = data.SpeedMultiplier - 1f;
            float st = data.StaminaMultiplier - 1f;

            if (Mathf.Abs(h) > 0.0001f) _upgrades.ApplyUpgrade(UpgradeType.Health, h);
            if (Mathf.Abs(d) > 0.0001f) _upgrades.ApplyUpgrade(UpgradeType.Damage, d);
            if (Mathf.Abs(s) > 0.0001f) _upgrades.ApplyUpgrade(UpgradeType.Speed, s);
            if (Mathf.Abs(st) > 0.0001f) _upgrades.ApplyUpgrade(UpgradeType.Stamina, st);
        }
    }
}