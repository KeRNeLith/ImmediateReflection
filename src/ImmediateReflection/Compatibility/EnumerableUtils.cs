#if !SUPPORTS_LINQ
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ImmediateReflection.Utils
{
    /// <summary>
    /// Helper to replace Enumerable standard utilities when not available in target version.
    /// </summary>
    internal static class EnumerableUtils
    {
        // Predicate delegate
        internal delegate bool Predicate<in T>(T param);

        /// <summary>
        /// Gets the first element in the <paramref name="source"/> enumerable that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Source enumerable.</param>
        /// <param name="predicate">Predicate to check on each source element.</param>
        /// <returns>First element matching <paramref name="predicate"/>.</returns>
        [Pure]
        [NotNull]
        public static T First<T>([NotNull, ItemNotNull] IEnumerable<T> source, [NotNull, InstantHandle] Predicate<T> predicate)
        {
            foreach (T element in source)
            {
                if (predicate(element))
                    return element;
            }

            throw new InvalidOperationException("No element matching the given predicate.");
        }

        /// <summary>
        /// Gets a sub set enumerable of <paramref name="source"/> elements matching the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Source enumerable.</param>
        /// <param name="predicate">Predicate to check on each source element.</param>
        /// <returns>Enumerable of elements matching <paramref name="predicate"/>.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<T> Where<T>([NotNull, ItemNotNull] IEnumerable<T> source, [NotNull, InstantHandle] Predicate<T> predicate)
        {
            foreach (T element in source)
            {
                if (predicate(element))
                    yield return element;
            }
        }

        /// <summary>
        /// Gets a sub set enumerable of source elements cast to <typeparamref name="TResult"/>. 
        /// </summary>
        /// <typeparam name="TResult">Output element type.</typeparam>
        /// <param name="source">Source enumerable.</param>
        /// <returns>Enumerable of elements cast to <typeparamref name="TResult"/>.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<TResult> OfType<TResult>([NotNull, ItemNotNull] IEnumerable source)
        {
            foreach (object obj in source)
            {
                if (obj is TResult element)
                    yield return element;
            }
        }

        private static class EmptyEnumerable<TElement>
        {
            [NotNull, ItemNotNull]
            public static readonly TElement[] Instance = new TElement[0];
        }

        /// <summary>
        /// Gets an empty enumerable of <typeparamref name="T"/> elements.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>Empty enumerable.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<T> Empty<T>()
        {
            return EmptyEnumerable<T>.Instance;
        }

        /// <summary>
        /// Gets an enumerable and return it as a enumerable sequence.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Source enumerable.</param>
        /// <returns>Enumerable.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<T> AsEnumerable<T>([NotNull, ItemNotNull] IEnumerable<T> source)
        {
            foreach (T element in source)
                yield return element;
        }

        #region Buffer helper

        /// <summary>
        /// Buffer of elements.
        /// </summary>
        /// <typeparam name="TElement">Element type.</typeparam>
        private struct Buffer<TElement>
        {
            private readonly TElement[] _items;
            private readonly int _count;

            internal Buffer([NotNull, ItemNotNull] IEnumerable<TElement> source)
            {
                TElement[] items = null;
                int count = 0;
                if (source is ICollection<TElement> collection)
                {
                    count = collection.Count;
                    if (count > 0)
                    {
                        items = new TElement[count];
                        collection.CopyTo(items, 0);
                    }
                }
                else
                {
                    foreach (TElement item in source)
                    {
                        if (items is null)
                        {
                            items = new TElement[4];
                        }
                        else if (items.Length == count)
                        {
                            var newItems = new TElement[checked(count * 2)];
                            Array.Copy(items, 0, newItems, 0, count);
                            items = newItems;
                        }

                        items[count] = item;
                        ++count;
                    }
                }

                _items = items;
                _count = count;
            }

            internal TElement[] ToArray()
            {
                if (_count == 0)
                    return new TElement[0];

                if (_items.Length == _count)
                    return _items;

                var result = new TElement[_count];
                Array.Copy(_items, 0, result, 0, _count);
                return result;
            }
        }

        #endregion

        /// <summary>
        /// Converts the given enumerable to an array.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Source enumerable.</param>
        /// <returns>Enumerable converted to array.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public static T[] ToArray<T>([NotNull, ItemNotNull] IEnumerable<T> source)
        {
            return new Buffer<T>(source).ToArray();
        }

        /// <summary>
        /// Gets an enumerable of <paramref name="first"/>values excepting those in <paramref name="second"/>.
        /// </summary>
        /// <param name="first">Enumerable of values.</param>
        /// <param name="second">Enumerable of values to except.</param>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>Source element that were not present in <paramref name="second"/>.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<T> Except<T>([NotNull, ItemNotNull] IEnumerable<T> first, [NotNull, ItemNotNull] IEnumerable<T> second)
        {
            var list = new List<T>(second);
            foreach (T source in first)
            {
                if (!list.Contains(source))
                    yield return source;
            }
        }
    }
}
#endif