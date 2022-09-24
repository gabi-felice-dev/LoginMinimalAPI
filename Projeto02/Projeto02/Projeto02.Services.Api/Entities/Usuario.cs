namespace Projeto02.Services.Api.Entities
{
    /// <summary>
    /// Modelo de entidade
    /// </summary>
    public class Usuario
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public DateTime? DataCriacao { get; set; }
    }
}
