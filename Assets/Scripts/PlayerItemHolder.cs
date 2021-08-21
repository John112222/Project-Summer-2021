using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerItemHolder : MonoBehaviourPunCallbacks
{
    public GameObject equipmentHolderParent = null;
    public const string EQUIP_INDEX = "equipmentIndex";
    public List<ItemObject> items;
    public KeyCode nextItemKey = KeyCode.Tab;

    [SerializeField]
    private List<GameObject> itemObjects;
    [SerializeField]
    private int currentItemIndex = 0;


    void Start()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        currentItemIndex = 0;
        // TODO: Get item based on saved Role
        items = RoleLookupTable.GetRoleItems("Default").itemsList;
        if(equipmentHolderParent == null)
        {
            equipmentHolderParent = new GameObject("Equipment Holder");
            equipmentHolderParent.transform.parent = this.transform;
        }
        foreach(ItemObject item in items)
        {
            // TODO: Instantiate actual equipment prefab
            itemObjects.Add(Instantiate(equipmentHolderParent, equipmentHolderParent.transform));
        }
    }

    void Update()
    {
        // Ignore input from other players
        if(!photonView.IsMine) return;

        if(Input.GetKeyDown(nextItemKey))
        {
            EquipItem(currentItemIndex + 1);
        }

    }

    public void EquipItem(int newIndex)
    {
        if(items.Count <= 0)
        {
            return;
        }
        if(itemObjects.Count != items.Count)
        {
            Debug.LogError("Mismatched size between Items and their Objects");
        }
        itemObjects[currentItemIndex].SetActive(false);
        currentItemIndex = newIndex % items.Count;
        itemObjects[currentItemIndex].SetActive(true);

        if(photonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add(EQUIP_INDEX, newIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        object equipIndex;
        if(!photonView.IsMine && targetPlayer == photonView.Owner && changedProps.TryGetValue(EQUIP_INDEX, out equipIndex))
        {
            EquipItem((int) equipIndex);
        }
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }
}
