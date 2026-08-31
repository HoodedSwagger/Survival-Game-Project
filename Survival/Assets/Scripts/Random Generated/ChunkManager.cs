using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private BiomeWorldGen[] chunks;
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private int crystalCount = 4;
    [SerializeField] private int fruitCount = 3;
    [SerializeField] private float worldSize => 250 * 4;
    private int seed;
    private bool isSeedSet = false;
    public int Seed => seed;
    public BiomeWorldGen[] Chunks => chunks;

    private List<Vector3> rareItemPositions = new List<Vector3>();
    private List<Vector3> harvestPositions = new List<Vector3>();
    public List<Vector3> RareItemPositions => rareItemPositions;
    public List<Vector3> HarvestedPositions => harvestPositions;
    private List<Vector3> pickedRareItems = new List<Vector3>();
    public List<Vector3> PickedRareItems => pickedRareItems;

    public void SetPickedRareItems(List<Vector3> positions)
    {
        pickedRareItems = positions;
    }
    public void OnEnable()
    {
        EventBus<ResourceHarvestedEvent>.Subscribe(AddHarvestedPosition);
        EventBus<RareItemPickedEvent>.Subscribe(AddPickedRareItem);
    }
    public void OnDisable()
    {
        EventBus<ResourceHarvestedEvent>.Unsubscribe(AddHarvestedPosition);
        EventBus<RareItemPickedEvent>.Unsubscribe(AddPickedRareItem);
    }

    private void Start()
    {
        StartCoroutine(WaitUntilAllChunksGenerated());
    }
    public void StartGenerate()
    {
        if (!isSeedSet)
        {
            seed = Random.Range(-100_000, 100_000);
            GenerateRareItemPositions();
        }

        foreach (var chunk in chunks)
        {
            foreach (var pos in harvestPositions)
            {
                chunk.AddClaimedPositions(pos);
            }

            chunk.SetRareItems(rareItemPositions, pickedRareItems, crystalPrefab, fruitPrefab, crystalCount);
            chunk.seed = seed;
            chunk.Generate();
        }
    }
    private void GenerateRareItemPositions()
    {
        rareItemPositions.Clear();
        Random.InitState(seed + 999);

        float chunkSize = 250f;
        int chunksPerRow = 4;
        float total = chunkSize * chunksPerRow;
        float q = total / 2f;

        Vector2[] quadrants = {
        new Vector2(0, 0),
        new Vector2(q, 0),
        new Vector2(0, q),
        new Vector2(q, q)
    };

        for (int i = 0; i < crystalCount; i++)
        {
            float x = quadrants[i].x + Random.Range(30f, q - 30f);
            float z = quadrants[i].y + Random.Range(30f, q - 30f);
            rareItemPositions.Add(new Vector3(x, 0, z));
        }

        int attempts = 0, spawned = 0;
        while (spawned < fruitCount && attempts < 200)
        {
            attempts++;
            float x = Random.Range(30f, total - 30f);
            float z = Random.Range(30f, total - 30f);
            Vector3 pos = new Vector3(x, 0, z);

            bool tooClose = false;
            foreach (var existing in rareItemPositions)
                if (Vector3.Distance(pos, existing) < 80f) { tooClose = true; break; }

            if (!tooClose) { rareItemPositions.Add(pos); spawned++; }
        }
    }
    public void SetSeed(int _seed)
    {
        seed = _seed;
        isSeedSet = true;
    }

    private void AddHarvestedPosition(ResourceHarvestedEvent harvestedEvent)
    {
        harvestPositions.Add(harvestedEvent.position);
    }
    public void SetHarvestedPositions(List<Vector3> positions)
    {
        harvestPositions = positions;
    }
    public void SetRareItemPositions(List<Vector3> positions)
    {
        rareItemPositions = positions;
    }
    private void AddPickedRareItem(RareItemPickedEvent e)
    {
        pickedRareItems.Add(e.position);
    }

    private IEnumerator WaitUntilAllChunksGenerated()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            if (chunks[i].isGenerated == false)
            {
                yield return null;
            }
        }
        EventBus<NavMeshUpdateEvent>.Raise(new NavMeshUpdateEvent());
    }

}
