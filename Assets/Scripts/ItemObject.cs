using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    public ItemInfo info;

    public virtual void Use()
    {
        Debug.Log($"Using Item: {info.itemName}");
    }
}
