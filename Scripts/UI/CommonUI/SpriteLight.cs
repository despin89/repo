namespace GD.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class SpriteLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        private readonly Color _darkColor = new Color(0.8F, 0.8F, 0.8F);
        private readonly Color _normalColor = new Color(1F, 1F, 1F);
        private Image _image;

        #endregion

        #region Implementations

        public void OnPointerEnter(PointerEventData eventData)
        {
            this._image.color = this._normalColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this._image.color = this._darkColor;
        }

        #endregion

        private void Start()
        {
            this._image = this.GetComponent<Image>();
            this._image.color = this._darkColor;
        }
    }
}