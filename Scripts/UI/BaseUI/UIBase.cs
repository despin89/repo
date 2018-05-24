namespace GD.UI
{
    using Extentions;
    using Game;
    using UnityEngine;

    public abstract class UIBase : MonoBehaviour, IWorld
    {
        #region Публичные методы

        public virtual void SetLastAndActivate()
        {
            this.gameObject.ActivateGO();
            this.gameObject.transform.SetAsLastSibling();
        }

        public virtual void Activate()
        {
            this.gameObject.ActivateGO();
        }

        public virtual void Deactivate()
        {
            this.gameObject.DeactivateGO();
        }

        public void Remove()
        {
            Destroy(this.gameObject);
        }

        public World CurrentWorld
        {
            get { return GameManager.CurrentWorld; }
        }

        #endregion
    }
}