using System;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UCompanyContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));


var app = builder.Build();
// URLs bound to HTTP Server from Where API is accessible
app.Urls.Add("http://localhost:3000");
app.Urls.Add("http://localhost:4000");

// Get Request

app.MapGet("/api/departments",async (UCompanyContext context) => {
    var records = await context.Departments.ToListAsync();
    return Results.Ok(records);
});

// Post Request

app.MapPost("/api/departments", async (Department dept, UCompanyContext context) => {
    await context.AddAsync(dept);
    await context.SaveChangesAsync();
    return Results.Ok("Record Created Successfully");
});

// Put Request
app.MapPut("/api/departments/{id}", async (int id, Department dept, UCompanyContext context) => {
    var deptToUpdate = await context.Departments.FindAsync(id);
    if (deptToUpdate == null)
        return Results.NotFound("Record to Update is not Found");
    else
    {
        deptToUpdate.DeptName = dept.DeptName;
        deptToUpdate.Location = dept.Location;
        deptToUpdate.Capacity = dept.Capacity;
        await context.SaveChangesAsync();
        return Results.Ok("Record Updated Successfully");
    }
});
// Delete Request
app.MapDelete("/api/departments/{id}", async (int id, UCompanyContext context) => {
    var dept = await context.Departments.FindAsync(id);
    if (dept == null)
        return Results.NotFound("Record to Delete is not Found");
    context.Remove<Department>(dept);
    await context.SaveChangesAsync();
    return Results.Ok("Record Deleted Successfully");
});


app.Run();

 