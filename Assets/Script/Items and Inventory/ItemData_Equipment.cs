using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentgType
{
    Weapon,
    Armor,
    Amulet,
    Flask,
}

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentgType equipmentType;
}

