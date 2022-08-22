using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyWorldDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyWorldDbConnection"));
});
 
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();

//Get Endpoint
app.MapGet("/Produtos/create", async(Produtos Produtos, MyWorldDbContext myWorldDb) => {
    myWorldDb.Produtos.Add(Produtos);
    await myWorldDb.SaveChangesAsync();
    return Results.Ok();
});

//Post Endpoint
app.MapPost("/Produtos", async(MyWorldDbContext myWorldDb) => {
    var Produtos = await myWorldDb.Produtos.ToListAsync();
    return Results.Ok();
});

//Put Endpoint
app.MapPut("/Produtos/update", async(Produtos gadgetToUpdate, MyWorldDbContext myWorldDbContext) => {
    var dbGadget = await myWorldDbContext.Produtos.FindAsync(gadgetToUpdate.Id);
    if(dbGadget == null) {
        return Results.NotFound();
    }

    dbGadget.Marca = gadgetToUpdate.Marca;
    dbGadget.Preco = gadgetToUpdate.Preco;
    dbGadget.Nome = gadgetToUpdate.Nome;
    dbGadget.Tipo = gadgetToUpdate.Tipo;

    await myWorldDbContext.SaveChangesAsync();
    return Results.NoContent();
});

//Delete Endpoint
app.MapDelete("/Produtos/delete/{id}", async(int id, MyWorldDbContext myWorldDbContext) => {
    var dbProdutos = await myWorldDbContext.Produtos.FindAsync(id);
    if(dbProdutos == null){
        return Results.NoContent();
    }

    myWorldDbContext.Produtos.Remove(dbProdutos);
    await myWorldDbContext.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

// Dbcontext
public class MyWorldDbContext:DbContext
{
    public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options):base(options)
    {
        
    }
    public DbSet<Produtos> Produtos { get; set; }
}
 
// Classes
public class Produtos
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Marca { get; set; }
    public decimal Preco { get; set; }
    public string Tipo { get; set; }
}