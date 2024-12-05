using System;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class UnityWebSocketServerBehavior : WebSocketBehavior
{
    public string ApiKey { get; set; }
    public Action<string> OnMessageReceivedCallback { get; set; }
    public Action OnClientConnectedCallback { get; set; }
    public Action OnClientDisconnectedCallback { get; set; }
    public Action<string> OnErrorCallback { get; set; }

    protected override void OnOpen()
    {
        base.OnOpen();
        OnClientConnectedCallback?.Invoke();
    }

    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        OnClientDisconnectedCallback?.Invoke();
    }

    protected override void OnError(ErrorEventArgs e)
    {
        base.OnError(e);
        OnErrorCallback?.Invoke(e.Message);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        try
        {
            OnMessageReceivedCallback?.Invoke(e.Data);
            HandleMessage(e.Data);
        }
        catch (Exception ex)
        {
            OnErrorCallback?.Invoke(ex.Message);
            SendError("Error processing message");
        }
    }

    private void HandleMessage(string message)
    {
        try
        {
            JObject json = JObject.Parse(message);
            
            // Validate message structure
            if (!ValidateMessage(json, out string error))
            {
                SendError(error);
                return;
            }

            string action = json["action"]?.ToString();
            switch (action.ToLower())
            {
                case "import_model":
                    ImportModel(json);
                    break;
                case "place_model":
                    PlaceModel(json);
                    break;
                case "analyze_environment":
                    AnalyzeEnvironment();
                    break;
                default:
                    SendError($"Unknown action: {action}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing message: {ex.Message}");
            SendError("Invalid message format.");
        }
    }

    private bool ValidateMessage(JObject message, out string error)
    {
        error = null;

        // Check API key for non-system messages
        if (message["category"]?.ToString() != "system" && 
            message["api_key"]?.ToString() != ApiKey)
        {
            error = "Invalid API key";
            return false;
        }

        if (!message.ContainsKey("action"))
        {
            error = "Missing action field";
            return false;
        }

        string action = message["action"].ToString().ToLower();
        switch (action)
        {
            case "import_model":
                if (!message.ContainsKey("model_path"))
                {
                    error = "model_path is required for import_model action";
                    return false;
                }
                break;

            case "place_model":
                if (!message.ContainsKey("model_name") || !message.ContainsKey("coordinates"))
                {
                    error = "model_name and coordinates are required for place_model action";
                    return false;
                }
                break;
        }

        return true;
    }

    private void ImportModel(JObject json)
    {
        string modelPath = json["model_path"]?.ToString();
        if (string.IsNullOrEmpty(modelPath))
        {
            SendError("model_path is required for import_model action.");
            return;
        }

        // Implement model import logic here
        Debug.Log($"Importing model from {modelPath}");
        // TODO: Add actual model import code

        JObject response = new JObject
        {
            ["status"] = "success",
            ["message"] = $"Model imported from {modelPath}."
        };
        Send(response.ToString());
    }

    private void PlaceModel(JObject json)
    {
        string modelName = json["model_name"]?.ToString();
        JObject coordinates = (JObject)json["coordinates"];

        if (string.IsNullOrEmpty(modelName) || coordinates == null)
        {
            SendError("model_name and coordinates are required for place_model action.");
            return;
        }

        float x = coordinates["x"]?.ToObject<float>() ?? 0f;
        float y = coordinates["y"]?.ToObject<float>() ?? 0f;
        float z = coordinates["z"]?.ToObject<float>() ?? 0f;

        // Implement model placement logic here
        Debug.Log($"Placing model '{modelName}' at ({x}, {y}, {z})");
        // TODO: Add actual model placement code

        JObject response = new JObject
        {
            ["status"] = "success",
            ["message"] = $"Model '{modelName}' placed at ({x}, {y}, {z})."
        };
        Send(response.ToString());
    }

    private void AnalyzeEnvironment()
    {
        // Implement environment analysis logic here
        Debug.Log("Analyzing environment to create an internal map.");
        
        JObject internalMap = new JObject
        {
            ["buildings"] = new JArray("Sheriff's Office", "Store", "Bank"),
            ["roads"] = new JArray("Main Street", "2nd Avenue"),
            ["parks"] = new JArray("Central Park")
        };

        JObject response = new JObject
        {
            ["status"] = "success",
            ["internal_map"] = internalMap
        };
        Send(response.ToString());
    }

    private void SendError(string errorMessage)
    {
        JObject response = new JObject
        {
            ["status"] = "failure",
            ["error"] = errorMessage
        };
        Send(response.ToString());
    }
}

public class UnityWebSocketServer : MonoBehaviour
{
    private WebSocketServer wssv;
    public int port = 8080;
    
    // Events
    public event Action<string> OnMessageReceived;
    public event Action OnConnected;
    public event Action OnDisconnected;
    public event Action<string> OnError;

    // Connection state
    public bool IsRunning { get; private set; }
    private readonly Queue<string> messageQueue = new Queue<string>();
    
    [SerializeField] private bool autoReconnect = true;
    [SerializeField] private float reconnectDelay = 5f;
    [SerializeField] private string apiKey = "default_key";

    void Start()
    {
        StartWebSocketServer();
    }

    public void StartWebSocketServer()
    {
        try 
        {
            wssv = new WebSocketServer(port);
            
            wssv.WaitTime = TimeSpan.FromSeconds(1);
            wssv.KeepClean = true;

            var config = new WebSocketServiceConfig
            {
                IgnoreExtensions = true,
                EnableRedirection = false
            };

            wssv.AddWebSocketService<UnityWebSocketServerBehavior>("/unity", () => 
            {
                var behavior = new UnityWebSocketServerBehavior
                {
                    ApiKey = apiKey,
                    OnMessageReceivedCallback = HandleMessage,
                    OnClientConnectedCallback = HandleClientConnected,
                    OnClientDisconnectedCallback = HandleClientDisconnected,
                    OnErrorCallback = HandleError
                };
                return behavior;
            }, config);

            wssv.Start();
            IsRunning = true;
            Debug.Log($"Unity WebSocket Server started on ws://localhost:{port}/unity");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to start WebSocket server: {e.Message}");
            OnError?.Invoke(e.Message);
            if (autoReconnect)
                StartCoroutine(AutoReconnect());
        }
    }

    private IEnumerator AutoReconnect()
    {
        while (!IsRunning && autoReconnect)
        {
            yield return new WaitForSeconds(reconnectDelay);
            Debug.Log("Attempting to reconnect WebSocket server...");
            StartWebSocketServer();
        }
    }

    public void SendMessage(string message)
    {
        if (!IsRunning)
        {
            messageQueue.Enqueue(message);
            return;
        }

        try
        {
            wssv.WebSocketServices["/unity"].Sessions.Broadcast(message);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send message: {e.Message}");
            messageQueue.Enqueue(message);
            OnError?.Invoke(e.Message);
        }
    }

    private void HandleMessage(string message)
    {
        OnMessageReceived?.Invoke(message);
    }

    private void HandleClientConnected()
    {
        OnConnected?.Invoke();
        // Process queued messages
        while (messageQueue.Count > 0 && IsRunning)
        {
            SendMessage(messageQueue.Dequeue());
        }
    }

    private void HandleClientDisconnected()
    {
        OnDisconnected?.Invoke();
    }

    private void HandleError(string error)
    {
        OnError?.Invoke(error);
    }

    private void OnDestroy()
    {
        StopWebSocketServer();
    }

    public void StopWebSocketServer()
    {
        if (wssv != null)
        {
            wssv.Stop();
            IsRunning = false;
            Debug.Log("Unity WebSocket Server stopped.");
        }
    }
}