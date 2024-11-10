using System.Reflection;

namespace ProjectTools.Core.Settings;

/// <summary>
///     An attribute to register a settings class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SettingRegistration : Attribute
{
    /// <summary>
    ///     The next compatible settings version.
    /// </summary>
    public readonly Version? NextSettingsVersion;

    /// <summary>
    ///     The settings version.
    /// </summary>
    public readonly Version SettingsVersion;

    /// <summary>
    ///     Constructs a new instance of the <see cref="SettingRegistration" /> class.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <param name="nextMajor">The next compatible major version.</param>
    /// <param name="nextMinor">The next compatible minor version.</param>
    public SettingRegistration(int major, int minor, int nextMajor = -1, int nextMinor = -1)
    {
        this.SettingsVersion = new Version(major, minor);
        if (nextMajor != -1 && nextMinor != -1)
        {
            this.NextSettingsVersion = new Version(nextMajor, nextMinor);
        }
    }

    /// <summary>
    ///     The current settings version class.
    /// </summary>
    public Type CurrentSettingsVersionType => GetSettingsVersionTypeForVersion(this.SettingsVersion);

    /// <summary>
    ///     The next settings version class, if it exists.
    /// </summary>
    public Type? NextSettingsVersionType => GetSettingsVersionTypeForVersion(this.NextSettingsVersion);

    /// <summary>
    ///     Returns the settings class for the specified version.
    /// </summary>
    /// <param name="version">The version to search for.</param>
    /// <returns>The settings class for the version, or null.</returns>
    public static Type? GetSettingsVersionTypeForVersion(Version? version)
    {
        if (version == null)
        {
            return null;
        }

        var settingsClasses = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AbstractSettings)) && !t.IsAbstract &&
                        t.IsDefined(typeof(SettingRegistration), false));

        foreach (var settingsClass in settingsClasses)
        {
            var registration =
                (SettingRegistration?)GetCustomAttribute(settingsClass, typeof(SettingRegistration));

            if (registration?.SettingsVersion == version)
            {
                return settingsClass;
            }
        }

        return null;
    }

    /// <summary>
    ///     Returns the settings class for the specified version.
    /// </summary>
    /// <param name="version">The version to search for.</param>
    /// <returns>The settings class for the version, or null.</returns>
    public static SettingRegistration? GetRegistrationForVersion(Version? version)
    {
        if (version == null)
        {
            return null;
        }

        var settingsClasses = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AbstractSettings)) && !t.IsAbstract &&
                        t.IsDefined(typeof(SettingRegistration), false));

        foreach (var settingsClass in settingsClasses)
        {
            var registration =
                (SettingRegistration?)GetCustomAttribute(settingsClass, typeof(SettingRegistration));

            if (registration?.SettingsVersion == version)
            {
                return registration;
            }
        }

        return null;
    }
}
