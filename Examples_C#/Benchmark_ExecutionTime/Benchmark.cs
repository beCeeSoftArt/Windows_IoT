/* Copyright André Spitzner 2003 - 2016 */
using bcplanet.IOT.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bcplanet.IOT.Base.Base
{
    /// <summary>
    /// Simple benchmark
    /// </summary>
    public class Benchmark
    {
        /// <summary>
        /// Benchmarks the time.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="iterations">The iterations.</param>
        /// <returns></returns>
        public static double BenchmarkTime(Action action, int iterations = 10000)
        {
            return Evaluate<TimeWatch>(action, iterations);
        }

        /// <summary>
        /// Benchmarks the time.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="iterations">The iterations.</param>
        /// <returns></returns>
        public static double BenchmarkTime(Action<object> action, int iterations = 10000)
        {
            return Evaluate<TimeWatch>(action, iterations);
        }

        /// <summary>
        /// Benchmarks the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="iterations">The iterations.</param>
        /// <returns></returns>
        private static double Evaluate<T>(Action action, int iterations) where T : IStopwatch, new()
        {
            ////clean Garbage
            //GC.Collect();

            ////wait for the finalizer queue to empty
            //GC.WaitForPendingFinalizers();

            ////clean Garbage
            //GC.Collect();

            var stopwatch = new T();
            var timings = new double[iterations];
            for (var i = 0; i < timings.Length; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                //for (var j = 0; j < iterations; j++)
                action();
                stopwatch.Stop();
                timings[i] = stopwatch.Elapsed.TotalMilliseconds / iterations;
                //print timings[i];
            }
            return NormalizedMean(timings);
        }

        /// <summary>
        /// Evaluates the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="iterations">The iterations.</param>
        /// <returns></returns>
        private static double Evaluate<T>(Action<object> action, int iterations) where T : IStopwatch, new()
        {
            ////clean Garbage
            //GC.Collect();

            ////wait for the finalizer queue to empty
            //GC.WaitForPendingFinalizers();

            ////clean Garbage
            //GC.Collect();

            var stopwatch = new T();
            var timings = new double[iterations];
            for (var i = 0; i < timings.Length; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                //for (var j = 0; j < iterations; j++)
                action(i);
                stopwatch.Stop();
                timings[i] = stopwatch.Elapsed.TotalMilliseconds;// / iterations;
                //print timings[i];
            }
            return NormalizedMean(timings);
        }

        /// <summary>
        /// Normalizeds the mean.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private static double NormalizedMean(ICollection<double> values)
        {
            if (values.Count == 0)
                return double.NaN;

            //double sum = 0;

            //foreach (var value in values)
            //    sum += value;
            //return sum / values.Count;// values.Average();

            var deviations = Deviations(values).ToArray();
            var meanDeviation = deviations.Sum(t => Math.Abs(t.Item2)) / values.Count;
            return deviations.Where(t => t.Item2 > 0 || Math.Abs(t.Item2) <= meanDeviation).Average(t => t.Item1);
        }

        /// <summary>
        /// Deviationses the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private static IEnumerable<Tuple<double, double>> Deviations(ICollection<double> values)
        {
            if (values.Count == 0)
                yield break;

            var avg = values.Average();
            foreach (var d in values)
                yield return Tuple.Create(d, avg - d);
        }
    }
}
