using Microsoft.EntityFrameworkCore;
using NotesApp.API.Database.Context;
using NotesApp.API.Interfaces;
using NotesApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotesAppContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NotesAppDB")));

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INoteService, NoteService>();
builder.Services.AddSingleton<IAuthorizedUserService, AuthorizedUserService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
