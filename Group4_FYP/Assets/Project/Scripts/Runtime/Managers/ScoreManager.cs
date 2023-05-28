using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PathOfHero.Managers.Data;

namespace PathOfHero.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        private ScoreEventChannel m_EventChannel;

        private bool m_InLevel;
        private SessionStats m_CurrentStats;

        public bool InLevel => m_InLevel;
        public SessionStats CurrentStats => m_CurrentStats;

        private void OnEnable()
        {
            m_EventChannel.OnLevelStart += LevelStarted;
            m_EventChannel.OnLevelEnd += LevelEnded;

            m_EventChannel.OnStepTaken += StepsTaken;
            m_EventChannel.OnWeaponUsed += WeaponUsed;
            m_EventChannel.OnAbilityUsed += AbilityUsed;
            m_EventChannel.OnDamageTaken += DamageTaken;
            m_EventChannel.OnDamageGiven += DamageGiven;
            m_EventChannel.OnMobKilled += MobKilled;
        }

        private void OnDisable()
        {
            m_EventChannel.OnLevelStart -= LevelStarted;
            m_EventChannel.OnLevelEnd -= LevelEnded;

            m_EventChannel.OnStepTaken -= StepsTaken;
            m_EventChannel.OnWeaponUsed -= WeaponUsed;
            m_EventChannel.OnAbilityUsed -= AbilityUsed;
            m_EventChannel.OnDamageTaken -= DamageTaken;
            m_EventChannel.OnDamageGiven -= DamageGiven;
            m_EventChannel.OnMobKilled -= MobKilled;
        }

        private void Update()
        {
            if (!m_InLevel)
                return;

            m_CurrentStats.timeTaken += Time.deltaTime;
        }

        private void LevelStarted()
        {
            if (m_InLevel)
                return;

            m_CurrentStats = new();
            m_InLevel = true;
        }

        private void LevelEnded()
        {
            if (!m_InLevel)
                return;

            m_InLevel = false;
        }

        private void StepsTaken()
        {
            if (!m_InLevel)
                return;

            m_CurrentStats.stepsTaken++;
        }

        private void DamageGiven(float amount)
        {
            if (!m_InLevel)
                return;

            m_CurrentStats.damageGiven += amount;
        }

        private void DamageTaken(float amount)
        {
            if (!m_InLevel)
                return;

            m_CurrentStats.damageTaken += amount;
        }

        private void WeaponUsed(string weponName)
        {
            if (!m_InLevel || string.IsNullOrWhiteSpace(weponName))
                return;

            if (!m_CurrentStats.weaponUsage.ContainsKey(weponName))
                m_CurrentStats.weaponUsage[weponName] = 0;

            m_CurrentStats.weaponUsage[weponName]++;
        }

        private void AbilityUsed(string abilityName)
        {
            if (!m_InLevel || string.IsNullOrWhiteSpace(abilityName))
                return;

            if (!m_CurrentStats.abilityUsage.ContainsKey(abilityName))
                m_CurrentStats.abilityUsage[abilityName] = 0;

            m_CurrentStats.abilityUsage[abilityName]++;
        }

        private void MobKilled(string mobName)
        {
            if (!m_InLevel || string.IsNullOrWhiteSpace(mobName))
                return;

            // Check if the mob is a chest
            if (mobName.EndsWith("Chest"))
            {
                if (!m_CurrentStats.chestsFound.ContainsKey(mobName))
                    m_CurrentStats.chestsFound[mobName] = 0;

                m_CurrentStats.chestsFound[mobName]++;
            }
            else
            {
                if (!m_CurrentStats.mobsKilled.ContainsKey(mobName))
                    m_CurrentStats.mobsKilled[mobName] = 0;

                m_CurrentStats.mobsKilled[mobName]++;
            }
        }

        public class SessionStats
        {
            public float timeTaken;
            public int stepsTaken;
            public float damageGiven;
            public float damageTaken;
            public Dictionary<string, int> weaponUsage;
            public Dictionary<string, int> abilityUsage;
            public Dictionary<string, int> mobsKilled;
            public Dictionary<string, int> chestsFound;

            public int WeaponUsage => weaponUsage.Sum(w => w.Value);
            public int AbilityUsage => abilityUsage.Sum(a => a.Value);
            public int MobsKilled => mobsKilled.Sum(m => m.Value);
            public int ChestsFound => chestsFound.Sum(d => d.Value);

            public SessionStats()
            {
                weaponUsage = new();
                abilityUsage = new();
                mobsKilled = new();
                chestsFound = new();
            }
        }
    }
}