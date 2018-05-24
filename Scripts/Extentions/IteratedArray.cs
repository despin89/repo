namespace GD.Extentions
{
    public class IteratedArray<T>
    {
        #region Constructors

        public IteratedArray(T[] array)
        {
            this._array = array;
        }

        #endregion

        #region Поля

        private readonly T[] _array;

        private int _index = -1;

        #endregion

        #region Публичные методы

        public void Reset()
        {
            this._index = -1;
        }

        public T GetNext()
        {
            this._index++;

            if (this._index < this._array.Length)
            {
                return this._array[this._index];
            }

            this._index = 0;
            return this._array[this._index];
        }

        public T GetPrevious()
        {
            this._index--;

            if (this._index < 0)
            {
                this._index = this._array.Length - 1;
                return this._array[this._index];
            }

            return this._array[this._index];
        }

        public T GetCurrent()
        {
            if (this._index < 0)
            {
                this._index = 0;
            }

            return this._array[this._index];
        }

        #endregion
    }
}