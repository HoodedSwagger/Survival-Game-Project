using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    [SerializeField] private ItemsRegistry itemRegistry;
    [SerializeField] private GameObjectsRegistry objectsRegistry;
    [SerializeField] private EnemiesRegistry enemiesRegistry;
    private string path => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance != null) { Destroy(Instance); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public bool HasSave()
    {
        return File.Exists(path);
    }
    public bool DeleteSave()
    {
        try
        {
            if (HasSave())
                File.Delete(path);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"ошибка сохранения: {e.Message}");
            return false;
        }
    }
    public bool Save()
    {
        try
        {
            SaveData saveData = new SaveData();

            GameObject player = GameObject.FindWithTag("Player");
            saveData.playerSaveData = new PlayerSaveData();
            saveData.playerSaveData.playerHealth = player.GetComponent<PlayerHealth>().CurrentHealth;
            saveData.playerSaveData.playerHunger = player.GetComponent<PlayerHunger>().CurrentHunger;
            saveData.playerSaveData.playerPosition = player.transform.position;
            saveData.playerSaveData.slots = new List<InventorySlotSaveData>();
            List<InventorySlot> inventory = player.GetComponent<Inventory>().GetItemsInInventory();
            foreach (var slot in inventory)
            {
                if (slot.ItemInSlot == null) continue;
                InventorySlotSaveData slotSaveData = new InventorySlotSaveData();
                slotSaveData.itemId = slot.ItemInSlot.ID;
                slotSaveData.count = slot.ItemsCount;
                slotSaveData.slotIndex = slot.SlotIndex;
                saveData.playerSaveData.slots.Add(slotSaveData);
            }

            saveData.worldData = new WorldData();
            saveData.worldData.seed = FindAnyObjectByType<ChunkManager>().Seed;
            DayNightCycle cycle = FindAnyObjectByType<DayNightCycle>();
            saveData.worldData.days = cycle.Days;
            saveData.worldData.hours = cycle.Hours;
            saveData.worldData.minutes = cycle.Minutes;

            GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
            saveData.buildingData = new BuildingData[buildings.Length];
            for (int i = 0; i < buildings.Length; i++)
            {
                saveData.buildingData[i] = new BuildingData();
                saveData.buildingData[i].type = buildings[i].GetComponent<Building>().ID;
                saveData.buildingData[i].position = buildings[i].transform.position;
                saveData.buildingData[i].rotation = buildings[i].transform.rotation;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            saveData.enemyData = new EnemyData[enemies.Length];
            for (int i = 0; i < enemies.Length; i++)
            {
                saveData.enemyData[i] = new EnemyData();
                saveData.enemyData[i].position = enemies[i].transform.position;
                saveData.enemyData[i].type = enemies[i].GetComponent<Enemy>().ID;
                saveData.enemyData[i].health = enemies[i].GetComponent<HealthSystem>().Health;
            }

            ChunkManager chunkManager = FindAnyObjectByType<ChunkManager>();
            saveData.harvestedData = new HarvestedData[chunkManager.HarvestedPositions.Count];

            for (int i = 0; i < saveData.harvestedData.Length; i++)
            {
                saveData.harvestedData[i] = new HarvestedData();
                saveData.harvestedData[i].position = chunkManager.HarvestedPositions[i];
            }

            saveData.settingsData = SettingsManager.Instance.GetSaveData();

            saveData.rareItemSpawnData = new RareItemSpawnData();
            saveData.rareItemSpawnData.rareItemPositions = chunkManager.RareItemPositions;
            saveData.rareItemSpawnData.pickedRareItems = chunkManager.PickedRareItems;


            File.WriteAllText(path, JsonUtility.ToJson(saveData, true));

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка сохранения {e.Message}");
            return false;
        }
    }
    public bool Load()
    {
        ChunkManager chunkManager = FindAnyObjectByType<ChunkManager>();

        if (!File.Exists(path))
        {
            FindAnyObjectByType<DayNightCycle>().InitDefault();
            chunkManager.StartGenerate();
            return false;
        }

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

        GameObject player = GameObject.FindWithTag("Player");

        player.GetComponent<PlayerHunger>().SetHunger(saveData.playerSaveData.playerHunger);
        player.GetComponent<PlayerHealth>().SetHealth(saveData.playerSaveData.playerHealth);
        player.transform.position = saveData.playerSaveData.playerPosition;

        List<InventorySlot> playerInventory = player.GetComponent<Inventory>().GetItemsInInventory();
        foreach (var slot in saveData.playerSaveData.slots)
        {
            playerInventory[slot.slotIndex].AddItems(itemRegistry.GetByID(slot.itemId), slot.count);
        }

        chunkManager.SetSeed(saveData.worldData.seed);

        FindAnyObjectByType<DayNightCycle>().SetTime(
            saveData.worldData.minutes,
            saveData.worldData.hours,
            saveData.worldData.days
            );

        foreach (var building in saveData.buildingData)
        {
            GameObject spawnedBuilding = Instantiate(objectsRegistry.GetByType(building.type), building.position, building.rotation);
            if (spawnedBuilding.TryGetComponent(out IPlaceable placeable))
            {
                placeable.Activate();
            }
        }

        foreach (var enemy in saveData.enemyData)
        {
            GameObject spawnedEnemy = Instantiate(enemiesRegistry.GetByID(enemy.type), enemy.position, Quaternion.identity);
            spawnedEnemy.GetComponent<HealthSystem>().SetHealth(enemy.health);
        }

        List<Vector3> positions = new List<Vector3>();
        foreach (var harvestData in saveData.harvestedData)
        {
            positions.Add(harvestData.position);
        }
        chunkManager.SetHarvestedPositions(positions);
        chunkManager.SetRareItemPositions(saveData.rareItemSpawnData.rareItemPositions);
        chunkManager.SetSeed(saveData.worldData.seed);
        chunkManager.SetPickedRareItems(saveData.rareItemSpawnData.pickedRareItems);
        chunkManager.StartGenerate();

        SettingsManager.Instance.ApplySaveData(saveData.settingsData);

        return true;
    }
}
