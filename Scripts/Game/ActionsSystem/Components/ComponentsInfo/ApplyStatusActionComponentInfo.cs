using System;
using System.Collections.Generic;
namespace GD.Game.ActionsSystem
{
    using StatusesSystem;

    [Serializable]
    public class ApplyStatusActionComponentInfo : ComponentInfoBase
    {
        public List<StatusInfo> StatusInfos = new List<StatusInfo>();

        public StatusTarget ApplyTarget = StatusTarget.Target;

        public StatusType ApplyType = StatusType.ApplyOnHit;

        public override ComponentBase GetInstance()
        {
            return new ApplyStatusActionComponent(this);
        }
    }

    public enum StatusType
    {
        ApplyOnHit,
        ApplyOnTurnBegin,
        ApplyOnCrit
    }

    public enum StatusTarget
    {
        Self,
        Target
    }
}
