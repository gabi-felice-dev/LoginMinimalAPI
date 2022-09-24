using Flunt.Notifications;
using Flunt.Validations;

namespace Projeto02.Services.Api.ViewModels
{
    /// <summary>
    /// Modelo de dados para o ENDPOINT /api/password-recover
    /// </summary>
    public class PasswordRecoverViewModel : Notifiable<Notification>
    {
        public string? Email { get; set; }

        public PasswordRecoverViewModel MapTo()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Email, "Informe o email de acesso.")
                .IsEmail(Email, "Informe um endereço de email válido."));

            return new PasswordRecoverViewModel
            {
                Email = Email,
            };
        }
    }
}
