using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Scripting;

namespace ProjectTools.Core.Settings;

/// <summary>
///     An abstract class for settings.
/// </summary>
public abstract class AbstractSettings
{
    /// <summary>
    ///     The settings version.
    /// </summary>
    public Version SettingsVersion;

    /// <summary>
    ///     Constructs a new instance of the <see cref="AbstractSettings" /> class.
    /// </summary>
    public AbstractSettings()
    {
        var version = GetSettingsVersionForClass(GetType());
        this.SettingsVersion = version;
    }

    /// <summary>
    ///     Loads the settings or raises an exception if we can't load them.
    /// </summary>
    /// <returns>The app settings.</returns>
    /// <exception cref="Exception">Raises if we couldn't load the app settings.</exception>
    public static AppSettings LoadOrThrow(bool loadPython = true)
    {
        var appSettings = Load();
        if (appSettings == null)
        {
            throw new Exception("Failed to load app settings.");
        }

        if (loadPython)
        {
            _ = PythonManager.Manager.PythonDirectory;
        }

        return appSettings;
    }

    /// <summary>
    ///     Loads the settings from a file.
    /// </summary>
    /// <returns>The app settings, or null if we couldn't load them.</returns>
    public static AppSettings? Load()
    {
        // if the settings file does not exist, return null
        if (!File.Exists(AppSettingsConstants.SettingsFilePath))
        {
            return null;
        }

        // otherwise, try to load it...
        Version? upgradeAppSettingsFileFromVersion = null;
        AppSettings? settings = null;
        try
        {
            settings = JsonHelpers.DeserializeFromFile<AppSettings>(AppSettingsConstants.SettingsFilePath);
            if (settings == null)
            {
                upgradeAppSettingsFileFromVersion = AppSettingsConstants.DefaultNotFoundSettingsVersion;
            }
            else if (settings.SettingsVersion != AppSettingsConstants.SettingsVersion)
            {
                upgradeAppSettingsFileFromVersion = settings.SettingsVersion;
            }
        }
        catch (Exception)
        {
            upgradeAppSettingsFileFromVersion = AppSettingsConstants.DefaultNotFoundSettingsVersion;
        }

        // if we failed to load it, we need to _try_ to upgrade it...
        if (upgradeAppSettingsFileFromVersion != null)
        {
            settings = UpgradeAppSettingsFile(upgradeAppSettingsFileFromVersion);
        }

        return settings;
    }

    /// <summary>
    ///     Upgrades the app settings file version.
    /// </summary>
    /// <param name="fromVersion">The version to upgrade from.</param>
    /// <returns>The upgraded app settings, or null if we could not update.</returns>
    private static AppSettings? UpgradeAppSettingsFile(Version fromVersion)
    {
        // Step 1 - Handle when the current version matches the app version
        if (fromVersion == AppSettingsConstants.SettingsVersion)
        {
            return JsonHelpers.DeserializeFromFile<AppSettings>(AppSettingsConstants.SettingsFilePath);
        }

        // Step 2 - Handle the default settings version...
        if (fromVersion == AppSettingsConstants.DefaultNotFoundSettingsVersion)
        {
            // TODO: do something in this scenario beyond resetting the settings file...
            return null;
        }

        // Step 3 - Get the registration for the version...
        var settingsRegistration = SettingRegistration.GetRegistrationForVersion(fromVersion);
        if (settingsRegistration == null)
        {
            return null;
        }

        if (settingsRegistration.NextSettingsVersion == null)
        {
            return null;
        }

        // Step 4 - Upgrade to the next version...
        var currentVersionType = settingsRegistration.CurrentSettingsVersionType;
        var currentLoadedVersion = currentVersionType.GetMethod("LoadVersion");
        var currentVersion = (AbstractSettings?)currentLoadedVersion?.Invoke(null, null);

        var nextVersionUpgradeMethod = currentVersionType.GetMethod("ToNextSettingsVersion");
        var nextVersion = (AbstractSettings?)nextVersionUpgradeMethod?.Invoke(null, [currentVersion]);
        if (nextVersion == null)
        {
            return null;
        }

        // Step 5 - Save the new settings...
        nextVersion.Save();

        // Step 6 - Determine what to do next...
        if (nextVersion.SettingsVersion == AppSettingsConstants.SettingsVersion)
        {
            return (AppSettings)nextVersion;
        }

        return UpgradeAppSettingsFile(nextVersion.SettingsVersion);
    }

    /// <summary>
    ///     A method to convert the settings to the next version.
    /// </summary>
    /// <returns>The updated settings.</returns>
    public static AbstractSettings? ToNextSettingsVersion(AbstractSettings? currentSettings)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     A method to load the specific version of the settings. Needs to be defined in the child classes.
    /// </summary>
    /// <returns>Loads the settings for the specified version.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static AbstractSettings? LoadVersion()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Saves the settings to a file.
    /// </summary>
    public void Save()
    {
        JsonHelpers.SerializeToFile(AppSettingsConstants.SettingsFilePath, this);
    }

    /// <summary>
    ///     Gets the registered settings version for the provided type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The version registered for the type.</returns>
    /// <exception cref="Exception">Raises if no SettingsRegistration is registered for the class provided.</exception>
    private static Version GetSettingsVersionForClass(Type type)
    {
        var registration = (SettingRegistration?)Attribute.GetCustomAttribute(type, typeof(SettingRegistration));
        if (registration == null)
        {
            throw new Exception("Settings class must have a SettingRegistration attribute.");
        }

        return registration.SettingsVersion;
    }
}
