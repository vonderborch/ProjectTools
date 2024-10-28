namespace ProjectTools.Core.Templaters;

/// <summary>
///     An attribute handling registration of a templater implementation.
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Class)]
public class TemplaterRegistration : Attribute;
