using UnityEngine;

[CreateAssetMenu(fileName = "New Map Data", menuName = "Game/Map Data")]
public class MapData : ScriptableObject
{
    public string mapId;
    [Tooltip("Players cannot attack / take damage when Map Type is Peaceful")]
    public string mapName;
    public MapType mapType;
    public MapDifficulty mapDifficulty;
    [Tooltip("Leave blank if none.")] [TextArea(5, 5)]
    public string objective;
    public GameObject mapPrefab;
}
