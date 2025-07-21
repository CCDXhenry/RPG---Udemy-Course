using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [SerializeField] private GameObject clonePrefab; // The prefab for the clone
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack; 
    public void CreateClone(Transform _cloneTransform,Vector3 _offset)
    {
        if (clonePrefab != null)
        {
            // Instantiate the clone at the player's position and rotation
            GameObject newClone = Instantiate(clonePrefab);
            
            newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_cloneTransform, cloneDuration, canAttack, _offset);
        }
        else
        {
            Debug.LogWarning("Clone prefab is not assigned in Clone_Skill.");
        }
    }
}
