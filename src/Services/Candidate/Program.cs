using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Application.UseCases.Candidate.ChangeCandidateStatus;
using SmartRecruitment.Application.UseCases.Candidate.CreateCandidate;
using SmartRecruitment.Application.UseCases.Candidate.InterviewTransitions;
using SmartRecruitment.Infrastructure.Persistence;
using SmartRecruitment.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CandidateDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

builder.Services.AddScoped<CreateCandidateHandler>();
builder.Services.AddScoped<ChangeCandidateStatusHandler>();
builder.Services.AddScoped<CandidateInterviewTransitionHandler>();

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
