namespace GD.Game.ActionsSystem
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class ComponentInfoBase : ScriptableObject
    {
        public abstract ComponentBase GetInstance();
    }
}