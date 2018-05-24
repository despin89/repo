namespace GD.UI.PopupWindows
{
    using Extentions;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class MessageBubble : SingletonUIBase<MessageBubble>, IPointerClickHandler
    {
        #region Implementations

        public void OnPointerClick(PointerEventData eventData)
        {
            this.Deactivate();
        }

        #endregion

        public void ShowMessage(string message, Vector3 invokePos, float lifeTime = -1)
        {
            this.Activate();

            if (invokePos != Vector3.zero)
            {
                this.transform.position = invokePos;
            }

            this.transform.GetTextComponent().text = message;

            if (lifeTime > 0)
            {
                this.StopAllCoroutines();
                UEx.DelayedAction(lifeTime, this.Deactivate,
                                                                  () => this.gameObject.activeSelf);
            }
        }
    }
}