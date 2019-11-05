using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RCN.API.Data;

namespace RCN.API.Controllers
{

[Route("v{version:apiVersion}/[controller]")]
//  [ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository Repositorio;

    public ProdutosController(IProdutoRepository repositorio)
    {
        Repositorio = repositorio;
    }
    
    [HttpGet]
    [ApiVersion("1.0")]
    [ResponseCache(Duration=30)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public IActionResult Get()
    {
        var lista = Repositorio.Obter();
        return Ok(lista);
    }
    
    [HttpGet("{id}")]
    [ApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [Produces("application/json","application/xml")]
    public IActionResult GetPorId(int Id)
    {
    var produto = Repositorio.Obter(Id);
    
    if (produto == null) return NotFound();
        return Ok(produto);
    }

    [HttpGet("{codigo}")]
    [ApiVersion("2.0")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [Produces("application/json","application/xml")]
    public IActionResult ObterPorCodigo(string codigo)
    {
    var produto = Repositorio.Obter(codigo);
    
    if (produto == null) return NotFound();
        return Ok(produto);
    }


[HttpPost]
[ApiVersion("1.0")]
[ProducesResponseType((int)HttpStatusCode.Created)]
[ProducesResponseType((int)HttpStatusCode.BadRequest)]
public IActionResult Criar([FromBody]Produto produto)
{

    if(produto.Codigo == "" )
        return BadRequest("Codigo do produto não informado");

    if(produto.Descricao == "" )
    return BadRequest("Descricao do produto não informado");

    Repositorio.Inserir(produto);

    return Created(nameof(Criar), produto);
}

[HttpPut]
[ApiVersion("1.0")]
[ProducesResponseType((int)HttpStatusCode.NotFound)]
[ProducesResponseType((int)HttpStatusCode.BadRequest)]
[ProducesResponseType((int)HttpStatusCode.NoContent)]
public IActionResult Atualizar ([FromBody]Produto  produto)
{

    var prod = Repositorio.Obter(produto.Id);

    if (prod == null) return NotFound();

    if(produto.Codigo == "" )
    return BadRequest("Codigo do produto não informado");

    if(produto.Descricao == "" )
    return BadRequest("Descricao do produto não informado");

    Repositorio.Editar(produto);

    return NoContent();
}

    [HttpDelete("{Id}")]
    [ApiVersion("1.0")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public IActionResult Remover (int Id)
    {
            var prod = Repositorio.Obter(Id);
        if (prod == null) return NotFound();
    
        Repositorio.Excluir(prod);
    return Ok();
    }

    [HttpGet]
    [ApiVersion("3.0")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    // [Produces("application/json","application/xml")]
    public IActionResult ObterTodos()
    {
        List<string> lista = new List<string>();
    
        for (int i = 0; i < 10000; i++)
        {
            lista.Add($"indice: {i}");
        }
        return Ok(string.Join(';',lista));
    
    }

}


}