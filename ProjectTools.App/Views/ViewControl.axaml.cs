using System;
using Avalonia.Controls;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.App.Views.Pages;

namespace ProjectTools.App.Views;

public partial class ViewControl : UserControl
{
    public ViewControl()
    {
        InitializeComponent();
    }

    public void ChangeView(Page newPage)
    {
        UserControl newControl;

        switch (newPage)
        {
            case Page.Home:
                newControl = new HomePage();
                break;
            case Page.Help:
                newControl = new HelpView();
                break;
            case Page.Settings:
                newControl = new ConfigurationPage();
                break;
            case Page.PrepareTemplate:
                newControl = new PrepareTemplate();
                break;
            default:
                throw new Exception("Unknown view!");
        }

        newControl.DataContext = this.DataContext;
        this.Content = newControl;
    }
}
