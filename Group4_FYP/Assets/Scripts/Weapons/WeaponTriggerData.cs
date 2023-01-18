using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used for player/enemy weapon that will collide with player/enemy

[CreateAssetMenu(fileName = "New Weapon Trigger Data", menuName = "Game/Weapons/Weapon Trigger Data")]
public class WeaponTriggerData : ScriptableObject
{
    public string weaponTriggerName;

    public float damage;
    public float criticalDamage;
}