using UnityEngine;
using System.Runtime.InteropServices;

namespace DigitalArchitects.UI
{
    public class ReactUIBridge : MonoBehaviour 
    {
        [SerializeField] private string containerName = "react-ui-container";
        
        // JavaScript interface methods
        [DllImport("__Internal")]
        private static extern void InitializeReactUI(string containerId);
        
        [DllImport("__Internal")]
        private static extern void UpdateUIState(string jsonState);
        
        void Start()
        {
            #if !UNITY_EDITOR && UNITY_WEBGL
                InitializeReactUI(containerName);
            #endif
            
            // Create container for React UI
            var container = new GameObject(containerName);
            container.AddComponent<RectTransform>();
            // Setup container properties...
        }

        public void SendStateToUI(string jsonState)
        {
            #if !UNITY_EDITOR && UNITY_WEBGL
                UpdateUIState(jsonState);
            #endif
        }
    }
}