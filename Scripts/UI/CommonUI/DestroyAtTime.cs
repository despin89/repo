namespace GD.UI.ScenesUI.Combat
{
    using UnityEngine;

    public class DestroyAtTime : MonoBehaviour
    {
        #region Fields

        public float Timer = 1F;

        #endregion

        private void Start()
        {
            Destroy(this.gameObject, this.Timer);
        }
    }
}