using Flunt.Notifications;
using Flunt.Validations;
using Projeto02.Services.Api.Entities;
using Projeto02.Services.Api.Helpers;

namespace Projeto02.Services.Api.ViewModels
{
    /// <summary>
    /// Modelo de dados para o ENDPOINT /api/register
    /// </summary>
    public class RegisterViewModel : Notifiable<Notification>
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }

        public Usuario MapTo()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Nome, "Informe o nome do usuário.")
                .IsGreaterOrEqualsThan(Nome, 6, "Nome do usuário deve ter no mínimo 6 caracteres.")
                .IsNotNullOrEmpty(Email, "Informe o email do usuário.")
                .IsEmail(Email, "Informe um endereço de email válido.")
                .IsNotNullOrEmpty(Senha, "Informe a senha do usuário.")
                .IsGreaterOrEqualsThan(Senha, 6, "Senha do usuário deve ter no mínimo 6 caracteres.")
                );

            return new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = Nome,
                Email = Email,
                Senha = MD5Helper.Get(Senha),
                DataCriacao = DateTime.Now
            };
        }
    }
}
