/*
    The MIT License (MIT)

    Copyright (c) 2016 Mike

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.

    FROM https://github.com/prime31/Nez
*/

using System.Collections.Generic;

namespace ChristianGreiner.Duality.Plugins.GrabBag.Collections
{
    /// <summary>
    /// simple static class that can be used to pool Lists
    /// </summary>
    public static class ListPool<T>
    {
        private static readonly Queue<List<T>> objectQueue = new Queue<List<T>>();

        /// <summary>
        /// warms up the cache filling it with a max of cacheCount objects
        /// </summary>
        /// <param name="cacheCount">new cache count</param>
        public static void WarmCache(int cacheCount)
        {
            cacheCount -= objectQueue.Count;
            if (cacheCount > 0)
            {
                for (var i = 0; i < cacheCount; i++)
                    objectQueue.Enqueue(new List<T>());
            }
        }

        /// <summary>
        /// trims the cache down to cacheCount items
        /// </summary>
        /// <param name="cacheCount">Cache count.</param>
        public static void TrimCache(int cacheCount)
        {
            while (cacheCount > objectQueue.Count)
                objectQueue.Dequeue();
        }

        /// <summary>
        /// clears out the cache
        /// </summary>
        public static void ClearCache()
        {
            objectQueue.Clear();
        }

        /// <summary>
        /// pops an item off the stack if available creating a new item as necessary
        /// </summary>
        public static List<T> Obtain()
        {
            if (objectQueue.Count > 0)
                return objectQueue.Dequeue();

            return new List<T>();
        }

        /// <summary>
        /// pushes an item back on the stack
        /// </summary>
        /// <param name="obj">Object.</param>
        public static void Free(List<T> obj)
        {
            objectQueue.Enqueue(obj);
            obj.Clear();
        }
    }
}