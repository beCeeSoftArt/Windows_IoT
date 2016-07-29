/* Copyright André Spitzner 2003 - 2016 */

namespace bcplanet
{
    /// <summary>
    /// Index element with value tag, see ref ToListWithIndex extension method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IndexElement<T>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexElement{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        public IndexElement(T value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}
