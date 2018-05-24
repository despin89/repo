namespace GD.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class DescriptedObject : MonoBehaviour
    {
        #region Fields

        public string Description = string.Empty;

        #endregion

        #region Закрытые методы

        private void Start()
        {
            this.GetComponent<Text>().text = this.Description;
        }

        #endregion
    }
}