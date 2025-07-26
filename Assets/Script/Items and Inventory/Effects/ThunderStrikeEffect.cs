using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Thunder strike effect",menuName ="Data/Item effect/Thunder strike")]
public class ThunderSkrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _transfrom)
    {
        GameObject thunderStrike = Instantiate(thunderStrikePrefab, _transfrom.position, Quaternion.identity);

    }
}
