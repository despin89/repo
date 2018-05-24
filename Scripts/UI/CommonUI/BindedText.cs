namespace GD.UI
{
    using Core.Localization;
    using UnityEngine;
    using UnityEngine.UI;

    public class BindedText : MonoBehaviour
    {
        #region Fields

        public string Key = string.Empty;

        #endregion

        private void Start()
        {
            this.GetComponent<Text>().text = Localization.GetStringByKey(this.Key);
        }
    }
}