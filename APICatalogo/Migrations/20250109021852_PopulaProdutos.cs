using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                "Values('Coca-cola Diet', 'Refrigerante de cola 350ml', 5.45, 'cocacola.jpg', 50, now(),1)");

            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                "Values('misto-quente', 'pão de forma queijo e presunto', 7.50, 'mistoquente.jpg', 10, now(),2)");

            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                "Values('Bolo de coco', 'Bolo de coco gelado', 5.99, 'bolodecoco.jpg', 8, now(),3)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");

        }
    }
}
