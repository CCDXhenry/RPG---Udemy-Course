using System.Collections;
using UnityEngine;

namespace Assets.Script.UI
{
    public class UI_ItemSlot_Equipment : UI_ItemSlot
    {
        public EquipmentgType equipmentType;

        private void OnValidate()
        {
            gameObject.name = "Equipment Slot - " + equipmentType;
        }
    }
}
