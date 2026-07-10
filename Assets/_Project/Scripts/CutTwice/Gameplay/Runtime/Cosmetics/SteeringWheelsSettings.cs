using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Cosmetics
{
    [CreateAssetMenu(fileName = "PurchasesSettings", menuName = "CutTwice/PurchasesSettings")]
    public class SteeringWheelsSettings : ScriptableObject
    {
        [Serializable]
        public struct SteeringWheelData
        {
            public string Id;
            public Material Material;
        }
        
        public List<SteeringWheelData> SteeringWheelMaterial = new ();
        
        public SteeringWheelData GetSteeringWheelData(string id)
        {
            return SteeringWheelMaterial.FirstOrDefault(d => d.Id == id);
        }
    }
}