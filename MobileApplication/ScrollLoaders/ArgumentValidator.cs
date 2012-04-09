using System;

namespace MetrocamPan.ScrollLoaders
{
    /// <summary>
    /// Utility class for validating method parameters.
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// Ensures the specified value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to test.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>The specified value.</returns>
        /// <exception cref="ArgumentNullException">Occurs if the specified value 
        /// is <code>null</code>.</exception>
        public static T AssertNotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Ensures the specified value is not <code>null</code> 
        /// or empty (a zero length string).
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>The specified value.</returns>
        /// <exception cref="ArgumentNullException">Occurs if the specified value 
        /// is <code>null</code> or empty (a zero length string).</exception>
        public static string AssertNotNullOrEmpty(string value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.Length < 1)
            {
                /* TODO: Make localizable resource. */
                throw new ArgumentException(
                    "Parameter should not be an empty string.", parameterName);
            }

            return value;
        }

        /// <summary>
        /// Ensures the specified value is not <code>null</code> 
        /// and that it is of the specified type.
        /// </summary>
        /// <param name="value">The value to test.</param> 
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The value to test.</returns>
        /// <exception cref="ArgumentNullException">Occurs if the specified value 
        /// is <code>null</code> or of type not assignable 
        /// from the specified type.</exception>
        /// <example>
        /// public DoSomething(object message)
        /// {
        /// 	this.message = ArgumentValidator.AssertNotNullAndOfType&lt;string&gt;(
        ///							message, "message");	
        /// }
        /// </example>
        public static T AssertNotNullAndOfType<T>(
            object value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            var result = value as T;
            if (result == null)
            {
                throw new ArgumentException(string.Format(
                    "Expected argument of type {0}, but was {1}",
                    typeof(T), value.GetType()),
                    parameterName);
            }
            return result;
        }

        /* TODO: [DV] Comment. */
        public static int AssertGreaterThan(
            int comparisonValue, int value, string parameterName)
        {
            if (value <= comparisonValue)
            {
                /* TODO: Make localizable resource. */
                throw new ArgumentOutOfRangeException(
                    "Parameter should be greater than "
                    + comparisonValue, parameterName);
            }
            return value;
        }

        /* TODO: [DV] Comment. */
        public static double AssertGreaterThan(
            double comparisonValue, double value, string parameterName)
        {
            if (value <= comparisonValue)
            {
                /* TODO: Make localizable resource. */
                throw new ArgumentOutOfRangeException(
                    "Parameter should be greater than "
                    + comparisonValue, parameterName);
            }
            return value;
        }

        /* TODO: [DV] Comment. */
        public static long AssertGreaterThan(
            long comparisonValue, long value, string parameterName)
        {
            if (value <= comparisonValue)
            {
                /* TODO: Make localizable resource. */
                throw new ArgumentOutOfRangeException(
                    "Parameter should be greater than "
                    + comparisonValue, parameterName);
            }
            return value;
        }

        /* TODO: [DV] Comment. */
        public static int AssertGreaterThanOrEqualTo(
            int comparisonValue, int value, string parameterName)
        {
            if (value < comparisonValue)
            {
                /* TODO: Make localizable resource. */
                throw new ArgumentOutOfRangeException(
                    "Parameter should be greater than or equal to "
                    + comparisonValue, parameterName);
            }
            return value;
        }

        /* TODO: [DV] Comment. */
        public static double AssertGreaterThanOrEqualTo(
            double comparisonValue, double value, string parameterName)
        {
            if (value < comparisonValue)
            {
                /* TODO: Make localizable resource. */
                throw new ArgumentOutOfRangeException(
                    "Parameter should be greater than or equal to "
                    + comparisonValue, parameterName);
            }
            return value;
        }

        /* TODO: [DV] Comment. */
        public static long AssertGreaterThanOrEqualTo(
            long comparisonValue, long value, string parameterName)
        {
            if (value < comparisonValue)
            {
                /* TODO: Make localizable resource. */
                throw new ArgumentOutOfRangeException(
                    "Parameter should be greater than "
                    + comparisonValue, parameterName);
            }
            return value;
        }

        /* TODO: [DV] Comment. */
        public static double AssertLessThan(
            double comparisonValue, double value, string parameterName)
        {
            if (value >= comparisonValue)
            {
                /* TODO: Make localizable resource. */
                throw new ArgumentOutOfRangeException(
                    "Parameter should be less than "
                    + comparisonValue, parameterName);
            }
            return value;
        }
    }
}
