using LaverieController.Modele.Business;
using LaverieController.Modele.Domaine;
using LaverieController.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProprietaireDAO, ProprietaireRepoImp>();
builder.Services.AddScoped<ConfigurationBusiness>();
builder.Services.AddScoped<IMachineDAO, MachineRepoImp>();
builder.Services.AddScoped<MachineBusiness>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
