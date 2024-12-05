namespace DigitalArchitects.Editor
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UISetupHelper : MonoBehaviour
    {
        public static GameObject CreateBasicUI()
        {
            // Create Canvas
            GameObject canvasObj = new GameObject("DigitalArchitectCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Create Chat Panel
            GameObject chatPanel = CreatePanel(canvasObj.transform, "ChatPanel");
            chatPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            
            // Create Input Field
            GameObject inputField = CreateInputField(chatPanel.transform);
            
            // Create Message Container
            GameObject messageContainer = CreateScrollView(chatPanel.transform);
            
            // Create Debug Panel
            GameObject debugPanel = CreateDebugPanel(canvasObj.transform);

            return canvasObj;
        }

        private static GameObject CreatePanel(Transform parent, string name)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent);
            
            RectTransform rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            
            // Add Image component for visualization
            Image image = panel.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.8f);
            
            return panel;
        }

        private static GameObject CreateInputField(Transform parent)
        {
            GameObject inputFieldObj = new GameObject("InputField");
            inputFieldObj.transform.SetParent(parent);

            RectTransform rect = inputFieldObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.sizeDelta = new Vector2(0, 40);
            rect.anchoredPosition = new Vector2(0, 10);

            // Add TMP Input Field
            TMP_InputField inputField = inputFieldObj.AddComponent<TMP_InputField>();
            
            // Create text area
            GameObject textArea = new GameObject("Text Area");
            textArea.transform.SetParent(inputFieldObj.transform);
            RectTransform textAreaRect = textArea.AddComponent<RectTransform>();
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.sizeDelta = Vector2.zero;

            // Create text component
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(textArea.transform);
            TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            // Add background image
            Image image = inputFieldObj.AddComponent<Image>();
            image.color = Color.white;

            inputField.textComponent = text;
            inputField.textViewport = textAreaRect;

            return inputFieldObj;
        }

        private static GameObject CreateScrollView(Transform parent)
        {
            GameObject scrollView = new GameObject("ScrollView");
            scrollView.transform.SetParent(parent);

            RectTransform rect = scrollView.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.sizeDelta = new Vector2(0, -50); // Leave space for input field
            rect.anchoredPosition = new Vector2(0, 25);

            ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
            Image image = scrollView.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.1f);

            // Create viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollView.transform);
            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.sizeDelta = Vector2.zero;
            viewport.AddComponent<Mask>();
            Image viewportImage = viewport.AddComponent<Image>();
            viewportImage.color = Color.white;

            // Create content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform);
            RectTransform contentRect = content.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.sizeDelta = new Vector2(0, 0);
            VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(10, 10, 10, 10);
            vlg.spacing = 10;
            ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;

            return scrollView;
        }

        private static GameObject CreateDebugPanel(Transform parent)
        {
            GameObject debugPanel = CreatePanel(parent, "DebugPanel");
            RectTransform rect = debugPanel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 0.5f);
            rect.sizeDelta = new Vector2(200, 0);
            
            // Add status texts
            CreateStatusText(debugPanel.transform, "ConnectionStatus", "Connection: Disconnected");
            CreateStatusText(debugPanel.transform, "LastMessage", "Last Message: None");
            CreateStatusText(debugPanel.transform, "EnvironmentStatus", "Environment: Not Loaded");

            // Add test buttons
            CreateButton(debugPanel.transform, "TestConnection", "Test Connection");
            CreateButton(debugPanel.transform, "TestAction", "Test Action");
            CreateToggle(debugPanel.transform, "MockResponses", "Mock Responses");

            return debugPanel;
        }

        private static GameObject CreateStatusText(Transform parent, string name, string defaultText)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent);
            
            RectTransform rect = textObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.sizeDelta = new Vector2(-20, 30);
            rect.pivot = new Vector2(0.5f, 1);
            
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = defaultText;
            tmp.fontSize = 12;
            tmp.color = Color.white;

            return textObj;
        }

        private static GameObject CreateButton(Transform parent, string name, string text)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent);
            
            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.sizeDelta = new Vector2(-20, 30);
            
            Button button = buttonObj.AddComponent<Button>();
            Image image = buttonObj.AddComponent<Image>();
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.alignment = TextAlignmentOptions.Center;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            return buttonObj;
        }

        private static GameObject CreateToggle(Transform parent, string name, string text)
        {
            GameObject toggleObj = new GameObject(name);
            toggleObj.transform.SetParent(parent);
            
            RectTransform rect = toggleObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.sizeDelta = new Vector2(-20, 30);
            
            Toggle toggle = toggleObj.AddComponent<Toggle>();
            
            // Create background
            GameObject background = CreatePanel(toggleObj.transform, "Background");
            Image backgroundImage = background.GetComponent<Image>();
            backgroundImage.color = Color.white;
            
            // Create checkmark
            GameObject checkmark = CreatePanel(background.transform, "Checkmark");
            Image checkmarkImage = checkmark.GetComponent<Image>();
            checkmarkImage.color = Color.black;
            
            // Create label
            GameObject label = new GameObject("Label");
            label.transform.SetParent(toggleObj.transform);
            TextMeshProUGUI tmp = label.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.alignment = TextAlignmentOptions.Left;
            
            RectTransform labelRect = label.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0);
            labelRect.anchorMax = new Vector2(1, 1);
            labelRect.sizeDelta = Vector2.zero;

            return toggleObj;
        }
    }
}