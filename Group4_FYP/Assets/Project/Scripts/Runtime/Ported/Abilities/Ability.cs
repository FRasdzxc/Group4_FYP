using PathOfHero.Telemetry;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    public HeroClass heroClass;
    public float lifeTime; // seconds until destroy
    public float cooldownTime; // seconds until next use
    public float manaCost;
    public Sprite icon;

    public float Cooldown
    {
        get => IsReady ? 0f : nextActivateTime - Time.time;
        set => nextActivateTime = Time.time + value;
    }

    protected float nextActivateTime;

    public bool IsReady => Time.time >= nextActivateTime;

    protected float abilityDamageUpgrade;

    public virtual void Activate(GameObject character)
    {
        if (!IsReady)
            return;

        //DataCollector.Instance?.AbilityUsed(abilityName);
        nextActivateTime = Time.time + cooldownTime;

        abilityDamageUpgrade = AbilityManager.Instance.GetAbilityDamageUpgrade();
        calculateAbilityDamage();
    }
    
    protected virtual void calculateAbilityDamage() { }
}
