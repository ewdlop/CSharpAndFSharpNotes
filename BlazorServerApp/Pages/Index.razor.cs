using BlazorServerApp.Data.CQRS;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace BlazorServerApp.Pages
{
    public partial class Index : ComponentBase, INotificationHandler<UpdateColorCommand>
    {
        public Task Handle(UpdateColorCommand notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
