#region

using ProjectTools.App.DataContexts.GenerateProjectSubContexts;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts;

public class GenerateProjectDataContext : ReactiveObject
{
    public GenerateProjectDataContext()
    {
        this.TemplateSelectionContext = new TemplateSelectionDataContext();
    }

    public TemplateSelectionDataContext TemplateSelectionContext { get; }
}
