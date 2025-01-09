/******************************************************************************
 * File: UIButton.cs
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

using System;
using TMPro;
using UnityEngine;

namespace CodeEcho
{
    public class UIButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text TextField;

        public static event Action<string> OnTextSelected;

        public void AssignText(string newText)
        {
            TextField.text = newText;
        }

        public void SelectText()
        {
            OnTextSelected?.Invoke(TextField.text);
        }
    }

}
