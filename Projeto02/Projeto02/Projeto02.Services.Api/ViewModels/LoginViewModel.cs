using Flunt.Notifications;
using Flunt.Validations;
using Projeto02.Services.Api.Helpers;

namespace Projeto02.Services.Api.ViewModels
{
    /// <summary>
    /// Modelo de dados para o ENDPOINT /api/login
    /// </summary>
    public class LoginViewModel : Notifiable<Notification>
    {
        public string? Email { get; set; }
        public string? Senha { get; set; }

        public LoginViewModel MapTo()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Email, "Informe o email de acesso.")
                .IsEmail(Email, "Informe um endereço de email válido.")
                .IsNotNullOrEmpty(Email, "Informe a senha de acesso.")
                .IsGreaterOrEqualsThan(Senha, 6, "Senha do usuário deve ter no mínimo 6 caracteres.")
                );

            return new LoginViewModel
            {
                Email = Email,
                Senha = MD5Helper.Get(Senha)
            };
        }
    }
}
