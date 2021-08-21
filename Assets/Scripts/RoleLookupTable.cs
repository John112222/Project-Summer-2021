using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleLookupTable : MonoBehaviour
{
    public RoleItems defaultRole;

    public List<RoleItems> roles = new List<RoleItems>();

    public static RoleLookupTable mainTable = null;

    private void Awake() {
        if(mainTable == null)
        {
            DontDestroyOnLoad(this.gameObject);
            mainTable = this;
        }
    }


    public static RoleItems GetRoleItems(string roleWanted)
    {
        foreach(RoleItems role in mainTable.roles)
        {
            if(roleWanted == role.roleName)
            {
                return role;
            }
        }
        return mainTable.defaultRole;
    }
}

[System.Serializable]
public class RoleItems
{
    public string roleName;
    public List<ItemObject> itemsList;

}