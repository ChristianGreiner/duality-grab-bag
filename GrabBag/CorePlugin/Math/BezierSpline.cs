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

using ChristianGreiner.Duality.Plugins.GrabBag.Collections;
using ChristianGreiner.Duality.Plugins.GrabBag.Helpers;
using Duality;

namespace ChristianGreiner.Duality.Plugins.GrabBag.Math
{
    /// <summary>
    /// houses a series of cubic bezier points and provides helper methods to access the bezier
    /// </summary>
    public class BezierSpline
    {
        private FastList<Vector2> points = new FastList<Vector2>();
        private int curveCount;

        /// <summary>
        /// helper that gets the bezier point index at time t. t is modified in the process to be in the range of the curve segment.
        /// </summary>
        /// <returns>The index at time.</returns>
        /// <param name="t">T.</param>
        private int PointIndexAtTime(ref float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Length - 4;
            }
            else
            {
                t = NumericHelper.Clamp01(t) * curveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }

            return i;
        }

        /// <summary>
        /// sets a control point taking into account if this is a shared point and adjusting appropriately if it is
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="point">Point.</param>
        public void SetControlPoint(int index, Vector2 point)
        {
            if (index % 3 == 0)
            {
                var delta = point - points[index];
                if (index > 0)
                    points.Buffer[index - 1] += delta;

                if (index + 1 < points.Length)
                    points.Buffer[index + 1] += delta;
            }
            points.Buffer[index] = point;
        }

        /// <summary>
        /// gets the point on the bezier at time t
        /// </summary>
        /// <returns>The point at time.</returns>
        /// <param name="t">T.</param>
        public Vector2 getPointAtTime(float t)
        {
            var i = PointIndexAtTime(ref t);
            return Bezier.GetPoint(points.Buffer[i], points.Buffer[i + 1], points.Buffer[i + 2], points.Buffer[i + 3], t);
        }

        /// <summary>
        /// gets the velocity (first derivative) of the bezier at time t
        /// </summary>
        /// <returns>The velocity at time.</returns>
        /// <param name="t">T.</param>
        public Vector2 GetVelocityAtTime(float t)
        {
            var i = PointIndexAtTime(ref t);
            return Bezier.GetFirstDerivative(points.Buffer[i], points.Buffer[i + 1], points.Buffer[i + 2], points.Buffer[i + 3], t);
        }

        /// <summary>
        /// gets the direction (normalized first derivative) of the bezier at time t
        /// </summary>
        /// <returns>The direction at time.</returns>
        /// <param name="t">T.</param>
        public Vector2 GetDirectionAtTime(float t)
        {
            return GetVelocityAtTime(t).Normalized;
        }

        /// <summary>
        /// Adds a curve to the bezier
        /// </summary>
        /// <param name="start">Start.</param>
        /// <param name="firstControlPoint">First control point.</param>
        /// <param name="secondControlPoint">Second control point.</param>
        public void AddCurve(Vector2 start, Vector2 firstControlPoint, Vector2 secondControlPoint, Vector2 end)
        {
            // we only Add the start point if this is the first curve. For all other curves the previous end should equal the start of the new curve.
            if (points.Length == 0)
                points.Add(start);

            points.Add(firstControlPoint);
            points.Add(secondControlPoint);
            points.Add(end);

            curveCount = (points.Length - 1) / 3;
        }

        /// <summary>
        /// resets the bezier removing all points
        /// </summary>
        public void Reset()
        {
            points.Clear();
        }

        /// <summary>
        /// breaks up the spline into totalSegments parts and returns all the points required to draw using lines
        /// </summary>
        /// <returns>The drawing points.</returns>
        /// <param name="totalSegments">Total segments.</param>
        public Vector2[] GetDrawingPoints(int totalSegments)
        {
            var points = new Vector2[totalSegments];
            for (var i = 0; i < totalSegments; i++)
            {
                var t = i / (float)totalSegments;
                points[i] = getPointAtTime(t);
            }

            return points;
        }
    }
}