using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Tornado Ability Data", menuName = "Game/Abilities/Tornado Ability Data")]
public class TornadoAbilityData : Ability
{
    public GameObject tornado;
    public float projectileSpeed;
    public float endScale;
    public float scaleDuration;

    public override void Activate(GameObject character)
    {
        //if (isReady)
        if (IsReady())
        {
            //isReady = false;
            Debug.Log("tornado ability activated on " + character.name);

            Cooldown();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = character.transform.position.z;
            Vector3 projectDir = (mousePos - character.transform.position).normalized;

            GameObject projectileClone = Instantiate(tornado, character.transform.position + projectDir, Quaternion.identity);
            projectileClone.GetComponent<Rigidbody2D>().AddForce(projectDir * projectileSpeed, ForceMode2D.Impulse);
            projectileClone.transform.DOScale(endScale, scaleDuration);
            DestroyAfterLifeTime(projectileClone);
        }
        else
        {
            Debug.Log("tornado ability not ready");

            // maybe show some warning on ui
        }
    }
}