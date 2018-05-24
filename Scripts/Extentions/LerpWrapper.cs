namespace GD.Extentions
{
    using System;
    using UnityEngine;

    public class LerpWrapperRot
    {
        #region Fields

        private float _moveLength;

        private float _startTime;

        private float _endTime;

        private Transform _transform;

        private Vector3 _start;
        private Vector3 _end;

        #endregion

        #region Properties

        private Vector3 Value
        {
            get
            {
                float distCovered = (Time.time - this._startTime) * this._moveLength / this._endTime;
                float fracMove = distCovered / this._moveLength;
                return Vector3.Lerp(this._start, this._end, fracMove);
            }
        }

        #endregion

        #region Constructors

        public LerpWrapperRot(Vector3 start, Vector3 end, float lerpTime, Transform transform)
        {
            if (this._transform == null)
                throw new NullReferenceException("Transform");

            this._moveLength = Vector3.Distance(start, end);
            this._endTime = lerpTime;
            this._start = start;
            this._end = end;
            this._startTime = Time.time;
            this._transform = transform;
        }

        #endregion

        public bool TryMove()
        {
            if (!this._transform.eulerAngles.V3Eq_0001(this._end))
            {
                this._transform.eulerAngles = this.Value;
                return true;
            }

            return false;
        }
    }

    public class LerpWrapperV3
    {
        #region Fields

        private float _moveLength;

        private float _startTime;

        private float _endTime;

        private Transform _transform;

        private Vector3 _start;
        private Vector3 _end;

        #endregion

        #region Properties

        private Vector3 Value
        {
            get
            {
                float distCovered = (Time.time - this._startTime) * this._moveLength / this._endTime;
                float fracMove = distCovered / this._moveLength;
                return Vector3.Lerp(this._start, this._end, fracMove);
            }
        }

        #endregion

        #region Constructors

        public LerpWrapperV3(Vector3 start, Vector3 end, float lerpTime, Transform transform)
        {
            this._transform = transform;

            if (this._transform == null)
                throw new NullReferenceException("Transform");

            this._moveLength = Vector3.Distance(start, end);
            this._endTime = lerpTime;
            this._start = start;
            this._end = end;
            this._startTime = Time.time;
            this._transform = transform;
        }

        #endregion

        public bool TryMove()
        {
            if (!this._transform.position.V3Eq_0001(this._end))
            {
                this._transform.position = this.Value;
                return true;
            }

            return false;
        }
    }

    public class LerpWrapperF
    {
        #region Fields

        private float _moveLength;

        private float _startTime;

        private float _endTime;

        private float _start;
        private float _end;

        #endregion

        #region Properties

        private float Value
        {
            get
            {
                float moveCovered = (Time.time - this._startTime) * this._moveLength / this._endTime;
                float fracMove = moveCovered / this._moveLength;
                return Mathf.Lerp(this._start, this._end, fracMove);
            }
        }

        #endregion

        #region Constructors

        public LerpWrapperF(float start, float end, float lerpTime)
        {
            this._moveLength = Mathf.Abs(start - end);
            this._endTime = lerpTime;
            this._start = start;
            this._end = end;
            this._startTime = Time.time;
        }

        #endregion

        public bool TryMove(ref float target)
        {
            if (!Mathf.Approximately(target, this._end))
            {
                target = this.Value;
                return true;
            }

            return false;
        }
    }
}