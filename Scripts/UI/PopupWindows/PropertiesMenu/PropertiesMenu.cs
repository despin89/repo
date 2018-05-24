namespace GD.UI.PopupWindows
{
    using Core;
    using Extentions;
    using UnityEngine;

    public class PropertiesMenu : Singleton<PropertiesMenu>
    {
        #region Публичные методы

        public void ShowMenu(Vector3 invokePos)
        {
            this.gameObject.ActivateGO();
            this.transform.position = invokePos;
        }

        #endregion

        #region Закрытые методы

        private void Start()
        {
            this.gameObject.DeactivateGO();
        }

        #endregion
    }
}