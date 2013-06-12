﻿namespace System.Collections.Generic
{
    using System;
    using System.Diagnostics;

    internal class SortedSetDebugView<T>
    {
        private SortedSet<T> set;

        public SortedSetDebugView(SortedSet<T> set)
        {
            if (set == null)
            {
                throw new ArgumentNullException("set");
            }
            this.set = set;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                return this.set.ToArray();
            }
        }
    }
}

