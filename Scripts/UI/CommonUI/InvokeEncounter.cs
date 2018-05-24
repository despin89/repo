namespace GD.UI.ScenesUI
{
    using Game.Combat;
    using UnityEngine;

    public class InvokeEncounter : MonoBehaviour
    {
        public void Invoke()
        {
            CombatManager.LoadEncounter();
        }
    }
}
