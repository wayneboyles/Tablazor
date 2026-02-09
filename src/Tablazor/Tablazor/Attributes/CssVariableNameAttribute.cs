namespace Tablazor.Attributes
{
    /// <summary>
    /// Sets the CSS variable
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class CssVariableNameAttribute : Attribute
    {
        /// <summary>
        /// The CSS variable name
        /// </summary>
        public string Variable { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CssVariableNameAttribute"/>
        /// class
        /// </summary>
        /// <param name="variableName">The CSS variable name</param>
        public CssVariableNameAttribute(string variableName)
        {
            Variable = variableName;
        }
    }
}
