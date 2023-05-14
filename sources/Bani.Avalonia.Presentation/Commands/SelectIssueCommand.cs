using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DustInTheWind.Bani.Avalonia.Application.SelectIssuer;
using DustInTheWind.Bani.Avalonia.Presentation.ViewModels;
using MediatR;

namespace DustInTheWind.Bani.Avalonia.Presentation.Commands
{
    public class SelectIssueCommand : ICommand
    {
        private readonly IMediator mediator;
        public event EventHandler CanExecuteChanged;

        public SelectIssueCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _ = SendRequest(parameter);
        }

        private async Task SendRequest(object parameter)
        {
            string issuerId = parameter is IssuerViewModel issuerViewModel
                ? issuerViewModel.IssuerInfo?.Id
                : null;
            
            SelectIssuerRequest request = new()
            {
                IssuerId = issuerId
            };

            await mediator.Send(request);
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}