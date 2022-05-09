using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MFCA.Pattern
{
    class WordsMap:IteratorAggregate
    {
        private List<Word> _wordsMap;

        public WordsMap(WordsCollection wordsCollection)
        {
            _wordsMap = new List<Word>();

            foreach (var word in wordsCollection)
            {
                AddWord((string)word);
            }
        }
        
        public WordsMap()
        {
            _wordsMap = new List<Word>();
        }
        
        /// <summary>
        /// Добавление текста в список
        /// </summary>
        /// <param name="word">Текст</param>
        /// <returns>Результат добавления текста в список: 1 - добавлен новый текст; 2 - увеличен счетчик повторений</returns>
        public int AddWord(string word)
        {
            int index = InList(word);
            if(index == -1)
            {
                _wordsMap.Add(new Word(word, 1));
                return 1;
            }
            else
            {
                _wordsMap[index].Value++;
                return 0;
            }
        }

        /// <summary>
        /// Проверка нахождение текста в списке
        /// </summary>
        /// <param name="key">Искомый текст</param>
        /// <returns>Индекс найденного соответствия</returns>
        private int InList(string key)
        {
            for (int i = 0; i < _wordsMap.Count; i++)
            {
                if (_wordsMap[i].Key.Equals(key)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Сортировка по возрастанию
        /// </summary>
        /// <param name="increase">Возрастание</param>
        internal void Sort(bool increase)
        {
            if (increase)
            {
                _wordsMap.Sort(CompareByValue);
            }
        }

        #region Sort
        private int CompareByValue(Word a, Word b)
        {
            if (a == null)
            {
                if (b == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (b == null)
                {
                    return 1;
                }
                else
                {
                    return a.Value.CompareTo(b.Value);
                }
            }
        }
        #endregion

        /// <summary>
        /// Вывод списка слов с количеством повторений
        /// </summary>
        public void Printer()
        {
            foreach (var word in _wordsMap)
            {
                Console.WriteLine($"{word.Key} - {word.Value}");
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return new MapIterator(this, false);
        }

        public List<Word> GetItems()
        {
            return _wordsMap;
        }

        public override bool Contains(string text)
        {
            foreach(var word in _wordsMap)
            {
                if (word.Key.Equals(text))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
