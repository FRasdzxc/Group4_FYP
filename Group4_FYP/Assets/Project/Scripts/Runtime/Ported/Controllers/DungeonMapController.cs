using UnityEngine;
using System.Threading.Tasks;

public class DungeonMapController : HostileMapController
{
    protected DungeonMapData dungeonMapData;
    protected float remainingTime;
    protected bool timerRunning;

    protected override void Start()
    {
        dungeonMapData = mapData as DungeonMapData;
        base.Start();

        if (dungeonMapData.isTimed)
        {
            remainingTime = dungeonMapData.timeLimit;
            HUD.Instance.ShowTimer((int)remainingTime);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (dungeonMapData.isTimed && timerRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                HUD.Instance.UpdateTimer((int)remainingTime);
            }
            else
            {
                timerRunning = false;
                _ = Notification.Instance.ShowNotification("You ran out of time!");
                StartCoroutine(Hero.Instance.Die());
            }
        }
        else
            timerRunning = false;
    }

    protected virtual void OnEnable()
        => Hero.onHeroDeath += () => timerRunning = false;
    
    protected override void OnDisable()
    {
        base.OnDisable();
        Hero.onHeroDeath -= () => timerRunning = false;
    }

    protected override void SpawnPortal()
    {
        timerRunning = false;
        _ = HUD.Instance.HideTimer();
        base.SpawnPortal();
    }

    protected async override Task ShowMapMessage()
    {
        await HUD.Instance.ShowHugeMessageAsync(dungeonMapData.mapName, $"{dungeonMapData.dungeonType} {dungeonMapData.mapType} / {dungeonMapData.mapDifficulty}");
        timerRunning = true;
    }
}
