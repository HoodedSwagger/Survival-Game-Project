using System.Collections.Generic;
using UnityEngine;

public class BiomeWorldGen : MonoBehaviour
{
    public Biome[] Biomes;
    public int seed;
    [Tooltip("n * n grid of vertices")]
    [SerializeField] private int n = 2;
    [SerializeField] private int resourcesAmount = 100;
    [SerializeField] private float biomeScale = 300f;
    [SerializeField] private float Scale = 1;
    [SerializeField] private float lacunarity = 1;
    [SerializeField] private float persistence = 1;
    [SerializeField] private float heightMultiplier = 1;

    [SerializeField] private int octaves = 1;

    public bool isGenerated { get; private set; } = false;

    private HashSet<Vector3> claimedPositions = new HashSet<Vector3>();
    private List<Vector3> rarePositions = new List<Vector3>();
    private GameObject rareCrystalPrefab;
    private GameObject rareFruitPrefab;
    private int crystalCount;
    private List<Vector3> pickedRareItems = new List<Vector3>();

    public HashSet<Vector3> ClaimedPositions => claimedPositions;

    private Mesh mesh;
    private MeshCollider meshCollider;

    public Vector3[] vertices { get; private set; }

    int[] triangles;

    float[] heights;
    float maxHeight = float.MinValue;
    Color[] colors;

    int[] dominantBiomes;

    float[] distances;
    float[] weights;

    public Vector2 chunkCoord;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();

