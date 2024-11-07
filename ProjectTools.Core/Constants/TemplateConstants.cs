namespace ProjectTools.Core.Constants;

public static class TemplateConstants
{
    /// <summary>
    ///     The name of the template settings file.
    /// </summary>
    public static string TemplateSettingsFileName = "template.json";

    /// <summary>
    ///     The default template version.
    /// </summary>
    public static Version DefaultTemplateVersion = new(0, 0, 0);

    /// <summary>
    ///     The current template version.
    /// </summary>
    public static Version CurrentTemplateVersion = new(1, 0, 0);

    /// <summary>
    ///     The minimum supported template version.
    /// </summary>
    public static Version MinSupportedTemplateVersion = new(1, 0, 0);

    /// <summary>
    ///     The maximum supported template version.
    /// </summary>
    public static Version MaxSupportedTemplateVersion = new(1, 1, 0);
}
