namespace GD.UI.PopupWindows
{
    using Extentions;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    #region Использованные модули

    #endregion

    public class ModalDialogWindow : SingletonUIBase<ModalDialogWindow>
    {
        #region Поля

        [SerializeField]
        private Button _yesButton;

        [SerializeField]
        private Button _noButton;

        [SerializeField]
        private Text _dialogText;

        #endregion

        #region Публичные методы

        public void InvokeModalDialog(string text, UnityAction yesAction, UnityAction noAction = null)
        {
            this._yesButton.onClick.RemoveAllListeners();
            if (yesAction != null)
            {
                this._yesButton.AddAction(yesAction);
            }
            this._yesButton.AddAction(this.Deactivate);

            this._noButton.onClick.RemoveAllListeners();
            if (noAction != null)
            {
                this._noButton.AddAction(yesAction);
            }
            this._noButton.AddAction(this.Deactivate);

            this._dialogText.text = text;

            this.transform.SetAsLastSibling();
            this.Activate();
        }

        public void InvokeModalMessage(string text)
        {
            this._noButton.gameObject.DeactivateGO();

            this._yesButton.Text().text = "OK";

            this._yesButton.onClick.RemoveAllListeners();
            this._yesButton.AddAction(() => this._noButton.gameObject.ActivateGO());
            this._yesButton.AddAction(() => this._yesButton.Text().text = "Yes");
            this._yesButton.AddAction(this.Deactivate);

            this._dialogText.text = text;

            this.transform.SetAsLastSibling();
            this.Activate();
        }

        public void InvokeErrorMessage(string text)
        {
            this._noButton.gameObject.DeactivateGO();

            this._yesButton.Text().text = "OK";

            this._yesButton.onClick.RemoveAllListeners();
            this._yesButton.AddAction(() => this._noButton.gameObject.ActivateGO());
            this._yesButton.AddAction(() => this._yesButton.Text().text = "Yes");
            this._yesButton.AddAction(() => this._dialogText.fontSize = 14);
            this._yesButton.AddAction(() => this._dialogText.transform.GetComponent<Shadow>().enabled = true);
            this._yesButton.AddAction(this.Deactivate);

            this._dialogText.transform.GetComponent<Shadow>().enabled = false;
            this._dialogText.fontSize = 10;
            this._dialogText.text = text;

            this.transform.SetAsLastSibling();
            this.Activate();
        }

        #endregion
    }
}