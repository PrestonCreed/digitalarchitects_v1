using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Threading.Tasks;

namespace DigitalArchitects.Tests
{
    public class DigitalArchitectsUnityTests
    {
        private DigitalArchitectCore architectCore;
        private UnityWebSocketServer webSocketServer;
        private EnvironmentController environmentController;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            // Create test scene
            GameObject testObj = new GameObject("TestObject");
            architectCore = testObj.AddComponent<DigitalArchitectCore>();
            webSocketServer = testObj.AddComponent<UnityWebSocketServer>();
            environmentController = testObj.AddComponent<EnvironmentController>();
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPackageInitialization()
        {
            Assert.NotNull(architectCore);
            Assert.NotNull(webSocketServer);
            Assert.NotNull(environmentController);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestWebSocketConnection()
        {
            bool connected = false;
            
            webSocketServer.OnConnected += () => connected = true;
            webSocketServer.StartWebSocketServer();
            
            // Wait for connection
            float timeout = Time.time + 5f;
            while (!connected && Time.time < timeout)
            {
                yield return null;
            }
            
            Assert.IsTrue(connected, "WebSocket failed to connect");
        }

        [UnityTest]
        public IEnumerator TestEnvironmentActions()
        {
            var action = new EnvironmentAction
            {
                actionType = "place_model",
                parameters = new Dictionary<string, object>
                {
                    ["model_name"] = "TestCube",
                    ["position"] = Vector3.zero
                }
            };
            
            bool actionComplete = false;
            ActionResult result = null;
            
            environmentController.ExecuteAction(action).ContinueWith(t => 
            {
                actionComplete = true;
                result = t.Result;
            });
            
            // Wait for action completion
            float timeout = Time.time + 5f;
            while (!actionComplete && Time.time < timeout)
            {
                yield return null;
            }
            
            Assert.IsTrue(actionComplete);
            Assert.IsTrue(result.success);
        }

        [UnityTest]
        public IEnumerator TestToolRegistration()
        {
            var registry = architectCore.GetComponent<ActionRegistry>();
            Assert.NotNull(registry);
            
            // Register test action
            var testAction = ScriptableObject.CreateInstance<BaseAction>();
            registry.RegisterAction(testAction);
            
            yield return null;
            
            // Verify registration
            Assert.IsTrue(registry.HasAction(testAction.actionId));
        }
    }
}