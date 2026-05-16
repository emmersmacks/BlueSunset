using CutTwice.Game;
using UnityEngine;

namespace CutTwice.Cosmetics
{
    public class SteeringWheelSkinController : MonoBehaviour
    {
        public SteeringWheelsSettings steeringWheelsSettings;
        
        public MeshRenderer SteeringWheelMesh;
        public Material DefaultMaterial;

        private void Awake()
        {
            PlayerData.OnSteeringWheelChanged.AddListener(UpdateSteeringWheelSkin);
            UpdateSteeringWheelSkin();
        }
        
        private void UpdateSteeringWheelSkin()
        {
            SteeringWheelMesh.material = string.IsNullOrEmpty(PlayerData.SteeringWheelId) ? DefaultMaterial : steeringWheelsSettings.GetSteeringWheelData(PlayerData.SteeringWheelId).Material;
        }
    }
}