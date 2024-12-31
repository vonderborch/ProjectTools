using System;
using Avalonia.Controls;
using ProjectTools.App.Views.Pages;

namespace ProjectTools.App.Views;

public partial class ViewControl : UserControl
{
    public ViewControl()
    {
        InitializeComponent();
    }

    public void ChangeView(View newView)
    {
        UserControl newControl;

        switch (newView)
        {
            case View.Home:
                newControl = new HomeView();
                break;
            case View.Help:
                newControl = new HelpView();
                break;
            case View.Settings:
                newControl = new ConfigurationView();
                break;
            case View.GenerateProject:
                newControl = new GenerateProject();
                break;
            case View.PrepareTemplate:
                newControl = new PrepareTemplate();
                break;
            default:
                throw new Exception("Unknown view!");
        }

        newControl.DataContext = this.DataContext;
        this.Content = newControl;
    }
}
