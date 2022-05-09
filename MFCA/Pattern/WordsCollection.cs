using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MFCA.Pattern
{
    class WordsCollection:IteratorAggregate
    {
        List<string> _collection = new List<string>();

        bool _direction = false;

        public override IEnumerator GetEnumerator()
        {
            return new StringIterator(this, _direction);
        }

        public void ReverseDirection()
        {
            _direction = !_direction;
        }

        public void AddItem(string item)
        {
            this._collection.Add(item);
        }

        public List<string> getItems()
        {
            return _collection;
        }

        internal string[] ToArray()
        {
            string[] array = new string[_collection.Count];

            int index = 0;

            foreach(var element in _collection)
            {
                array[index] = element;
                index++;
            }

            return array;
        }

        internal WordsCollection CastToWordsCollection(string text)
        {
            this._collection.AddRange(text.Split());

            return this;
        }

        internal void Printer()
        {
            foreach(var item in _collection)
            {
                Console.Write($"{item} ");
            }
        }

        public override bool Contains(string text)
        {
            if (_collection.Contains(text))
            {
                return true;
            }
            return false;
        }
    }
}
