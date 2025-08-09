using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [SerializeField] private GameObject clonePrefab; // The prefab for the clone
    [SerializeField] private float cloneDuration;
    [SerializeField] private float cloneMultplePro;// 多重分身概率
    [SerializeField] private float cloneMultipleCounts; // 分身数量
    [Space]
    [SerializeField] private bool canAttack; 
    public void CreateClone(Transform _cloneTransform,Vector3 _offset,bool _ismultiple)
    {
        if (clonePrefab != null)
        {
            CreateClonePrefab(_cloneTransform, _offset);
            if (_ismultiple)
            {

                for (int i = 0; i < cloneMultipleCounts; i++)
                {
                    var rand = Random.Range(0f, 1f);
                    if (rand < cloneMultplePro)
                    {
                        StartCoroutine(DelayedCreateClone(_cloneTransform, _offset, 0.1f));
                    }
                }
            }

            // Instantiate the clone at the player's position and rotation

        }
        else
        {
            Debug.LogWarning("Clone prefab is not assigned in Clone_Skill.");
        }
    }

    private void CreateClonePrefab(Transform _cloneTransform, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_cloneTransform, cloneDuration, canAttack, _offset);
    }

    private IEnumerator DelayedCreateClone(Transform _cloneTransform, Vector3 _offset, float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateClonePrefab(_cloneTransform, _offset);
    }

}
