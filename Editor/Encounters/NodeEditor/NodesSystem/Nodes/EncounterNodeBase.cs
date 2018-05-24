namespace GD.EditorExtentions
{
    using System;
    using Game.EncountersSystem;
    using UnityEngine;

    public abstract class EncounterNodeBase : Node
    {
        public EncounterInfo Encounter;

        protected EncounterNodeBase(int id, Graph parent) : base(id, parent)
        {
            this.Encounter = ScriptableObject.CreateInstance<EncounterInfo>();
            this.Encounter.Id = Guid.NewGuid().ToString();
        }
    }
}
