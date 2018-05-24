namespace GD.UI.PopupWindows
{
    using Extentions;
    using UnityEngine;
    using UnityEngine.UI;

    public class GenericTooltip : SingletonUIBase<GenericTooltip>
    {
        [SerializeField]
        private Text _header;

        [SerializeField]
        private Text _tooltip;

        #region Публичные методы

        public static void ShowTooltip(ITooltip thing, string header, Vector3 invokePos)
        {
            Instance.Activate();
            Instance.transform.position = invokePos;

            Instance._header.text = header;
            Instance._tooltip.text = thing.GenerateTooltip();
        }

        public new static void Deactivate()
        {
            Instance.gameObject.DeactivateGO();
        }

        #endregion
    }
}