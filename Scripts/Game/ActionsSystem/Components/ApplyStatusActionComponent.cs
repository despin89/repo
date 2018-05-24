namespace GD.Game.ActionsSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MoonSharp.Interpreter;
    using StatusesSystem;

    [MoonSharpUserData]
    [Serializable]
    public class ApplyStatusActionComponent : ComponentBase
    {
        #region Properties

        public StatusTarget ApplyTarget { get; }

        public StatusType ApplyType { get; }

        public List<Status> Statuses { get; }

        #endregion

        #region Constructors

        public ApplyStatusActionComponent(ApplyStatusActionComponentInfo info)
        {
            this.ApplyTarget = info.ApplyTarget;
            this.ApplyType = info.ApplyType;

            this.Statuses = info.StatusInfos.Select(_ => new Status(_)).ToList();
        }

        #endregion
    }
}