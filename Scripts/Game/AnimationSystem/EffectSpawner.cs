namespace GD.Game.AnimationSystem
{
    using Extentions;
    using UnityEngine;
    using UnityEngine.Playables;

    public class EffectSpawner : MonoBehaviour
    {
        #region Fields

        private float _length;

        private bool _isFired;

        private PlayableDirector _director;

        #endregion

        public void Instantiate(Transform parent, float length, bool flip)
        {
            EffectSpawner spawner = Instantiate(this.gameObject, parent, false).GetComponent<EffectSpawner>();
            spawner._length = length;
            spawner.Play();
        }

        private void Play()
        {
            this._director = this.GetComponent<PlayableDirector>();
            this._director.Play();

            double speed = this._director.playableGraph.GetRootPlayable(0).GetDuration() / this._length;
            this._director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
            this._isFired = true;
        }

        private void Update()
        {
            if (this._isFired && this._director.state != PlayState.Playing )
            {
                this.DestroyEffectDelayed();
            }
        }

        private void DestroyEffectDelayed()
        {
            if (this.gameObject != null)
            {
                this.gameObject.Destroy();
            }
        }
    }
}