/******************************************************************************
 * File: CodeEchoMark.cs
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

namespace CodeEcho
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CodeEchoMark : Attribute
    {
        public string CommandName { get; }
        public Type ParameterType { get; }

        public CodeEchoMark(string commandName = null, Type parameterType = null)
        {
            CommandName = commandName;
            ParameterType = parameterType;
        }

        public CodeEchoMark() { }
    }

}

