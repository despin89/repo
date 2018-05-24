using System;
using UnityEngine;

namespace GD.EditorExtentions
{
    public static class Log
    {


        public static void Info(String info)
        {
            if (NodeEditorConfig.LogLevel > 0)
            {
                Debug.Log(info);
            }
        }

    }
}

