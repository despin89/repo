namespace GD.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class PutTopOnClick : MonoBehaviour, IPointerClickHandler
    {
        #region Implementations

        public void OnPointerClick(PointerEventData eventData)
        {
            this.transform.SetAsLastSibling();
        }

        #endregion
    }
}