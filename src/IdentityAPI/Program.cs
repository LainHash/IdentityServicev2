using IdentityAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEnvConfig(builder.Configuration);
builder.Services.AddJwtConfig(builder.Configuration);
builder.Services.AddMyServices(builder.Configuration);
builder.Services.AddCustomSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            if (httpReq.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix) &&
                !string.IsNullOrWhiteSpace(prefix))
            {
                swaggerDoc.Servers =
                [
                    new Microsoft.OpenApi.Models.OpenApiServer { Url = prefix.ToString() }
                ];
            }
        });
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
