using System.Collections.Generic;
using System.Linq;

namespace RCN.API.Data
{
public class ProdutoRepository : IProdutoRepository
{
    private readonly ProdutoContexto Contexto;
    public ProdutoRepository(ProdutoContexto contexto)
    {
        Contexto = contexto;
    }
    public void Inserir(Produto produto)
    {
        Contexto.Produtos.Add(produto);
        Contexto.SaveChanges();
    }
    public void Editar(Produto produto)
    {
        Contexto.Produtos.Update(produto);
        Contexto.SaveChanges();
    }

    public void Excluir(Produto produto)
    {
        Contexto.Produtos.Remove(produto);
        Contexto.SaveChanges();
    }
    
        public Produto Obter(string codigo)
    {
        return Contexto.Produtos.Where(w=> w.Codigo == codigo).FirstOrDefault();
    }

    public Produto Obter(int Id)
    {
        return Contexto.Produtos.Find(Id);
    }
    public IEnumerable<Produto> Obter()
    {
        return Contexto.Produtos.ToList();
    }

}

}