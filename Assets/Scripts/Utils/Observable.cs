using System;

namespace Utils
{
    [Serializable]
    public class Observable<T>
    {
        private T _value;

        public event Action<T> OnChange;

        public T Value
        {
            get => _value;
            set
            {
                if(value.Equals(_value)) return;

                _value = value;
                OnChange?.Invoke(_value);
            }
        }
    }
}