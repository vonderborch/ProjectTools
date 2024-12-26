using System.Text;

namespace ProjectTools.CL.MenuSystem;

/// <summary>
///     A simple console menu.
/// </summary>
public class Menu
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Menu" /> class.
    /// </summary>
    /// <param name="programTitle">The program title.</param>
    /// <param name="menuTitle">The menu title.</param>
    /// <param name="menuItems">The menu items.</param>
    /// <param name="menuHelpText">The menu help items.</param>
    /// <param name="menuItemsAdditionalArgs">The menu items additional arguments.</param>
    /// <param name="selectedItemId">The selected item id. Defaults to 0.</param>
    /// <param name="includeExit">Whether or not to include the exit option. Defaults to true.</param>
    /// <exception cref="ArgumentException">Raised if we have more items to display than the menu supports.</exception>
    public Menu(string programTitle, string menuTitle, List<string> menuItems, List<string> menuHelpText,
        List<string> menuItemsAdditionalArgs,
        int selectedItemId = 0,
        bool includeExit = true)
    {
        this.ProgramTitle = programTitle;
        this.MenuTitle = menuTitle;
        this.MenuItems = [..menuItems];
        if (includeExit)
        {
            this.MenuItems.Add("EXIT");
            menuHelpText.Add("Exits the program");
            menuItemsAdditionalArgs.Add("");
        }

        if (this.MenuItems.Count > 26)
        {
            throw new ArgumentException("The menu can only have up to 26 items. Time to upgrade it!");
        }

        this.SelectedItemId = selectedItemId;
        this.MenuItemMapping = new Dictionary<ConsoleKey, string>();
        this.MenuItemKeys = new Dictionary<string, string>();
        this.MenuItemsHelpText = new Dictionary<string, string>();
        this.MenuItemsAdditionalArgs = new Dictionary<string, string>();
        for (var i = 0; i < this.MenuItems.Count; i++)
        {
            this.MenuItemMapping.Add((ConsoleKey)(i + 65), this.MenuItems[i]);
            this.MenuItemKeys.Add(this.MenuItems[i], ((char)(i + 65)).ToString());
            this.MenuItemsHelpText.Add(this.MenuItems[i], menuHelpText[i]);
            this.MenuItemsAdditionalArgs.Add(this.MenuItems[i], menuItemsAdditionalArgs[i]);
        }
    }

    /// <summary>
    ///     The title of the program.
    /// </summary>
    private string ProgramTitle { get; }

    /// <summary>
    ///     The title of the menu.
    /// </summary>
    private string MenuTitle { get; }

    /// <summary>
    ///     The help text for the menu items.
    /// </summary>
    private Dictionary<string, string> MenuItemsHelpText { get; }

    /// <summary>
    ///     The additional args for the menu items.
    /// </summary>
    private Dictionary<string, string> MenuItemsAdditionalArgs { get; }

    /// <summary>
    ///     The menu items.
    /// </summary>
    private List<string> MenuItems { get; }

    /// <summary>
    ///     Mapping of console keys to menu items.
    /// </summary>
    private Dictionary<ConsoleKey, string> MenuItemMapping { get; }

    /// <summary>
    ///     The keys to display for the menu items.
    /// </summary>
    private Dictionary<string, string> MenuItemKeys { get; }

    /// <summary>
    ///     The selected item id.
    /// </summary>
    private int SelectedItemId { get; set; }

    /// <summary>
    ///     Displays the menu and returns the selected item.
    /// </summary>
    /// <param name="selectedItemId">The starting selected item id.</param>
    /// <returns>The selected menu item.</returns>
    public string DisplayMenu(int? selectedItemId = null)
    {
        this.SelectedItemId = selectedItemId ?? this.SelectedItemId;

        while (true)
        {
            Display();
            var selection = GetUserSelection();
            if (selection != null)
            {
                return selection;
            }
        }
    }

    /// <summary>
    ///     Gets the user's selection.
    /// </summary>
    /// <returns>Null if no selection, a string if an item was selected.</returns>
    private string? GetUserSelection()
    {
        var key = Console.ReadKey().Key;

        switch (key)
        {
            case ConsoleKey.Enter:
                return GetCommandLineArgsForMenuItem(this.MenuItems[this.SelectedItemId]);
            case ConsoleKey.UpArrow:
                this.SelectedItemId--;
                if (this.SelectedItemId < 0)
                {
                    this.SelectedItemId = this.MenuItems.Count - 1;
                }

                break;
            case ConsoleKey.DownArrow:
                this.SelectedItemId++;
                if (this.SelectedItemId >= this.MenuItems.Count)
                {
                    this.SelectedItemId = 0;
                }

                break;
            default:
                if (this.MenuItemMapping.TryGetValue(key, out var menuItem))
                {
                    return GetCommandLineArgsForMenuItem(menuItem);
                }

                break;
        }

        return null;
    }

    private string GetCommandLineArgsForMenuItem(string menuItem)
    {
        var args = this.MenuItemsAdditionalArgs[menuItem];
        return string.IsNullOrWhiteSpace(args) ? menuItem : $"{menuItem} {args}";
    }

    /// <summary>
    ///     Outputs the menu to the console.
    /// </summary>
    private void Display()
    {
        var menu = new StringBuilder();
        menu.AppendLine(this.ProgramTitle);
        menu.AppendLine(this.MenuTitle);


        for (var i = 0; i < this.MenuItems.Count; i++)
        {
            var menuItemKey = this.MenuItemKeys[this.MenuItems[i]];
            var selectionPart = this.SelectedItemId == i ? "> " : "  ";
            var line =
                $"{selectionPart}{menuItemKey}: {this.MenuItems[i]} - {this.MenuItemsHelpText[this.MenuItems[i]]}";
            menu.AppendLine(line);
        }

        Console.Clear();
        Console.Write(menu.ToString());
    }
}
