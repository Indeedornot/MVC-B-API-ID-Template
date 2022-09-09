// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using IDServer;
using IDServer.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authentication.Certificate;

#region logging
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
  .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
  .MinimumLevel.Override("System", LogEventLevel.Warning)
  .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
  .Enrich.FromLogContext()
  // uncomment to write to Azure diagnostics stream
  //.WriteTo.File(
  //    @"D:\home\LogFiles\Application\identityserver.txt",
  //    fileSizeLimitBytes: 1_000_000,
  //    rollOnFileSizeLimit: true,
  //    shared: true,
  //    flushToDiskInterval: TimeSpan.FromSeconds(1))
  .WriteTo.Console(
    outputTemplate:
    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
    theme: AnsiConsoleTheme.Code
  )
  .CreateLogger();


Log.CloseAndFlush();
#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddIdentityServer(
    options => {
      options.Events.RaiseErrorEvents = true;
      options.Events.RaiseInformationEvents = true;
      options.Events.RaiseFailureEvents = true;
      options.Events.RaiseSuccessEvents = true;

      // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
      options.EmitStaticAudienceClaim = true;
    }
  )
  .AddTestUsers(TestUsers.Users)
  .AddInMemoryIdentityResources(Config.IdentityResources)
  .AddInMemoryApiScopes(Config.ApiScopes)
  .AddInMemoryClients(Config.Clients)
  // ^ in-memory, code config
  .AddDeveloperSigningCredential();
// ^ not recommended for production - you need to store your key material somewhere secure

builder.Services.AddAuthentication()
  // .AddGoogle(options =>
  // {
  //     options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
  //
  //     // register your IdentityServer with Google at https://console.developers.google.com
  //     // enable the Google+ API
  //     // set the redirect URI to https://localhost:5001/signin-google
  //     options.ClientId = "copy client ID from Google here";
  //     options.ClientSecret = "copy client secret from Google here";
  // });
  .AddCertificate(
    options => {
      options.AllowedCertificateTypes = CertificateTypes.All;
      options.RevocationMode = X509RevocationMode.NoCheck;
    }
  );

var app = builder.Build();

if (app.Environment.IsDevelopment()){
  app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(
  endpoints => {
    endpoints.MapDefaultControllerRoute()
      .RequireAuthorization();
  }
);

app.Run();