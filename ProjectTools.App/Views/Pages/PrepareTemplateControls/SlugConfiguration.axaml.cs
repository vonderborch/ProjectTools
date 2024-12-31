using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.ViewModels;
using ProjectTools.Core.Templates;

namespace ProjectTools.App.Views.Pages.PrepareTemplateControls;

public partial class SlugConfiguration : UserControl
{
    private readonly Dictionary<SlugType, string> _messageExtras = new();
    private readonly Dictionary<string, SlugType> _slugTypesByName;
    private readonly Dictionary<SlugType, string> _slugTypesByType;
    private string _currentEditingSlug = string.Empty;
    private ulong _currentSlugId;
    private Dictionary<string, PreparationSlug> _slugCache;
    public PrepareTemplateViewModel PrepareViewModel;

    public SlugConfiguration()
    {
        InitializeComponent();

        this._slugTypesByName =
            Enum.GetValues(typeof(SlugType)).Cast<SlugType>().ToDictionary(x => x.ToString(), x => x);
        this._slugTypesByType =
            Enum.GetValues(typeof(SlugType)).Cast<SlugType>().ToDictionary(x => x, x => x.ToString());
        this.ComboBoxBoxSlugType.ItemsSource = this._slugTypesByName.Keys;

        var specialValueHandler = new SpecialValueHandler("C://my_fake_directory//sub_directory", "fake_proj", null);

        foreach (var slugType in Enum.GetValues<SlugType>())
        {
            var messageExtra = "";
            var specialKeywords = specialValueHandler.GetSpecialKeywords(slugType);
            if (specialKeywords.Count > 0)
            {
                messageExtra = string.Join(Environment.NewLine, specialKeywords);
            }

            this._messageExtras.Add(slugType, messageExtra);
        }
    }

    public void ResetSlugConfigurationPanel()
    {
        this.PrepareViewModel.PrepareTemplate.ViewerSlugConfig.IsEnabled = false;
        this.PrepareViewModel.PrepareTemplate.ViewerTemplateGeneration.IsEnabled = false;
        this._currentEditingSlug = string.Empty;
        this._slugCache = new Dictionary<string, PreparationSlug>();
        UpdateSlugChoiceComboBox();
        ResetSlugOptions();
    }

    private void ResetSlugOptions()
    {
        this.TextBoxDisplayName.Text = string.Empty;
        this.TextBoxSlugKey.Text = string.Empty;
        this.ComboBoxBoxSlugType.SelectedItem = null;
        this.TextBoxSearchStrings.Text = string.Empty;
        this.PanelSlugType.IsEnabled = false;
        this.TextBoxSpecialValues.Text = string.Empty;
        this.TextBoxDefaultValue.Text = string.Empty;
        this.TextBoxAllowedValues.Text = string.Empty;
        this.TextBoxDisallowedValues.Text = string.Empty;
        this.CheckBoxRequiresUserInput.IsChecked = false;
        this.PanelNonGuidOptions.IsVisible = false;
    }

    public void UpdateSlugConfigurationPanel()
    {
        this._slugCache = this.PrepareViewModel.PreparationTemplate.Slugs.ToDictionary(x => x.DisplayName, x => x);
        UpdateSlugChoiceComboBox();

        this.PrepareViewModel.PrepareTemplate.ViewerSlugConfig.IsEnabled = true;
    }

    private void ComboBoxSlugs_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is not null)
        {
            var slugName = e.AddedItems[0].ToString();
            var slug = this._slugCache[slugName];
            this._currentEditingSlug = slugName;

            this.TextBoxDisplayName.Text = slug.DisplayName;
            this.TextBoxSlugKey.Text = slug.SlugKey;
            this.ComboBoxBoxSlugType.SelectedItem = this._slugTypesByType[slug.Type];
            if (slug.CustomSlug)
            {
                this.PanelSlugType.IsEnabled = true;
            }
            else
            {
                this.PanelSlugType.IsEnabled = false;
            }

            this.TextBoxSearchStrings.Text = string.Join(Environment.NewLine, slug.SearchStrings);
            this.PanelNonGuidOptions.IsVisible = slug.Type != SlugType.RandomGuid;
            this.TextBoxSpecialValues.Text = this._messageExtras[slug.Type];

            this.TextBoxDefaultValue.Text = slug.DefaultValue?.ToString() ?? string.Empty;
            this.TextBoxAllowedValues.Text = string.Join(Environment.NewLine, slug.AllowedValues);
            this.TextBoxDisallowedValues.Text = string.Join(Environment.NewLine, slug.DisallowedValues);
            this.CheckBoxRequiresUserInput.IsChecked = slug.RequiresUserInput;
        }
    }

    private void ButtonAddSlug_OnClick(object? sender, RoutedEventArgs e)
    {
        this._currentEditingSlug = $"NewSlug{this._currentSlugId++}";
        this._slugCache.Add(this._currentEditingSlug,
            new PreparationSlug { DisplayName = this._currentEditingSlug, CustomSlug = true });
        UpdateSlugChoiceComboBox();
        this.ComboBoxSlugs.SelectedItem = this._currentEditingSlug;
    }

    private void ButtonGenerateTemplate_OnClick(object? sender, RoutedEventArgs e)
    {
        this.PrepareViewModel.PreparationTemplate.Slugs = this._slugCache.Values.ToList();
        
    }

    private void ButtonSaveSlug_OnClick(object? sender, RoutedEventArgs e)
    {
        var slug = this._slugCache[this._currentEditingSlug];
        slug.DisplayName = this.TextBoxDisplayName.Text;
        slug.SlugKey = this.TextBoxSlugKey.Text;
        slug.Type = this._slugTypesByName[this.ComboBoxBoxSlugType.SelectedItem.ToString()];
        slug.SearchStrings = this.TextBoxSearchStrings.Text.Split(Environment.NewLine).ToList();
        slug.DefaultValue = this.TextBoxDefaultValue.Text;
        slug.AllowedValues = this.TextBoxAllowedValues.Text.Split(Environment.NewLine).Select(x => (object?)x).ToList();
        slug.DisallowedValues = this.TextBoxDisallowedValues.Text.Split(Environment.NewLine).Select(x => (object?)x)
            .ToList();
        slug.RequiresUserInput = this.CheckBoxRequiresUserInput.IsChecked ?? false;

        if (slug.DisplayName != this._currentEditingSlug)
        {
            this._slugCache[slug.DisplayName] = slug;
            this._slugCache.Remove(this._currentEditingSlug);
            this._currentEditingSlug = slug.DisplayName;
            UpdateSlugChoiceComboBox();
            this.ComboBoxSlugs.SelectedItem = this._currentEditingSlug;
        }
    }

    private void ComboBoxBoxSlugType_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is not null)
        {
            var slugType = this._slugTypesByName[e.AddedItems[0].ToString()];
            this.TextBoxSpecialValues.Text = this._messageExtras[slugType];
            this.PanelNonGuidOptions.IsVisible = slugType != SlugType.RandomGuid;
        }
    }

    private void ButtonDeleteSlug_OnClick(object? sender, RoutedEventArgs e)
    {
        this._slugCache.Remove(this._currentEditingSlug);
        this._currentEditingSlug = string.Empty;
        ResetSlugOptions();
        UpdateSlugChoiceComboBox();
    }

    private void UpdateSlugChoiceComboBox()
    {
        this.ComboBoxSlugs.ItemsSource = this._slugCache.Values
            .Where(x => x.RequiresAnyInput)
            .Select(x => x.DisplayName).ToList();
    }
}
