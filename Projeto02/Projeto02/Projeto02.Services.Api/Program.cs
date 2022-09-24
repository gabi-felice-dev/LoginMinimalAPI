using Bogus;
using Microsoft.EntityFrameworkCore;
using Projeto02.Services.Api.Contexts;
using Projeto02.Services.Api.Helpers;
using Projeto02.Services.Api.Security;
using Projeto02.Services.Api.ViewModels;

var builder = WebApplication.CreateBuilder(args);

//configurando o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configurando o contexto do EntityFramework
var connectionString = builder.Configuration.GetConnectionString("BDProjeto02");
builder.Services.AddDbContext<SqlServerContext>
    (options => options.UseSqlServer(connectionString));

//configurando o JWT (Autentica��o)
JwtConfiguration.AddJwtBearerConfiguration(builder);

//configurando inje��o de depend�ncia para a classe de envio de email
builder.Services.AddTransient<EmailHelper>
    (map => new EmailHelper(
            builder.Configuration.GetValue<string>("EmailSettings:Conta"),
            builder.Configuration.GetValue<string>("EmailSettings:Senha"),
            builder.Configuration.GetValue<string>("EmailSettings:Smtp"),
            builder.Configuration.GetValue<int>("EmailSettings:Porta")
        ));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.MapPost("/api/register", (SqlServerContext context, RegisterViewModel model) =>
{
    var usuario = model.MapTo();
    if(!model.IsValid)
        return Results.BadRequest(model.Notifications);

    if(context.Usuarios.FirstOrDefault(u => u.Email.Equals(model.Email)) != null)
        return Results.BadRequest(new { message = "O email informado j� est� cadastrado, tente outro." });
        
    context.Usuarios.Add(usuario);
    context.SaveChanges();

    return Results.Ok(
        new 
        { 
            usuario.Id, usuario.Nome, usuario.Email, usuario.DataCriacao 
        });
});

app.MapPost("/api/login", (SqlServerContext context, JwtTokenService service, LoginViewModel model) =>
{
    var login = model.MapTo();
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    var usuario = context.Usuarios
        .FirstOrDefault(u => u.Email.Equals(login.Email)
                          && u.Senha.Equals(login.Senha));

    //se usu�rio n�o encontrado..
    if (usuario == null)
        return Results.BadRequest(new { message = "Acesso negado. Usu�rio inv�lido." });

    //retornando sucesso e gerando o token
    return Results.Ok(
        new 
        { 
            usuario.Id, usuario.Nome, usuario.Email, usuario.DataCriacao,
            accessToken = service.Get(usuario.Email)
        });
});

app.MapPost("/api/password-recover", (SqlServerContext context, EmailHelper emailHelper, PasswordRecoverViewModel model) =>
{
    var passwordRecover = model.MapTo();
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    //consultando os dados do usu�rio atrav�s do email
    var usuario = context.Usuarios
        .FirstOrDefault(u => u.Email.Equals(passwordRecover.Email));

    //verificar se nenhum usu�rio foi encontrado
    if (usuario == null)
        return Results.BadRequest(new { message = "Email n�o cadastrado." });

    //gerando uma senha nova para o usu�rio
    var faker = new Faker();
    var novaSenha = faker.Internet.Password(8);

    //escrevendo a mensagem que ser� enviada para o usu�rio
    var texto = @$"
        <div>
            Ol�, <strong>{usuario.Nome}</strong> <br/><br/>
            Foi gerada uma nova senha de acesso para o seu usu�rio. <br/>
            Utilize a senha: <strong>{novaSenha}</strong> para acessar o sistema. <br/><br/>
            Voc� poder� atualizar esta senha para outra de sua prefer�ncia.<br/><br/>
            Att, <br>
            Equipe COTI Inform�tica
        </div>
    ";

    //enviando um email para o usu�rio com a nova senha
    emailHelper.Send(usuario.Email, "Nova senha gerada com sucesso - COTI Inform�tica", texto);

    //atualizando a senha no banco de dados
    usuario.Senha = MD5Helper.Get(novaSenha);
    context.Entry(usuario).State = EntityState.Modified;
    context.SaveChanges();

    return Results.Ok(new { usuario.Id, usuario.Nome, usuario.Email, usuario.DataCriacao });
});

app.Run();
