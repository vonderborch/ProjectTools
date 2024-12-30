namespace ProjectTools.App.ViewModels;

public class OkDialogBoxViewModel : ViewModelBase
{
    public OkDialogBoxViewModel(string title, string description)
    {
        this.Title = title;
        this.Description = description;
        this.ResultIsOk = null;
    }

    public string Description { get; set; }

    public bool? ResultIsOk { get; set; }

    public string Title { get; set; }

    public string OkText { get; set; } = "OK";

    public string CancelText { get; set; } = "Cancel";

    public void SetResult(bool result)
    {
        this.ResultIsOk = result;
    }
}
