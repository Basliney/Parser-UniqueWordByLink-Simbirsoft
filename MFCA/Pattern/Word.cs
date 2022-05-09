using System;
using System.Collections.Generic;
using System.Text;

namespace MFCA.Pattern
{
    class Word
    {
        private string _key;
        private int _value;

        #region property
        public string Key
        {
            get { return _key; }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion

        public Word(string key)
        {
            this._key = key;
            this._value = 0;
        }

        public Word(string key, int value)
        {
            this._key = key;
            this._value = value;
        }
    }
}
