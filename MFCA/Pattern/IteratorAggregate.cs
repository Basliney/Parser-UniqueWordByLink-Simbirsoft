using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MFCA.Pattern
{
    abstract class IteratorAggregate:IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
        public abstract bool Contains(string text);
    }
}
