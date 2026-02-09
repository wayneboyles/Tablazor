namespace Tablazor.Exceptions;

/// <summary>
/// Custom class for exceptions thrown by the framework
/// </summary>
/// <param name="message">The error message</param>
/// <param name="exception">The actual <see cref="Exception"/></param>
public sealed class TabException(string message, Exception? exception = null) : Exception(message, exception);