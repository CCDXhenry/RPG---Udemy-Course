using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect")]
public class ItemEffect : ScriptableObject
{
    /// <summary>
    /// 执行物品效果的方法。
    /// </summary>
    /// <param name="item"></param>
    /// <param name="inventory"></param>
    public virtual void ExecuteEffect(Transform _transfrom)
    {
        // This method can be overridden by derived classes to implement specific effects
        Debug.Log("Executing effect");
    }
}

