/*
    MIT License

    Copyright (c) 2017 Andrei Kurosh

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

    FROM: https://github.com/impworks/corund
*/

using ChristianGreiner.Duality.Plugins.GrabBag.Extensions;
using Duality;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ChristianGreiner.Duality.Plugins.GrabBag.Helpers
{
    public static class NumericHelper
    {
        /// <summary>
        /// The mininum value to take into account when modifying object properties.
        /// Prevents coordinates, angle, etc from jitter degradation over time.
        /// </summary>
        public const float Epsilon = 0.0001f;

        /// <summary>
        /// Check whether a number is too small to account for.
        /// </summary>
        public static bool IsAlmostNull(this float number)
        {
            return number < Epsilon
                   && number > -Epsilon;
        }

        /// <summary>
        /// Check whether two floating point numbers are almost equal (to a given precision).
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="compareTo">Other number</param>
        /// <param name="precision">Precision.</param>
        public static bool IsAlmost(this float number, float compareTo, float precision = Epsilon)
        {
            return (number <= compareTo + precision) && (number >= compareTo - precision);
        }

        /// <summary>
        /// clamps value between 0 and 1
        /// </summary>
        /// <param name="value">Value.</param>
        public static float Clamp01(float value)
        {
            if (value < 0f)
                return 0f;

            if (value > 1f)
                return 1f;

            return value;
        }

        public static float CurveAngle(float from, float to, float step)
        {
            // http://circlessuck.blogspot.de/2012/07/xna-smooth-sprite-rotation.html

            if (step == 0) return from;
            if (from == to || step == 1) return to;

            Vector2 fromVector = new Vector2(MathF.Cos(from), MathF.Sin(from));
            Vector2 toVector = new Vector2(MathF.Cos(to), MathF.Sin(to));

            Vector2 currentVector = Slerp(fromVector, toVector, step);

            var result = MathF.Atan2(currentVector.Y, currentVector.X);

            if (float.IsNaN(result))
                return 0;

            return result;
        }

        private static Vector2 Slerp(Vector2 from, Vector2 to, float step)
        {
            if (step == 0.0f) return from;
            if (from == to || step == 1) return to;

            var theta = MathF.Acos(Vector2.Dot(from, to));
            if (theta == 0.0f) return to;

            var sinTheta = MathF.Sin(theta);
            return (MathF.Sin((1 - step) * theta) / sinTheta) * from + MathF.Sin(step * theta) / sinTheta * to;
        }

        private static readonly Regex _extractNumbers = new Regex(@"(?<num>[0-9]+(\.[0-9]+)?)",
    RegexOptions.ExplicitCapture);

        public static void Clamp(ref float value, ref float min, ref float max, out float result)
        {
            result = (value < min) ? (min) : ((value > max) ? (max) : (value));
        }

        /// <summary>Clamps a value between 2 values, but wraps the value around.
        /// So that one plus <paramref name="max"/> would result in one plus <paramref name="min"/>.</summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>Clamped value.</returns>
        public static void ClampWrap(ref float value, ref float min, ref float max, out float result)
        {
            if (value >= min && value <= max)
            {
                result = value;
            }
            else
            {
                var rem = (value - min) % (max - min);
                result = rem + ((rem < 0) ? max : min);
            }
        }

        public static void Sort(ref float value1, ref float value2, out float min, out float max)
        {
            if (value1 > value2)
            {
                max = value1;
                min = value2;
            }
            else
            {
                max = value2;
                min = value1;
            }
        }

        /// <summary>Tries to solve for x in the equation: (a * (x * x) + b * x + c == 0).</summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="plus">The result of (b + Math.Sqrt((b * b) - (4 * a * c))) / (2 * a)</param>
        /// <param name="minus">The result of (b - Math.Sqrt((b * b) - (4 * a * c))) / (2 * a)</param>
        /// <returns><see langword="false" /> if an error would have been thrown; otherwise <see langword="true" />.</returns>
        public static bool TrySolveQuadratic(ref float a, ref float b, ref float c, out float plus, out float minus)
        {
            if (a.EqualsZero(0.000001f))
            {
                plus = -c / b;
                minus = plus;
                return true;
            }
            c = (b * b) - (4 * a * c);
            if (0 <= c)
            {
                c = MathF.Sqrt(c);
                a = .5f / a;
                plus = ((c - b) * a);
                minus = ((-c - b) * a);
                return true;
            }
            plus = 0;
            minus = 0;
            return false;
        }

        public static Match MatchNumbers(string target)
        {
            return _extractNumbers.Match(target);
        }

        public static bool ParseVectorAndNumber(string source, out Vector2 vector, out float number)
        {
            if (!String.IsNullOrWhiteSpace(source))
            {
                var match = MatchNumbers(source);
                if (match.Success && match.Groups.Count == 3)
                {
                    TakeVectorFromMatch(match, 1, out vector);
                    TakeNumberFromMatch(match, 3, out number);
                    return true;
                }
            }

            vector = Vector2.Zero;
            number = 0;
            return false;
        }

        public static bool ParseTwoVectors(string source, out Vector2 vector1, out Vector2 vector2)
        {
            if (!String.IsNullOrWhiteSpace(source))
            {
                var match = MatchNumbers(source);
                if (match.Success && match.Groups.Count == 4)
                {
                    TakeVectorFromMatch(match, 1, out vector1);
                    TakeVectorFromMatch(match, 3, out vector2);
                    return true;
                }
            }

            vector1 = Vector2.Zero;
            vector2 = Vector2.Zero;
            return false;
        }

        private static void TakeVectorFromMatch(Match match, int index, out Vector2 result)
        {
            if (index + 1 > match.Groups.Count)
            {
                result = Vector2.Zero;
            }
            else
            {
                result.X = float.Parse(match.Groups[index].Value, CultureInfo.InvariantCulture);
                result.Y = float.Parse(match.Groups[index + 1].Value, CultureInfo.InvariantCulture);
            }
        }

        private static void TakeNumberFromMatch(Match match, int index, out float result)
        {
            result = (index > match.Groups.Count)
                ? 0
                : float.Parse(match.Groups[index].Value, CultureInfo.InvariantCulture);
        }
    }
}