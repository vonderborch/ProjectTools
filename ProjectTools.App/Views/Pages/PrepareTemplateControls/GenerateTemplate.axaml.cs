using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.ViewModels;

namespace ProjectTools.App.Views.Pages.PrepareTemplateControls;

public partial class GenerateTemplate : UserControl
{
    public PrepareTemplateViewModel PrepareViewModel;

    public GenerateTemplate()
    {
        InitializeComponent();
    }

    public void Reset()
    {
        this.TextTemplateGenerationLog.Text = "Template Generation Log";
    }

    public bool LogText(string message)
    {
        this.TextTemplateGenerationLog.Text = $"{this.TextTemplateGenerationLog.Text}{Environment.NewLine}{message}";
        return true;
    }
}
