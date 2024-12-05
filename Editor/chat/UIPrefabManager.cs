namespace DigitalArchitects.Editor
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UIPrefabManager : MonoBehaviour
    {
        [Header("Chat UI")]
        [SerializeField] private GameObject chatPanel;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private ScrollRect chatScrollRect;
        [SerializeField] private RectTransform messageContainer;

        [Header("Message Prefabs")]
        [SerializeField] private GameObject userMessagePrefab;
        [SerializeField] private GameObject agentMessagePrefab;
        [SerializeField] private GameObject systemMessagePrefab;

        [Header("Debug Panel")]
        [SerializeField] private GameObject debugPanel;
        [SerializeField] private TextMeshProUGUI connectionStatus;
        [SerializeField] private TextMeshProUGUI lastMessageReceived;
        [SerializeField] private TextMeshProUGUI environmentStatus;

        [Header("Test Controls")]
        [SerializeField] private Button testConnectionButton;
        [SerializeField] private Button testActionButton;
        [SerializeField] private Toggle mockResponseToggle;

        private DigitalArchitectCore core;

        private void Awake()
        {
            core = FindObjectOfType<DigitalArchitectCore>();
            SetupUI();
        }

        private void SetupUI()
        {
            testConnectionButton?.onClick.AddListener(() => core.TestWebSocketConnection());
            testActionButton?.onClick.AddListener(() => core.TestEnvironmentAction());
            
            if (mockResponseToggle != null)
            {
                mockResponseToggle.onValueChanged.AddListener(value => 
                {
                    // Update mock mode
                    UpdateDebugStatus("Mock Mode: " + value);
                });
            }
        }

        public void UpdateDebugStatus(string status)
        {
            if (connectionStatus != null)
                connectionStatus.text = status;
        }

        public void UpdateLastMessage(string message)
        {
            if (lastMessageReceived != null)
                lastMessageReceived.text = message;
        }

        public void UpdateEnvironmentStatus(string status)
        {
            if (environmentStatus != null)
                environmentStatus.text = status;
        }

        #region UI Message Creation Methods
        public void CreateUserMessage(string message)
        {
            if (userMessagePrefab != null && messageContainer != null)
            {
                GameObject msgObj = Instantiate(userMessagePrefab, messageContainer);
                var textComponent = msgObj.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = message;
                }
                ScrollToBottom();
            }
        }

        public void CreateAgentMessage(string message)
        {
            if (agentMessagePrefab != null && messageContainer != null)
            {
                GameObject msgObj = Instantiate(agentMessagePrefab, messageContainer);
                var textComponent = msgObj.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = message;
                }
                ScrollToBottom();
            }
        }

        public void CreateSystemMessage(string message)
        {
            if (systemMessagePrefab != null && messageContainer != null)
            {
                GameObject msgObj = Instantiate(systemMessagePrefab, messageContainer);
                var textComponent = msgObj.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = message;
                }
                ScrollToBottom();
            }
        }
        #endregion

        #region UI Helper Methods
        private void ScrollToBottom()
        {
            if (chatScrollRect != null)
            {
                // Wait for end of frame to ensure proper scrolling
                StartCoroutine(ScrollToBottomCoroutine());
            }
        }

        private System.Collections.IEnumerator ScrollToBottomCoroutine()
        {
            yield return new WaitForEndOfFrame();
            chatScrollRect.normalizedPosition = new Vector2(0, 0);
        }

        public void ClearChat()
        {
            if (messageContainer != null)
            {
                foreach (Transform child in messageContainer)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void SetInputFieldInteractable(bool interactable)
        {
            if (inputField != null)
                inputField.interactable = interactable;
            if (sendButton != null)
                sendButton.interactable = interactable;
        }
        #endregion

        #region Debug Panel Methods
        public void ToggleDebugPanel(bool show)
        {
            if (debugPanel != null)
                debugPanel.SetActive(show);
        }

        public void UpdateTestControls(bool enableTests)
        {
            if (testConnectionButton != null)
                testConnectionButton.gameObject.SetActive(enableTests);
            if (testActionButton != null)
                testActionButton.gameObject.SetActive(enableTests);
            if (mockResponseToggle != null)
                mockResponseToggle.gameObject.SetActive(enableTests);
        }
        #endregion

        private void OnDestroy()
        {
            // Clean up any listeners or resources
            if (testConnectionButton != null)
                testConnectionButton.onClick.RemoveAllListeners();
            if (testActionButton != null)
                testActionButton.onClick.RemoveAllListeners();
            if (mockResponseToggle != null)
                mockResponseToggle.onValueChanged.RemoveAllListeners();
        }
    }
}