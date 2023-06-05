using UnityEngine;

[CreateAssetMenu(fileName = "New Dungeon Map Data", menuName = "Game/Map Data/Dungeon Map Data")]
public class DungeonMapData : HostileMapData
{
    public DungeonType dungeonType;

    public bool isTimed = true;

    [HideInInspector]
    public int timeLimit;
}
