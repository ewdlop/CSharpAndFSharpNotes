using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace BlazorApp1.Pages;

public partial class Index
{
    private async ValueTask<ItemsProviderResult<int>> LoadEmployees(
        ItemsProviderRequest request)
    {
        var test = Enumerable.Range(0, 1000);
        if(1000 - request.StartIndex > 0)
        {
            var numEmployees = Math.Min(request.Count, 1000 - request.StartIndex);
            var employees = test.Skip(request.StartIndex).Take(numEmployees);

            return new ItemsProviderResult<int>(employees, test.Count());
        }
        return new ItemsProviderResult<int>(Enumerable.Empty<int>(), 0);
    }

}