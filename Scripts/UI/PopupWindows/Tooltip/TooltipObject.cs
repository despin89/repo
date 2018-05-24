namespace GD.UI
{
    using Core.Localization;
    using PopupWindows;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class TooltipObject : MonoBehaviour, ITooltip, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        #region Поля

        public string TooltipKey = string.Empty;

        public string HeaderKey = string.Empty;

        #endregion

        #endregion

        #region Публичные методы

        public string GenerateTooltip()
        {
            return Localization.GetStringByKey(this.TooltipKey);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GenericTooltip.ShowTooltip(this, Localization.GetStringByKey(this.HeaderKey), this.transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GenericTooltip.Deactivate();
        }

        #endregion
    }
}