// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using Microsoft.AspNetCore.DataProtection;
using WebServerAndSerial.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppConfiguration config = AppConfiguration.Load();
builder.Services.AddSingleton(config);

SwitchManagement swch = null!;
try
{
    swch = new SwitchManagement(config.SwitchNumberSwitches, config.SwitchMinimumDuration, config.SwitchMaximumDuration,
        config.SwitchMaximumAngle, config.SwitchMultiplexPins, config.SwitchPwmChip, config.SwitchPwmChannel);
    builder.Services.AddSingleton(swch);
}
catch 
{
// Nothing
}

// Need to figure out how to properly do this
var dir = new DirectoryInfo(@"/var/keys/");
if(!dir.Exists)
{
    dir.Create();
}

builder.Services.AddDataProtection().PersistKeysToFileSystem(dir);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

// app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
