using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore; // 
using TmsApi.Configuration;
using TmsApi.Services;
using TmsApi.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); 
builder.Services.AddOpenApi();    
builder.Services.AddAuthentication(); 
builder.Services.AddAuthorization();  
builder.Services.AddSingleton<EnrollmentWorker>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddOptions<PaymentOptions>()
   .BindConfiguration("Payments") 
    .ValidateDataAnnotations()
    .ValidateOnStart(); 
builder.Services.AddProblemDetails(); 
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

var app = builder.Build();
if (app.Environment.IsDevelopment()) 
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi(); 
    app.MapScalarApiReference(); 
}
else
{
 app.UseExceptionHandler(); // 
}
app.UseStatusCodePages(); 
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapControllers(); 
app.MapGet("/api/enrollments/worker-smoke", (EnrollmentWorker worker) =>
{
    worker.ProcessBatch();
    return Results.Ok("processed");
});

app.Run();