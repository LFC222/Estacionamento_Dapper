using Dapper;
using estacionamento_dapper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace estacionamento_dapper.Controllers;

[Route("/valores")]
public class ValorDoMinutoController : Controller
{
    private readonly IDbConnection _connection;
    public ValorDoMinutoController(IDbConnection connection)
    {
        _connection = connection;
    }

    public IActionResult Index()
    {
        var valores = _connection.Query<ValoreDoMinuto>("select * from valores");
        return View(valores);
    }

    [HttpGet("/novo")]
    public IActionResult Novo()
    {
        return View();
    }

    [HttpPost("/criar")]
    public IActionResult Criar([FromForm] ValoreDoMinuto valoreDoMinuto)
    {
        var sql = "insert into valores(minutos, valor) values (@Minutos, @Valor)";
        _connection.Execute(sql, valoreDoMinuto);
        return Redirect("/valores");
    }

    [HttpPost("/{id}/apagar")]
    public IActionResult Apagar([FromRoute] int id)
    {
        var sql = "delete from valores where id=@id";
        _connection.Execute(sql, new ValoreDoMinuto{ Id = id });
        return Redirect("/valores");
    }

    [HttpGet("/{id}/alterar")]
    public IActionResult Alterar([FromRoute] int id)
    {
        var valor = _connection.Query<ValoreDoMinuto>("select * from valores where id = @id", new ValoreDoMinuto { Id = id}).FirstOrDefault();
        return View(valor);
    }

    [HttpPost("/{id}/editar")]
    public IActionResult Editar([FromRoute] int id, [FromForm] ValoreDoMinuto valoreDoMinuto)
    {
        valoreDoMinuto.Id = id;

        var sql = "update valores set Minutos=@Minutos,valor=@Valor where id=@id";
        _connection.Execute(sql, valoreDoMinuto);
        return Redirect("/valores");
    }
}
