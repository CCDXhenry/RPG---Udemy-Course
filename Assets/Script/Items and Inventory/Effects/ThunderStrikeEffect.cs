using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]
public class ThunderSkrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    [Range(0,1)]
    [SerializeField] private float triggerProbability;
    public override void ExecuteEffect(Transform _transfrom)
    {
        var rand = Random.Range(0f, 1f);
        if (rand < triggerProbability)
        {
            float offsetX = 0;
            float offsetY = 0;
            Vector3 offsetScale = Vector3.zero;

            if (_transfrom.gameObject.GetComponent<Enemy>().entityName == "BringerOfDeath")
            {
                offsetX = -0.05f;
                offsetY = -0.91f;
                offsetScale = new Vector3(1.5834f, 1.7021f, 1.5834f);
            }
            GameObject thunderStrike = Instantiate(thunderStrikePrefab, _transfrom.position + new Vector3(offsetX, offsetY), Quaternion.identity);
            thunderStrike.transform.localScale = offsetScale;
        }

    }
}
