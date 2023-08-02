using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsData", menuName = "Item/ItemsData")]
public class ItemsStore : ScriptableObject
{
    public List<ItemData> items = new List<ItemData>();
}
