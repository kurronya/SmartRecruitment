using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Application.UseCases.Interview.CancelInterview;
using SmartRecruitment.Application.UseCases.Interview.MarkInterviewCompleted;
using SmartRecruitment.Application.UseCases.Interview.RecordInterviewResult;
using SmartRecruitment.Application.UseCases.Interview.ScheduleInterview;
using SmartRecruitment.Application.UseCases.JobDescription.ApproveJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.PublishJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.RejectJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.StartJobDescriptionReview;
using SmartRecruitment.Application.UseCases.JobDescription.SubmitJobDescription;
using SmartRecruitment.Application.UseCases.Matching.MatchCandidatesForJobDescription;
using SmartRecruitment.Infrastructure.AI;
using SmartRecruitment.Infrastructure.Gateways;
using SmartRecruitment.Infrastructure.Persistence;
using SmartRecruitment.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RecruitmentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IJobDescriptionRepository, JobDescriptionRepository>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();

// Temporary fake adapter - to be replaced by an HTTP adapter calling the Python AI Service
builder.Services.AddScoped<IEmbeddingService, FakeEmbeddingService>();

// HTTP gateways to the services that own Candidate, Company and CV data
builder.Services.AddHttpClient<ICandidateGateway, CandidateHttpGateway>(client =>
    ConfigureInternalClient(client, builder.Configuration, "Candidate"));
builder.Services.AddHttpClient<ICompanyGateway, CompanyHttpGateway>(client =>
    ConfigureInternalClient(client, builder.Configuration, "Company"));
builder.Services.AddHttpClient<ICVGateway, CVHttpGateway>(client =>
    ConfigureInternalClient(client, builder.Configuration, "CV"));

builder.Services.AddScoped<SubmitJobDescriptionHandler>();
builder.Services.AddScoped<StartJobDescriptionReviewHandler>();
builder.Services.AddScoped<ApproveJobDescriptionHandler>();
builder.Services.AddScoped<RejectJobDescriptionHandler>();
builder.Services.AddScoped<PublishJobDescriptionHandler>();

builder.Services.AddScoped<ScheduleInterviewHandler>();
builder.Services.AddScoped<MarkInterviewCompletedHandler>();
builder.Services.AddScoped<RecordInterviewResultHandler>();
builder.Services.AddScoped<CancelInterviewHandler>();

builder.Services.AddScoped<MatchCandidatesForJobDescriptionHandler>();

static void ConfigureInternalClient(HttpClient client, IConfiguration configuration, string serviceName)
{
    // Base address must end with '/' so relative paths like "internal/..." are appended correctly
    client.BaseAddress = new Uri(configuration[$"Services:{serviceName}"]
        ?? throw new InvalidOperationException($"Missing configuration value Services:{serviceName}"));
    client.Timeout = TimeSpan.FromSeconds(15);

    var internalKey = configuration["InternalApiKey"];
    if (!string.IsNullOrEmpty(internalKey))
        client.DefaultRequestHeaders.Add("X-Internal-Key", internalKey);
}


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
