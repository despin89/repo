namespace GD.UI
{
    using UnityEngine;

    public class SingletonUIBase<T> : UIBase
        where T : Component
    {
        #region Properties

        public static T Instance { get; private set; }

        #endregion

        #region Публичные методы

        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        #endregion

        private void Start()
        {
            this.Deactivate();
        }
    }
}