using ChristianGreiner.Duality.Plugins.GrabBag.Extensions;
using Duality;
using System;

namespace ChristianGreiner.Duality.Plugins.GrabBag.Helpers
{
    public static class Vector2Helper
    {
        /// <summary>Returns current <see cref="Vector2" /> with absolute values.</summary>
        /// <param name="current">Current  <see cref="Vector2" />.</param>
        /// <param name="absed">Current <see cref="Vector2" /> with absolute values.</param>
        public static void Abs(ref Vector2 current, out Vector2 absed)
        {
            absed.X = MathF.Abs(current.X);
            absed.Y = MathF.Abs(current.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed normal.</returns>
        public static Vector2 TransformNormal(Vector2 normal, Matrix4 matrix)
        {
            return new Vector2((normal.X * matrix.M11) + (normal.Y * matrix.M21), (normal.X * matrix.M12) + (normal.Y * matrix.M22));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(ref Vector2 normal, ref Matrix4 matrix, out Vector2 result)
        {
            var x = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            var y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
        {
            return new Vector2(
                MathF.Clamp(value1.X, min.X, max.X),
                MathF.Clamp(value1.Y, min.Y, max.Y));
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <param name="result">The clamped value as an output parameter.</param>
        public static void Clamp(ref Vector2 value1, ref Vector2 min, ref Vector2 max, out Vector2 result)
        {
            result.X = MathF.Clamp(value1.X, min.X, max.X);
            result.Y = MathF.Clamp(value1.Y, min.Y, max.Y);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static float Distance(Vector2 value1, Vector2 value2)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return MathF.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = MathF.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>Calculates squared euclidean distances between two specified vectors.</summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <returns>Squared euclidean distance between two specified vectors.</returns>
        public static float DistanceSq(Vector2 left, Vector2 right)
        {
            float result;
            DistanceSq(ref left, ref right, out result);
            return result;
        }

        /// <summary>Calculates squared euclidean distance between two specified vectors.</summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <param name="result">Squared euclidean distance between two specified vectors.</param>
        public static void DistanceSq(ref Vector2 left, ref Vector2 right, out float result)
        {
            float x, y;
            x = left.X - right.X;
            y = left.Y - right.Y;
            result = x * x + y * y;
        }

        /// <summary>
        ///     This returns the Normalized Vector2 that is passed. This is also known as a Unit Vector.
        /// </summary>
        /// <param name="source">The Vector2 to be Normalized.</param>
        /// <returns>The Normalized Vector2. (Unit Vector)</returns>
        /// <remarks>
        ///     <seealso href="http://en.wikipedia.org/wiki/Vector_%28spatial%29#Unit_vector" />
        /// </remarks>
        public static Vector2 Normalize(Vector2 source)
        {
            Vector2 result;
            Normalize(ref source, out result);
            return result;
        }

        public static void Normalize(ref Vector2 source, out Vector2 result)
        {
            var magnitude = source.Length;
            if (magnitude > 0)
            {
                magnitude = (1 / magnitude);
                result.X = source.X * magnitude;
                result.Y = source.Y * magnitude;
            }
            else
            {
                result = Vector2.Zero;
            }
        }

        [CLSCompliant(false)]
        public static void Normalize(ref Vector2 source)
        {
            Normalize(ref source, out source);
        }

        /// <summary>
        ///     This returns the Normalized Vector2 that is passed. This is also known as a Unit Vector.
        /// </summary>
        /// <param name="source">The Vector2 to be Normalized.</param>
        /// <param name="magnitude">the magitude of the Vector2 passed</param>
        /// <returns>The Normalized Vector2. (Unit Vector)</returns>
        /// <remarks>
        ///     <seealso href="http://en.wikipedia.org/wiki/Vector_%28spatial%29#Unit_vector" />
        /// </remarks>
        public static Vector2 Normalize(Vector2 source, out float magnitude)
        {
            Vector2 result;
            Normalize(ref source, out magnitude, out result);
            return result;
        }

        public static void Normalize(ref Vector2 source, out float magnitude, out Vector2 result)
        {
            magnitude = source.Length;
            if (magnitude > 0)
            {
                var magnitudeInv = (1 / magnitude);
                result.X = source.X * magnitudeInv;
                result.Y = source.Y * magnitudeInv;
            }
            else
            {
                result = Vector2.Zero;
            }
        }

        /// <summary>
        ///     Gets a Vector2 that is perpendicular(orthogonal) to the passed Vector2 while staying on the XY Plane.
        /// </summary>
        /// <param name="source">The Vector2 whose perpendicular(orthogonal) is to be determined.</param>
        /// <returns>An perpendicular(orthogonal) Vector2 using the Right Hand Rule</returns>
        /// <remarks>
        ///     <seealso href="http://en.wikipedia.org/wiki/Right-hand_rule" />
        /// </remarks>
        public static Vector2 RightPerp(Vector2 source)
        {
            Vector2 result;
            RightPerp(ref source, out result);
            return result;
        }

        public static void RightPerp(ref Vector2 source, out Vector2 result)
        {
            float sourceX = source.X;
            result.X = -source.Y;
            result.Y = sourceX;
        }

        /// <summary>Does a 2D Cross Product also know as an Outer Product.</summary>
        /// <param name="left">The left Vector2 operand.</param>
        /// <param name="right">The right Vector2 operand.</param>
        /// <returns>The Z value of the resulting vector.</returns>
        /// <remarks>
        ///     This 2D Cross Product is using a cheat. Since the Cross product (in 3D space)
        ///     always generates a vector perpendicular (orthogonal) to the 2 vectors used as
        ///     arguments. The cheat is that the only vector that can be perpendicular to two
        ///     vectors in the XY Plane will parallel to the Z Axis. Since any vector that is
        ///     parallel to the Z Axis will have zeros in both the X and Y Fields I can represent
        ///     the cross product of 2 vectors in the XY plane as single float: Z. Also the
        ///     Cross Product of and Vector on the XY plan and that of one ont on the Z Axis
        ///     will result in a vector on the XY Plane. So the ZCross Methods were well thought
        ///     out and can be trusted.
        ///     <seealso href="http://en.wikipedia.org/wiki/Cross_product" />
        /// </remarks>
        public static float ZCross(Vector2 left, Vector2 right)
        {
            float result;
            ZCross(ref left, ref right, out result);
            return result;
        }

        public static void ZCross(ref Vector2 left, ref Vector2 right, out float result)
        {
            result = left.X * right.Y - left.Y * right.X;
        }

        /// <summary>Checks the specified vectors for equality using epsilon.</summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <param name="epsilon">Epsilon used for absolute tolerance.</param>
        /// <returns>
        ///     true if vectors are equal; false otherwise.
        /// </returns>
        public static bool Equals(Vector2 left, Vector2 right, float epsilon = FloatExtensions.Tolerance)
        {
            return Equals(ref left, ref right, epsilon);
        }

        /// <summary> Checks the specified vectors for equality using epsilon. </summary>
        /// <param name="left">First vector.</param>
        /// <param name="right">Second vector.</param>
        /// <param name="epsilon">Epsilon used for absolute tolerance.</param>
        /// <returns>true if vectors are equal; false otherwise.</returns>
        [CLSCompliant(false)]
        public static bool Equals(ref Vector2 left, ref Vector2 right, float epsilon = FloatExtensions.Tolerance)
        {
            return left.X.NearlyEquals(right.X) && left.Y.NearlyEquals(right.Y);
        }
    }
}