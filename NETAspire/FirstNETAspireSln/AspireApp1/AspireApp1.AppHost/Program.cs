var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireApp1_ApiService>("apiservice");

builder.AddProject<Projects.AspireApp1_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.SSEDemoSln>("ssedemosln");

builder.AddProject<Projects.WatchDogSlns>("watchdogslns");

builder.AddProject<Projects.DotNetify_Pulse>("dotnetify-pulse");

builder.AddProject<Projects.Knife4jSln>("knife4jsln");

builder.AddProject<Projects.ScalarSln>("scalarsln");

builder.AddProject<Projects.Scalar2Sln>("scalar2sln");

builder.AddProject<Projects.AuthServer>("authserver");

builder.AddProject<Projects.WebA>("weba");

builder.AddProject<Projects.WebB>("webb");

builder.Build().Run();
