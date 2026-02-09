namespace Tablazor.Attributes
{
    /// <summary>
    /// Sets the CSS Class name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class CssClassNameAttribute : Attribute
    {
        /// <summary>
        /// The CSS class name
        /// </summary>
        public string ClassName { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CssClassNameAttribute"/>
        /// class
        /// </summary>
        /// <param name="className">The CSS class name</param>
        public CssClassNameAttribute(string className)
        {
            ClassName = className;
        }
    }
}
