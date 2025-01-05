#region

using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.DataContexts;
using ProjectTools.App.Dialogs.YesNoDialogBox;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

#endregion

namespace ProjectTools.App.Views.Pages.GenerateProjectControls;

public partial class SlugConfigurationControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly GenerateProjectPage _parentPage;

    public SlugConfigurationControl(GenerateProjectPage parent)
    {
        this._parentPage = parent;

        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    private GenerateProjectDataContext Context => this._parentPage.Context;

    /// <summary>
    ///     Event handler for when the button to generate the project is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private async void ButtonGenerateProject_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            this.Context.ClearCoreLog();
            this.Context.ClearScriptLog();
            this.Context.ClearInstructionLog();

            if (this.Context.TemplateInfo == null)
            {
                throw new Exception("No template selected.");
            }

            if (this.Context.Template == null)
            {
                throw new Exception("No template selected.");
            }

            this.Context.SlugConfigurationContext.ValidateSlugs();

            this.Context.Template.Slugs = this.Context.SlugConfigurationContext.GetAllSlugs();

            var backupConfigFile = Path.Combine(PathConstants.ProjectGenerationConfigDirectory,
                $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}_{this.Context.Template.SafeName}.json");
            var interactiveSlugs = this.Context.SlugConfigurationContext.GetSlugs(true);
            var mainFile = Path.Combine(PathConstants.ProjectGenerationConfigDirectory,
                this.Context.SlugConfigurationContext.ProjectConfigFile);
            SlugHelpers.SaveSlugsToProjectConfigFile(mainFile, interactiveSlugs);
            SlugHelpers.SaveSlugsToProjectConfigFile(backupConfigFile, interactiveSlugs);

            if (this.Context.CoreConfigurationContext.WhatIf)
            {
                this.Context.LogToCore("What-if Mode Enabled, no project generated!");
            }
            else
            {
                Logger corelogger = new(this.Context.LogToCore);
                Logger scriptLogger = new(this.Context.LogToScript);
                Logger instructionsLogger = new(this.Context.LogToInstruction);
                var result = await this.Context.Template.AsyncGenerateProject(
                    this.Context.CoreConfigurationContext.ParentOutputDirectory,
                    this.Context.CoreConfigurationContext.ProjectName, this.Context.TemplateInfo.Value.LocalPath,
                    corelogger,
                    instructionsLogger, scriptLogger, this.Context.CoreConfigurationContext.ForceOverwrite);
                if (!string.IsNullOrEmpty(result))
                {
                    corelogger.Log(result);
                }
            }
        }
        catch (Exception ex)
        {
            this.Context.LogToCore($"Error While Generating Project: {ex.Message}");
            await YesNoDialogBox.Open(
                this,
                "Error",
                $"Error While Generating Project: {ex.Message}",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
        }
    }
}