        vertices = new Vector3[(n + 1) * (n + 1)];
        triangles = new int[(n) * (n) * 2 * 3];
        heights = new float[(n + 1) * (n + 1)];
        maxHeight = float.MinValue;
        colors = new Color[(n + 1) * (n + 1)];
        dominantBiomes = new int[(n + 1) * (n + 1)];
        distances = new float[Biomes.Length];
        weights = new float[Biomes.Length];
    }

    private void Distances(float humidity, float temperature, float[] distancesArray)
    {
        for (int i = 0; i < Biomes.Length; i++)
        {
            Vector2 point = new Vector2(humidity, temperature);
            Vector2 center = new Vector2(
                (Biomes[i].MaxHumidity + Biomes[i].MinHumidity) / 2,
                (Biomes[i].MaxTemperature + Biomes[i].MinTemperature) / 2);
            float dist = Vector2.Distance(point, center);

            distancesArray[i] = dist;
        }
    }
    private void GetWeights(float[] distances, float[] weightsArray)
    {
        float sum = 0;
        for (int i = 0; i < weightsArray.Length; i++)
        {
            if (distances[i] < 0.001f) distances[i] = 0.001f;

            weightsArray[i] = 1f / (distances[i] * distances[i]);
            sum += weightsArray[i];
        }
        for (int i = 0; i < weightsArray.Length; i++)
        {
            weightsArray[i] /= sum;
        }
    }

    public void Generate()
    {

        for (int x = 0; x <= n; x++)
        {
            for (int z = 0; z <= n; z++)
            {
                Random.InitState(seed);
                float offsetX = Random.Range(-10_000f, 10_000f);
                float offsetZ = Random.Range(-10_000f, 10_000f);

                float temperature = 0;
                float humidity = 0;

                float worldX = x + chunkCoord.x * n;
                float worldZ = z + chunkCoord.y * n;

                Random.InitState(seed + 1);
                float tempOffsetX = Random.Range(-10_000f, 10_000f);
                float tempOffsetZ = Random.Range(-10_000f, 10_000f);

                Random.InitState(seed + 2);
                float humOffsetX = Random.Range(-10_000f, 10_000f);
                float humOffsetZ = Random.Range(-10_000f, 10_000f);

                temperature = Mathf.PerlinNoise((worldX + tempOffsetX) / biomeScale, (worldZ + tempOffsetZ) / biomeScale);
                humidity = Mathf.PerlinNoise((worldX + humOffsetX) / biomeScale, (worldZ + humOffsetZ) / biomeScale);

                Distances(humidity, temperature,distances);
                GetWeights(distances,weights);

                float y = 0;

                for (int j = 0; j < Biomes.Length; j++)
                {
                    float biomeY = 0;
                    float amplitude = 1f;
                    float frequency = 1f;

                    for (int o = 0; o < octaves; o++)
                    {
                        float octave = Mathf.PerlinNoise(
                            (float)(worldX + offsetX) / (Biomes[j].Scale * frequency),
                            (float)(worldZ + offsetZ) / (Biomes[j].Scale * frequency)) * Biomes[j].HeightMultiplier;
                        biomeY += octave * amplitude;
                        frequency *= lacunarity;
                        amplitude *= persistence;
                    }
                    y += biomeY * weights[j] * weights[j];
                }

                int dominantBiome = 0;
                float maxWeight = 0;
                for (int j = 0; j < weights.Length; j++)
                {
                    if (weights[j] > maxWeight)
                    {
                        maxWeight = weights[j];
                        dominantBiome = j;

                        dominantBiomes[x + z * (n + 1)] = dominantBiome;
                    }

                }
                colors[x + z * (n + 1)] = Biomes[dominantBiome].Color;

                heights[x + z * (n + 1)] = y;
                if (y > maxHeight) maxHeight = y;

                vertices[x + z * (n + 1)].Set(x, y, z);

            }
        }

        int i = 0;
        for (int x = 0; x < n; x++)
        {
            for (int z = 0; z < n; z++)
            {
                int a = x + z * (n + 1);
                int b = (x + 1) + z * (n + 1);
                int c = x + (z + 1) * (n + 1);
                int d = (x + 1) + (z + 1) * (n + 1);

                // 
                triangles[i] = a;
                triangles[i + 1] = c;
                triangles[i + 2] = b;
                // 
                triangles[i + 3] = b;
                triangles[i + 4] = c;
                triangles[i + 5] = d;

                i += 6;
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.SetColors(colors);
        meshCollider.sharedMesh = mesh;
        mesh.RecalculateNormals();

        SpawnBiomeResources(dominantBiomes);
        SpawnRareItems();
        isGenerated = true;
    }

    private void SpawnBiomeResources(int[] dominantBiomes)
    {
        int chunkSeed = seed + (int)chunkCoord.x * 1000 + (int)chunkCoord.y;
        Random.InitState(chunkSeed);

        for (int i = 0; i < resourcesAmount; i++)
        {
            Vector3 verticePosition = vertices[Random.Range(0, vertices.Length)];

            int biomeIndex = dominantBiomes[(int)verticePosition.x + (int)verticePosition.z * (n + 1)];
            Biome biome = Biomes[biomeIndex];

            int resourceIndex = Random.Range(0, biome.Resources.Length);
            GameObject resourceToSpawn = biome.Resources[resourceIndex];

            Vector3 resourcePosition = verticePosition;
            Vector3 worldPosition = resourcePosition + new Vector3(chunkCoord.x * n, 0.1f, chunkCoord.y * n);

            if (!claimedPositions.Contains(worldPosition))
            {
                GameObject spawnedTree = Instantiate(resourceToSpawn, worldPosition, Quaternion.identity);
                claimedPositions.Add(worldPosition);
            }
        }
    }
    public void AddClaimedPositions(Vector3 position)
    {
        claimedPositions.Add(position);
    }
    public void SetRareItems(List<Vector3> positions, List<Vector3> picked,
                             GameObject crystal, GameObject fruit, int _crystalCount)
    {
        rarePositions = positions;
        pickedRareItems = picked;
        rareCrystalPrefab = crystal;
        rareFruitPrefab = fruit;
        crystalCount = _crystalCount;
    }
    private void SpawnRareItems()
    {
        Vector3 chunkOrigin = new Vector3(chunkCoord.x * n, 0, chunkCoord.y * n);
        Bounds chunkBounds = new Bounds(
            chunkOrigin + new Vector3(n / 2f, 0, n / 2f),
            new Vector3(n, 1000f, n)
        );

        for (int i = 0; i < rarePositions.Count; i++)
        {
            Vector3 pos = rarePositions[i];
            if (!chunkBounds.Contains(new Vector3(pos.x, chunkOrigin.y, pos.z)))
                continue;

            bool alreadyPicked = false;
            foreach (var picked in pickedRareItems)
            {
                if (Mathf.Approximately(picked.x, pos.x) &&
                    Mathf.Approximately(picked.z, pos.z))
                {
                    alreadyPicked = true;
                    break;
                }
            }
            if (alreadyPicked) continue;

            float worldY = chunkOrigin.y + 0.2f;
            if (Physics.Raycast(new Vector3(pos.x, 500f, pos.z), Vector3.down,
                                out RaycastHit hit, 1000f))
                worldY = hit.point.y + 0.1f;

            Vector3 spawnPos = new Vector3(pos.x, worldY, pos.z);
            GameObject prefab = (i < crystalCount) ? rareCrystalPrefab : rareFruitPrefab;
            GameObject spawned = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawned.GetComponent<RareItem>().Init(pos); 
            claimedPositions.Add(pos);
        }
    }
}
