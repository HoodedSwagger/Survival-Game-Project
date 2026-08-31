using System;
[Serializable]
public class SaveData
{
    public PlayerSaveData playerSaveData;
    public BuildingData[] buildingData;
    public EnemyData[] enemyData;
    public HarvestedData[] harvestedData;
    public RareItemSpawnData rareItemSpawnData;
    public WorldData worldData;
    public SettingsSaveData settingsData;
}
