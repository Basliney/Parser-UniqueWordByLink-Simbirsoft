using System;
using System.Collections.Generic;
using System.Text;

namespace MFCA.Pattern
{
    class StringIterator:Iterator
    {
        private WordsCollection _collection;

        private int _position = -1;

        private bool _reverse = false;

        public StringIterator(WordsCollection collection, bool reverse)
        {
            this._collection = collection;
            this._reverse = reverse;

            if (reverse)
            {
                this._position = _collection.getItems().Count;
            }
        }

        public override object Current()
        {
            try
            {
                return _collection.getItems()[_position];
            }
            catch(Exception e)
            {
                Logger.Write(e.StackTrace);
                Console.WriteLine(e.StackTrace);
            }
            return string.Empty;
        }

        public override int Key()
        {
            return this._position;
        }

        public override bool MoveNext()
        {
            int updatePosition = this._position + (_reverse ? -1 : 1);
            if (updatePosition >= 0 && updatePosition < _collection.getItems().Count)
            {
                this._position = updatePosition;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Reset()
        {
            this._position = this._reverse ? _collection.getItems().Count - 1 : 0;
        }
    }
}
