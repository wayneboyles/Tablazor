using System.Reflection;
using Tablazor.Attributes;

namespace Tablazor.Validation;

/// <summary>
/// Provides validation services for component parameters using custom validation attributes.
/// </summary>
public sealed class ComponentValidator
{
    /// <summary>
        /// Validates all parameters on the given component instance.
        /// </summary>
        /// <param name="component">The component instance to validate.</param>
        /// <returns>A collection of validation error messages. Empty if validation passes.</returns>
        /// <exception cref="ArgumentNullException">Thrown when component is null.</exception>
        public static IEnumerable<string> Validate(object component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            var errors = new List<string>();
            var componentType = component.GetType();
            var properties = componentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                errors.AddRange(ValidateRequiredIf(component, property));
                errors.AddRange(ValidateNotRequiredIf(component, property));
            }

            return errors;
        }

        /// <summary>
        /// Validates RequiredIf attributes on a property.
        /// </summary>
        private static IEnumerable<string> ValidateRequiredIf(object component, PropertyInfo property)
        {
            var errors = new List<string>();
            var requiredIfAttributes = property.GetCustomAttributes<RequiredIfAttribute>();

            foreach (var attribute in requiredIfAttributes)
            {
                var dependentProperty = component.GetType().GetProperty(attribute.DependentProperty);

                if (dependentProperty == null)
                {
                    errors.Add($"Dependent property '{attribute.DependentProperty}' not found on component type '{component.GetType().Name}'.");
                    continue;
                }

                var dependentValue = dependentProperty.GetValue(component);

                // Check if the dependent condition is met
                if (ValuesAreEqual(dependentValue, attribute.DependentValue))
                {
                    var propertyValue = property.GetValue(component);

                    // If the property is required but not set, add an error
                    if (IsNullOrDefault(propertyValue, property.PropertyType))
                    {
                        errors.Add(attribute.GetErrorMessage(property.Name));
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// Validates NotRequiredIf attributes on a property.
        /// </summary>
        private static IEnumerable<string> ValidateNotRequiredIf(object component, PropertyInfo property)
        {
            var errors = new List<string>();
            var notRequiredIfAttributes = property.GetCustomAttributes<NotRequiredIfAttribute>();

            foreach (var attribute in notRequiredIfAttributes)
            {
                var conflictingProperty = component.GetType().GetProperty(attribute.ConflictingProperty);

                if (conflictingProperty == null)
                {
                    errors.Add($"Conflicting property '{attribute.ConflictingProperty}' not found on component type '{component.GetType().Name}'.");
                    continue;
                }

                var conflictingValue = conflictingProperty.GetValue(component);
                var propertyValue = property.GetValue(component);

                // Only validate if this property is actually set
                if (IsNullOrDefault(propertyValue, property.PropertyType))
                {
                    continue;
                }

                // Check if the conflicting condition is met
                bool conflictExists = attribute.ConflictingValue == null
                    ? !IsNullOrDefault(conflictingValue, conflictingProperty.PropertyType)
                    : ValuesAreEqual(conflictingValue, attribute.ConflictingValue);

                if (conflictExists && attribute.ErrorIfSet)
                {
                    errors.Add(attribute.GetErrorMessage(property.Name));
                }
            }

            return errors;
        }

        /// <summary>
        /// Determines if two values are equal, handling null and type conversions.
        /// </summary>
        private static bool ValuesAreEqual(object? value1, object? value2)
        {
            if (value1 == null && value2 == null)
            {
                return true;
            }

            if (value1 == null || value2 == null)
            {
                return false;
            }

            // Handle string comparisons
            if (value1 is string str1 && value2 is string str2)
            {
                return string.Equals(str1, str2, StringComparison.Ordinal);
            }

            // Handle boolean comparisons
            if (value1 is bool bool1 && value2 is bool bool2)
            {
                return bool1 == bool2;
            }

            // Handle numeric comparisons
            if (IsNumericType(value1.GetType()) && IsNumericType(value2.GetType()))
            {
                var decimal1 = Convert.ToDecimal(value1);
                var decimal2 = Convert.ToDecimal(value2);
                return decimal1 == decimal2;
            }

            // Default to Equals
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines if a value is null or the default value for its type.
        /// </summary>
        private static bool IsNullOrDefault(object? value, Type type)
        {
            if (value == null)
            {
                return true;
            }

            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return value.Equals(defaultValue);
            }

            // For strings, check for empty or whitespace
            if (type == typeof(string))
            {
                return string.IsNullOrWhiteSpace(value as string);
            }

            return false;
        }

        /// <summary>
        /// Determines if a type is a numeric type.
        /// </summary>
        private static bool IsNumericType(Type type)
        {
            return 
                type == typeof(byte) || 
                type == typeof(sbyte) ||
                type == typeof(short) || 
                type == typeof(ushort) ||
                type == typeof(int) || 
                type == typeof(uint) ||
                type == typeof(long) || 
                type == typeof(ulong) ||
                type == typeof(float) || 
                type == typeof(double) ||
                type == typeof(decimal);
        }
}