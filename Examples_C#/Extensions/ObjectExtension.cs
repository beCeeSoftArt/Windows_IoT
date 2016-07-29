/* Copyright André Spitzner 2003 - 2016 */
using bcplanet.IOT.Base.Base;
using bcplanet.IOT.Base.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using static System.Decimal;
using static System.Int64;

namespace bcplanet
{
    /// <summary>
    /// Object Extension Class
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Convert String To Byte Array
        /// </summary>
        /// <param name="sText">The String To Convert</param>
        /// <returns>Byte Array Of String</returns>
        public static byte[] ToByteArray(this string sText)
        {
            var oEncoding = new UTF8Encoding();
            return oEncoding.GetBytes(sText);
        }

        /// <summary>
        /// Convert Byte Array To String
        /// </summary>
        /// <param name="oByteArray">The String To Convert</param>
        /// <returns>Byte Array Of String</returns>
        public static string ToString(this byte[] oByteArray)
        {
            var oEncoding = new UTF8Encoding();
            return oEncoding.GetString(oByteArray);
        }

        /// <summary>
        /// Is Numeric Check For Text
        /// </summary>
        /// <param name="o">The Object To Test</param>
        /// <returns>True If Object Is A Number Otherwise False</returns>
        public static bool IsNumeric(this object o)
        {
            var bIsNumber = false;
            try
            {
                double dRetNumber;
                bIsNumber = double.TryParse(Convert.ToString(o), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out dRetNumber);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (bIsNumber);
        }

        /// <summary>
        /// To integer.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int ToInteger(this object o, int defaultValue = default(int))
        {
            var dRetNumber = default(int);
            try
            {
                if (o is int)
                    return Convert.ToInt32(o);

                if (!int.TryParse(Convert.ToString(o), NumberStyles.Any,
                        NumberFormatInfo.InvariantInfo, out dRetNumber))
                    dRetNumber = defaultValue;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (dRetNumber);
        }

        /// <summary>
        /// To the unsigned integer.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static uint ToUnsignedInteger(this object o, uint defaultValue = default(uint))
        {
            var dRetNumber = default(uint);
            try
            {
                if (!uint.TryParse(Convert.ToString(o), NumberStyles.Any,
                        NumberFormatInfo.InvariantInfo, out dRetNumber))
                    dRetNumber = defaultValue;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (dRetNumber);
        }

        /// <summary>
        /// To double.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double ToDouble(this object o, double defaultValue = default(double))
        {
            var dRetNumber = default(double);
            try
            {
                if (o == null)
                    return dRetNumber;

                if (!double.TryParse(Convert.ToString(o).Replace(",", "."), NumberStyles.Any,
                        NumberFormatInfo.InvariantInfo, out dRetNumber))
                    dRetNumber = defaultValue;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (dRetNumber);
        }

        /// <summary>
        /// To decimal.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object o, decimal defaultValue = default(decimal))
        {
            var dRetNumber = default(decimal);
            try
            {
                if (!TryParse(Convert.ToString(o), NumberStyles.Any,
                        NumberFormatInfo.InvariantInfo, out dRetNumber))
                    dRetNumber = defaultValue;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (dRetNumber);
        }

        /// <summary>
        /// To the boolean.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <returns></returns>
        public static bool ToBoolean(this object o)
        {
            var dRetNumber = default(bool);
            try
            {
                // Check int
                if (o is int)
                    switch ((int)o)
                    {
                        case 1:
                        case -1:
                            return true;
                        case 0:
                            return false;
                    }
                // Check double
                if (o is double)
                    switch (o.ToInteger())
                    {
                        case 1:
                        case -1:
                            return true;
                        case 0:
                            return false;
                    }

                // Check string
                if (o is string)
                    switch (o.ToString().ToLower())
                    {
                        case "yes":
                            return true;
                        case "no":
                            return false;
                    }

                // Check the rest
                bool.TryParse(Convert.ToString(o), out dRetNumber);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (dRetNumber);
        }

        /// <summary>
        /// Is Integer Check For Integer
        /// </summary>
        /// <param name="o">The Object To Test</param>
        /// <returns>True If Object Is A Integer Otherwise False</returns>
        public static bool IsInteger(this object o)
        {
            var bIsNumber = false;
            try
            {
                long iRetNumber;
                bIsNumber = TryParse(Convert.ToString(o), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out iRetNumber);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (bIsNumber);
        }

        /// <summary>
        /// Determines whether the specified o is boolean.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <returns></returns>
        public static bool IsBoolean(this object o)
        {
            var bIsNumber = false;
            try
            {
                bool iRetNumber;
                bIsNumber = bool.TryParse(Convert.ToString(o), out iRetNumber);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (bIsNumber);
        }

        /// <summary>
        /// Is Integer Check For DateTime
        /// </summary>
        /// <param name="o">The Object To Test</param>
        /// <returns>True If Object Is A DateTime Otherwise False</returns>
        public static bool IsDateTime(this object o)
        {
            var bIsDateTime = false;
            try
            {
                DateTime tRetDateTime;
                bIsDateTime = DateTime.TryParse(Convert.ToString(o), out tRetDateTime);
                if (!bIsDateTime)
                {
                    var dateTimeParts = Convert.ToString(o).Split(' ');
                    if (dateTimeParts.Length > 1)
                        bIsDateTime = DateTime.TryParse($"{dateTimeParts[0]} {dateTimeParts[1].Replace("-", ":")}", out tRetDateTime);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (bIsDateTime);
        }

        /// <summary>
        /// Is Integer Check For DateTime
        /// </summary>
        /// <param name="o">The Object To Test</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// True If Object Is A DateTime Otherwise False
        /// </returns>
        public static bool ParseToDateTime(this object o, out DateTime dateTime)
        {
            var bIsDateTime = false;
            dateTime = new DateTime(1977, 10, 31);
            try
            {
                bIsDateTime = DateTime.TryParse(Convert.ToString(o), CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime);
                if (!bIsDateTime)
                {
                    var dateTimeParts = Convert.ToString(o).Split(' ');
                    if (dateTimeParts.Length > 1)
                    {
                        bIsDateTime = DateTime.TryParse($"{dateTimeParts[0]} {dateTimeParts[1].Replace("-", ":")}", out dateTime);

                        if (!bIsDateTime)
                        {
                            var timeString = Convert.ToString(o);

                            if (timeString.Length > 4
                                && timeString[timeString.Length - 4] == ':')
                            {
                                bIsDateTime = DateTime.TryParse(timeString.Substring(0, timeString.Length - 4), out dateTime);
                                if (bIsDateTime)
                                    dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                                        dateTime.Minute, dateTime.Second,
                                        timeString.Substring(timeString.Length - 3).ToInteger());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (bIsDateTime);
        }

        /// <summary>
        /// Adds the random offset.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public static int AddRandomOffset(this int o, int range)
        {
            var dRetNumber = default(int);
            try
            {
                var random = new Random();
                dRetNumber = o + random.Next(range);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ELogTypes.Error, ex);
            }
            return (dRetNumber);
        }

        /// <summary>
        /// Fire Event
        /// </summary>
        /// <param name="o">The Object Instance</param>
        /// <param name="e">The Event</param>
        /// <param name="sender">The Sender Object</param>
        public static void FireEvent(this EventHandler<EventArgs> o, object sender, EventArgs e)
        {
            o?.Invoke(sender, e);
        }

        /// <summary>
        /// Fire Event
        /// </summary>
        /// <param name="o">The Inctance</param>
        /// <param name="e">The Event</param>
        /// <param name="sender">The Sender Object</param>
        public static void FireEvent(this EventHandler o, object sender, EventArgs e)
        {
            var handler = o;
            handler?.Invoke(sender, e);
        }

        /// <summary>
        /// Fire Event
        /// </summary>
        /// <param name="o">The Inctance</param>
        /// <param name="e">The Event</param>
        /// <param name="sender">The Sender Object</param>
        public static void FireEvent<TEventArgs>(this EventHandler<TEventArgs> o, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            var handler = o;
            handler?.Invoke(sender, e);
        }

        /// <summary>
        /// Fire Event
        /// </summary>
        /// <param name="o">The Inctance</param>
        /// <param name="e">The Event</param>
        /// <param name="sender">The Sender Object</param>
        public static void FireEvent(this PropertyChangedEventHandler o, object sender, PropertyChangedEventArgs e)
        {
            var handler = o;
            handler?.Invoke(sender, e);
        }

        /// <summary>
        /// Tries the again if fails.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="tries">The tries.</param>
        /// <param name="throwLastException">if set to <c>true</c> [throw last exception].</param>
        /// <param name="tryExecuteAction">The try execute action.</param>
        /// <returns></returns>
        public static bool TryAgainIfFails(this object o, int tries, bool throwLastException, Action tryExecuteAction)
        {
            if (tryExecuteAction == null)
                return false;

            Exception lastException = null;
            for (var i = 0; i < tries; i++)
            {
                try
                {
                    tryExecuteAction();

                    return true;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }

            if (!throwLastException)
                return false;

            if (lastException != null)
                throw lastException;

            return false;
        }

        /// <summary>
        /// Tries the again if fails.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The object.</param>
        /// <param name="tries">The tries.</param>
        /// <param name="throwLastException">if set to <c>true</c> [throw last exception].</param>
        /// <param name="tryExecuteAction">The try execute action.</param>
        /// <returns></returns>
        public static T TryAgainIfFails<T>(this object o, int tries, bool throwLastException, Func<T> tryExecuteAction)
        {
            if (tryExecuteAction == null)
                return default(T);

            Exception lastException = null;
            for (var i = 0; i < tries; i++)
                try
                {
                    return tryExecuteAction();
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

            if (!throwLastException)
                return default(T);

            if (lastException != null)
                throw lastException;

            return default(T);
        }

        /// <summary>
        /// Distincts the by.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }


        /// <summary>
        /// Distincts the by.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">
        /// source
        /// or
        /// keySelector
        /// </exception>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new NullReferenceException("source");

            if (keySelector == null)
                throw new NullReferenceException("keySelector");

            return DistinctByImpl(source, keySelector, comparer);
        }

        /// <summary>
        /// Distincts the by implementation.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var knownKeys = new HashSet<TKey>(comparer);
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }


        /// <summary>
        /// To the index of the list with.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static List<IndexElement<T>> ToListWithIndex<T>(this IEnumerable<T> list)
        {
            List<IndexElement<T>> returnValue;

            if (list == null)
                return null;

            var enumerable = list as IList<T> ?? list.ToList();
            lock (enumerable)
                returnValue = enumerable.Select(((v, i) => new IndexElement<T>(v, i))).ToList();
            return returnValue;
        }

        /// <summary>
        /// To the type of the string.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <returns></returns>
        public static string ToStringType(this object o)
        {
            return o?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Averages the time span.
        /// </summary>
        /// <param name="timeSpanList">The time span list.</param>
        /// <returns></returns>
        public static TimeSpan AverageTimeSpan(this List<TimeSpan> timeSpanList)
        {
            if (timeSpanList == null
                || timeSpanList.Count == 0)
                return new TimeSpan();

            var averageTicks = Convert.ToInt64(timeSpanList.Average(timeSpan => timeSpan.Ticks));
            return new TimeSpan(averageTicks);
        }

        /// <summary>
        /// To the binary.
        /// </summary>
        /// <param name="numeral">The numeral.</param>
        /// <returns></returns>
        public static BitArray ToBinary(this int numeral)
        {
            return new BitArray(new[] { numeral });
        }

        /// <summary>
        /// Prefixes for numbers
        /// Since a long can only hold 9.2E18 (ie., exa territory) we
        /// don't need zetta, yotta, xona, weka, vunda, uda, treda, sorta, 
        /// rinta, quexa, pepta, ocha, nena, minga, or luma
        /// Just so you know...
        /// </summary>
        private static readonly string[] SiUnitSizes =
        {
            " ",
            "Kilo",
            "Mega",
            "Giga",
            "Tera",
            "Peta",
            "Exa"
        };

        /// <summary>
        /// Format a set of bytes into a human readable format
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        /// <exception cref="System.ArithmeticException">64 bit numbers are bigger than they should be...</exception>
        public static string ToReadableSiUnit(this long value, string unit = "")
        {
            // Get the exponent
            // Since logn(x) = y means "Multiply 'n' by itself 'y' times to get 'x'",
            // the integer part of the log base 10 of any number is the exponent.
            // (This is called the "characteristic" in math parlance)
            // Since we are using longs which don't have a faractional part, this can't be negative.
            int exponent = (int)Math.Log10(value);
            int group = exponent / 3;
            if (group >= SiUnitSizes.Length)
                throw new ArithmeticException("64 bit numbers are bigger than they should be...");
            double divisor = Math.Pow(10, group * 3);
            return $"{value / divisor:0.0} {SiUnitSizes[@group]}{unit}";
        }

        /// <summary>
        /// To the readable si unit.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static string ToReadableSiUnit(this int value, string unit = "")
        {
            return ToReadableSiUnit((long)value, unit);
        }

        /// <summary>
        /// To the double from date time.
        /// </summary>
        /// <param name="theDate">The date.</param>
        /// <returns></returns>
        public static double ToDoubleFromDateTime(this DateTime theDate)
        {
            return (long)(theDate - new DateTime(2016, 1, 1)).TotalMilliseconds;
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="intDate">The int date.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this double intDate)
        {
            return new DateTime(2016, 1, 1).AddMilliseconds(intDate);
        }

    }
}
