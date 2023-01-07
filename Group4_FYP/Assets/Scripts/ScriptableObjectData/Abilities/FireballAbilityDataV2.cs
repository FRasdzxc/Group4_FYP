using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fireball Ability Data V2", menuName = "Game/Fireball Ability Data V2")]
public class FireballAbilityDataV2 : Ability
{
    public Transform fireball;
    public float speed;
    public int fireballCount;

    public override async void Activate(GameObject character)
    {
        if (isReady)
        {
            isReady = false;
            Debug.Log("fireball ability activated on " + character.name);

            Cooldown();

            float currentAngle = 0;
            for (int i = 0; i < fireballCount; i++)
            {
                float radians = currentAngle * Mathf.Deg2Rad;
                Vector3 projectDir = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;
                float projectAngle = Mathf.Atan2(projectDir.y, projectDir.x) * Mathf.Rad2Deg;

                Transform projectileClone = Instantiate(fireball, character.transform.position + projectDir, Quaternion.Euler(0, 0, projectAngle));
                projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * speed, ForceMode2D.Impulse);
                DestroyGobj(projectileClone.gameObject);

                currentAngle += 360 / (float)fireballCount;
                await Task.Delay(50);
            }
        }
        else
        {
            Debug.Log("fireball ability not ready");

            // maybe show some warning on ui
        }
    }
}