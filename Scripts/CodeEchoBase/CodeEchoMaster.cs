/******************************************************************************
 * File: CodeEchoMaster.cs
 * Author: Kirill Yakubov
 * Copyright: © 2025 WednesdayGaming. All rights reserved.
 * 
 * Description:
 * This script provides a custom runtime console for Unity, allowing developers 
 * to execute marked methods via a command-line interface.
 * 
 * License:
 * This script is part of a Unity Asset Store package and is subject to the 
 * Unity Asset Store End User License Agreement. Redistribution or modification 
 * of this script outside of the terms of the license is prohibited.
 *****************************************************************************/




using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CodeEcho
{
    public class CodeEchoMaster : MonoBehaviour
    {
        [SerializeField] private CodeEchoUI m_CodeEchoUI;

        private static CodeEchoMaster _instance;
        public static CodeEchoMaster Instance => _instance;

        private readonly Dictionary<string, Action<object>> commandRegistry = new Dictionary<string, Action<object>>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            m_CodeEchoUI = GetComponent<CodeEchoUI>();
        }

        private void Start()
        {
            RegisterAllCommandsInScene();
            m_CodeEchoUI.AssignMethodList(commandRegistry.Keys.ToList());
        }

        public void RegisterAllCommandsInScene()
        {
            MonoBehaviour[] allMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            foreach (var obj in allMonoBehaviours)
            {
                if (obj is ICodeEcho codeEchoObject)
                {
                    RegisterCommandsFromObject(codeEchoObject);
                }
            }
        }

        public void RegisterCommandsFromObject(object obj)
        {
            var methods = obj.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<CodeEchoMark>() != null);

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<CodeEchoMark>();
                string commandName = attribute.CommandName ?? method.Name;

                if (commandRegistry.ContainsKey(commandName))
                {
                    Debug.LogError($"Command '{commandName}' is already registered.");
                    continue;
                }

                // Ensure the method has exactly one or zero parameters
                var parameters = method.GetParameters();
                if (parameters.Length > 1)
                {
                    Debug.LogError($"Method '{method.Name}' has more than one parameter.");
                    continue;
                }

                // Create delegate for the method
                Action<object> action = (param) =>
                {
                    var parameterToPass = parameters.Length > 0 ? ConvertParameter(method, (string)param) : null;
                    method.Invoke(obj, parameters.Length > 0 ? new object[] { parameterToPass } : new object[] { });
                };

                commandRegistry[commandName] = action;
            }
        }

        public void ExecuteCommand(string commandName, string parameter = null)
        {
            if (commandRegistry.TryGetValue(commandName, out var method))
            {
                method.Invoke(parameter);  // Pass the string parameter to the method
            }
            else
            {
                Debug.LogError($"Command '{commandName}' not found.");
            }
        }

        private object ConvertParameter(MethodInfo method, string stringParameter)
        {
            var parameterInfo = method.GetParameters().FirstOrDefault();
            if (parameterInfo == null) return null;  // No parameter for the method

            if (stringParameter == null)
            {
                throw new ArgumentException("Must have a param");
            }

            var paramType = parameterInfo.ParameterType;

            // Handle basic types
            if (paramType == typeof(int))
            {
                return int.Parse(stringParameter);
            }
            else if (paramType == typeof(float))
            {
                return float.Parse(stringParameter);
            }
            else if (paramType == typeof(string))
            {
                return stringParameter;  // No conversion needed for string
            }
            else
            {
                throw new ArgumentException($"Unsupported parameter type: {paramType}");
            }
        }
    }
}


