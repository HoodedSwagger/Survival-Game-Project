using UnityEngine;

[CreateAssetMenu(fileName = "Biome", menuName = "Scriptable Objects/Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] private float heightMultiplier;
    [SerializeField] private float scale;
    [SerializeField] private float minTemperature, maxTemperature;
    [SerializeField] private float minHumidity, maxHumidity;
    [SerializeField] private Color color;

    [SerializeField] private GameObject[] resources;

    public float HeightMultiplier => heightMultiplier;
    public float Scale => scale;
    public float MinTemperature => minTemperature;
    public float MaxTemperature => maxTemperature;

    public float MinHumidity => minHumidity;
    public float MaxHumidity => maxHumidity;
    public Color Color => color;

    public GameObject[] Resources => resources;
}
