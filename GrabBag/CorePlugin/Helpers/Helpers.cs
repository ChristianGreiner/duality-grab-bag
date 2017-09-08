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
    }
}