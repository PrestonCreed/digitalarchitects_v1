using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DigitalArchitects.Editor
{
    public abstract class BaseAction : ScriptableObject
    {
        public string actionId;
        public string[] requiredPermissions;
        
        public abstract Task<ActionResult> Execute(Dictionary<string, object> parameters);
        public abstract bool ValidateParameters(Dictionary<string, object> parameters);
    }
}