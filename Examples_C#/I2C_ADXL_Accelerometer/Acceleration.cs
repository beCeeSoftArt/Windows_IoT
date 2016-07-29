/* Copyright André Spitzner 2003 - 2016 */
namespace bcplanet.IOT.Base.Sensors
{
    /// <summary>
    /// Acceleration class
    /// </summary>
    public class Acceleration
    {
        /// <summary>
        /// X
        /// </summary>
        private double _X;

        /// <summary>
        /// Y
        /// </summary>
        private double _Y;

        /// <summary>
        /// Z
        /// </summary>
        private double _Z;

        /// <summary>
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X
        {
            get { return _X; }
            set { _X = value; }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        /// <summary>
        /// </summary>
        /// <value>
        /// The z.
        /// </value>
        public double Z
        {
            get { return _Z; }
            set { _Z = value; }
        }
    }
}
