/******************************************************************************
 * File: CodeEchoInput.cs
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
using UnityEngine.InputSystem;

namespace CodeEcho
{
    [RequireComponent(typeof(CodeEchoUI))]
    public class CodeEchoInput : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private InputAction m_activateConsole;
        [SerializeField] private InputAction m_compileCommand;
        [Header("References")]
        [SerializeField] private CodeEchoUI m_CodeEchoUI;

        private void Awake()
        {
            if (m_CodeEchoUI == null)
                m_CodeEchoUI = GetComponent<CodeEchoUI>();
        }

        private void OnEnable()
        {
            m_activateConsole.Enable();
            m_compileCommand.Enable();
            m_activateConsole.performed += ToggleConsoleActivate;
            m_compileCommand.performed += TrySubmit;
        }

        private void OnDisable()
        {
            m_activateConsole.performed -= ToggleConsoleActivate;
            m_compileCommand.performed -= TrySubmit;
            m_activateConsole.Disable();
            m_compileCommand.Disable();
        }

        private void ToggleConsoleActivate(InputAction.CallbackContext ctx)
        {
            if (m_CodeEchoUI)
                m_CodeEchoUI.ToggleConsoleWindow();
        }

        private void TrySubmit(InputAction.CallbackContext ctx)
        {
            if (m_CodeEchoUI)
                m_CodeEchoUI.TrySubmitCommand();

        }

    }
}


