using CleanArch.Application.Services;
using CleanArch.Domain.Repositories;
using CleanArch.Infrastructure;
using CleanArch.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
 * 
    +-------------------+
    |      API Layer     |  --> Depende das camadas Application e Infrastructure
    +-------------------+
            
    +-------------------+
    |  Application Layer |  --> Depende da camada Domain
    +-------------------+
            
    +-------------------+
    | Infrastructure     |  --> Depende da camada Domain e Application
    +-------------------+
            
    +-------------------+
    |    Domain Layer    |  --> Não depende de nenhuma camada
    +-------------------+
 * 
 */

builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<ICursoService, CursoService>();
builder.Services.AddScoped<IMatriculaService, MatriculaService>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();

builder.Services.AddDbContext<Contexto>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MinhaConexao")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
