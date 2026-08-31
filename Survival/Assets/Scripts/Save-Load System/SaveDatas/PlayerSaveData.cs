using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class PlayerSaveData
{
    public Vector3 playerPosition;
    public int playerHealth;
    public int playerHunger;
    public List<InventorySlotSaveData> slots;
}
 