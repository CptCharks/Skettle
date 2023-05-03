using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunPrices", menuName = "", order = 0)]
public class GunPrices : ScriptableObject
{
    [System.Serializable]
    public struct GunListing
    {
        public string gunName;
        public int gunPrice;
    }

    //The list should follow the gunEnum pattern
    public List<GunListing> gunPrices;
}
