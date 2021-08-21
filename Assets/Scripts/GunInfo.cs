using UnityEngine;

[CreateAssetMenu(fileName = "GunInfo", menuName = "Custom/GunInfo")]
public class GunInfo : ItemInfo 
{
    [Header("Gun Info")]
    public GameObject gunPrefab;
    public Transform firePoint;

    [Header("Gun Properties")]
    public int ammoAmount;
    public bool isAutomatic;
}