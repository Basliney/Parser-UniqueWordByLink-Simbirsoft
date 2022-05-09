using System;
using System.Collections;
using System.Collections.Generic;

namespace MFCA.Pattern
{
    abstract class Iterator:IEnumerator
    {
        object IEnumerator.Current => Current();

        public abstract int Key();

        public abstract bool MoveNext();

        public abstract object Current();

        public abstract void Reset();
    }
}
