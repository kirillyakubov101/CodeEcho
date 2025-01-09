/******************************************************************************
 * File: CodeEchoUI.cs
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



using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CodeEcho
{
    public class CodeEchoUI : MonoBehaviour
    {
        [SerializeField] private Transform m_CanvasTransform;
        [SerializeField] private TMP_InputField m_InputField;
        [SerializeField] private RectTransform m_MethodListPanel;
        [SerializeField] private RectTransform m_MethodListViewport;
        [SerializeField] private UIButton m_ButtonPrefab;

        private List<string> m_ListOfMethods = new List<string>();
        private Dictionary<string, UIButton> m_SuggestedMethods = new Dictionary<string, UIButton>();
        private CodeEchoMaster m_CodeEchoMaster;

        private void Awake()
        {
            m_CodeEchoMaster = GetComponent<CodeEchoMaster>();
        }

        /// <summary>
        /// Need this method to show the console UI every time the user presses the assigned action key/button
        /// </summary>
        public void ToggleConsoleWindow()
        {
            if (m_CanvasTransform == null)
            {
                Debug.LogError("m_CanvasTransform is NULL, make sure you have a ref to the Console Canvas");
            }
            bool state = m_CanvasTransform.gameObject.activeSelf;
            m_CanvasTransform.gameObject.SetActive(!state);

            if (!state)
            {
                m_InputField.ActivateInputField();
                m_InputField.Select();
            }

        }

        public void AssignMethodList(List<string> list)
        {
            m_ListOfMethods = list;
            foreach (var ele in m_ListOfMethods)
            {
                var instance = Instantiate(m_ButtonPrefab, m_MethodListViewport.transform);
                instance.AssignText(ele);
                instance.gameObject.SetActive(false);
                m_SuggestedMethods.Add(ele, instance);
            }
        }

        /// <summary>
        /// The submit input ('Enter' usually) tries to pass the correct method from the input field
        /// </summary>
        public void TrySubmitCommand()
        {
            if (m_CanvasTransform.gameObject.activeSelf)
            {
                string[] words = m_InputField.text.Split(' ');
                string command = null;
                string param = null;
                if (words.Length > 0)
                {
                    command = words[0];
                    if (words.Length > 1)
                    {
                        param = words[1];
                    }
                }

                m_CodeEchoMaster.ExecuteCommand(command, param);
                m_InputField.text = null;
            }
        }

        private void OnEnable()
        {
            m_InputField.onValueChanged.AddListener(ShowSuggestedMatches);
            UIButton.OnTextSelected += PopulateInputFieldWithSelection;
        }

        private void OnDisable()
        {
            m_InputField.onValueChanged.RemoveAllListeners();
            UIButton.OnTextSelected -= PopulateInputFieldWithSelection;
        }

        private void PopulateInputFieldWithSelection(string selectedText)
        {
            m_InputField.text = selectedText;
            m_MethodListPanel.gameObject.SetActive(false);
        }


        private void ShowSuggestedMatches(string suggested)
        {
            if (suggested.Length == 0) { m_MethodListPanel.gameObject.SetActive(false); return; }
            var list = GetMatchingStrings(suggested, m_ListOfMethods);

            bool atLeastOneMatch = false;
            foreach (var ele in m_SuggestedMethods)
            {
                if (list.Contains(ele.Key))
                {
                    ele.Value.gameObject.SetActive(true);
                    atLeastOneMatch = true;
                }
                else
                {
                    ele.Value.gameObject.SetActive(false);
                }
            }

            m_MethodListPanel.gameObject.SetActive(atLeastOneMatch);
        }

        private List<string> GetMatchingStrings(string input, List<string> collection)
        {
            return collection.Where(str => str.StartsWith(input, System.StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}


