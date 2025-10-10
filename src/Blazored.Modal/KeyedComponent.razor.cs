using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazored.Modal;

internal class KeyedComponent : ComponentBase
{
    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ChildContent(builder);
    }
}