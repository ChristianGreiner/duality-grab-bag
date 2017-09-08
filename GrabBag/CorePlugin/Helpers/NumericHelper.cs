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

using Duality;

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
    }
}