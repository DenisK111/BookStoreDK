using BookStoreDK;

var builder = WebApplication.CreateBuilder(args);

//add Services in IoCContainer Class
var app = builder.AddServicesAndBuild();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
