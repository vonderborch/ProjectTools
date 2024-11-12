using ProjectTools.Core.Settings;

namespace ProjectTools.CL.Options;

/// <summary>
///     An abstract command line option
/// </summary>
public abstract class AbstractOption
{
    /// <summary>
    ///     Gets or sets a value indicating whether [allow automatic configuration].
    /// </summary>
    /// <value><c>true</c> if [allow automatic configuration]; otherwise, <c>false</c>.</value>
    protected bool AllowAutoConfiguration { get; set; } = true;

    /// <summary>
    ///     Gets or sets a value indicating whether [allow template updates].
    /// </summary>
    /// <value><c>true</c> if [allow template updates]; otherwise, <c>false</c>.</value>
    protected bool AllowTemplateUpdates { get; set; } = true;

    /// <summary>
    ///     Executes what this option represents.
    /// </summary>
    /// <returns>The result of the execution.</returns>
    public abstract string Execute();

    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <returns>The result.</returns>
    public string ExecuteOption(AbstractOption option)
    {
        SetOptions(option);

        // run configuration option if we are allowed to and need to...
        if (this.AllowAutoConfiguration && AbstractSettings.Load() == null)
        {
            LogMessage("Creating settings file...");
            var configure = new Configure();
            configure.Execute();
        }

        // run updating logic if we are allowed to and need to...
        var appSettings = AbstractSettings.Load();
        if (this.AllowTemplateUpdates && appSettings is { ShouldUpdateTemplates: true })
        {
            var updateTemplates = GetTemplateUpdaterCommandObject();
            updateTemplates.Execute();
        }

        return Execute();
    }

    /// <summary>
    ///     Gets the template updater command object.
    /// </summary>
    /// <returns>The template updater command object.</returns>
    protected virtual UpdateTemplates GetTemplateUpdaterCommandObject()
    {
        UpdateTemplates obj = new() { Silent = false, ForceCheck = false, ForceRedownload = false };
        return obj;
    }

    /// <summary>
    ///     Logs the message.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>True to indicate a message was wrwitten.</returns>
    protected bool LogMessage(string value)
    {
        Console.WriteLine(value);
        return true;
    }

    /// <summary>
    ///     Returns the total seconds since a given time in string form.
    /// </summary>
    /// <param name="time">The time.</param>
    /// <returns>The total time in seconds.</returns>
    protected string TotalSecondsSinceTime(DateTime time)
    {
        var totalTime = DateTime.Now - time;
        return totalTime.TotalSeconds.ToString("0.00");
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected abstract void SetOptions(AbstractOption option);
}
