using BookHub.Client.Models;
using Microsoft.AspNetCore.Components;

public class CategoryFormBase : ComponentBase
{
    [Parameter] public CategoryCreateDto Model { get; set; } = new();
    [Parameter] public EventCallback<CategoryCreateDto> OnSubmit { get; set; }
    [Parameter] public EventCallback OnCancelClick { get; set; }
    [Parameter] public string ButtonText { get; set; } = "Lưu";

    protected async Task HandleValidSubmit()
    {
        await OnSubmit.InvokeAsync(Model);
    }

    protected async Task OnCancel()
    {
        await OnCancelClick.InvokeAsync();
    }
}
