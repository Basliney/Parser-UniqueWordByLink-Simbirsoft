using System;
using System.Collections.Generic;
using System.Text;

namespace MFCA.Pattern
{
    class MapIterator:Iterator
    {
        private WordsMap _map;

        private int _position = -1;

        private bool _reverse = false;

        public MapIterator(WordsMap map, bool reverse)
        {
            this._map = map;
            this._reverse = reverse;

            if (reverse)
            {
                this._position = _map.GetItems().Count;
            }
        }

        public override object Current()
        {
            try
            {
                return _map.GetItems()[_position];
            }
            catch(Exception e)
            {
                Logger.Write(e.StackTrace);
                Console.WriteLine(e.StackTrace);
            }
            return new Word(string.Empty);
        }

        public override int Key()
        {
            return this._position;
        }

        public override bool MoveNext()
        {
            int updatePosition = this._position + (_reverse ? -1 : 1);
            if (updatePosition >= 0 && updatePosition < _map.GetItems().Count)
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
            this._position = this._reverse ? _map.GetItems().Count - 1 : 0;
        }
    }
}
