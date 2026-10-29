using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Application.UseCases.CV.FinalizeCV;
using SmartRecruitment.Application.UseCases.CV.GetFinalizedCVEmbeddings;
using SmartRecruitment.Application.UseCases.CV.UpdateCVContent;
using SmartRecruitment.Application.UseCases.CV.UploadCV;
using SmartRecruitment.Infrastructure.AI;
using SmartRecruitment.Infrastructure.Persistence;
using SmartRecruitment.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CVDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICVRepository, CVRepository>();

// Temporary fake adapters - to be replaced by HTTP adapters calling the Python AI Service
builder.Services.AddScoped<IEmbeddingService, FakeEmbeddingService>();
builder.Services.AddScoped<ICVTextExtractor, FakeCVTextExtractor>();

builder.Services.AddScoped<UploadCVHandler>();
builder.Services.AddScoped<UpdateCVContentHandler>();
builder.Services.AddScoped<FinalizeCVHandler>();
builder.Services.AddScoped<GetFinalizedCVEmbeddingsHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Internal endpoints (/internal/*) are only meant for other services, never for the public.
// When InternalApiKey is configured, callers must send it in the X-Internal-Key header.
var internalKey = app.Configuration["InternalApiKey"];
app.Use(async (context, next) =>
{
    if (!string.IsNullOrEmpty(internalKey)
        && context.Request.Path.StartsWithSegments("/internal")
        && context.Request.Headers["X-Internal-Key"] != internalKey)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return;
    }

    await next();
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
