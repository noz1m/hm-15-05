using Infrastructure.Data;
using Infrastructure.Interface;
using Infrastructure.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DataContext, DataContext>(); 
builder.Services.AddScoped<IBookService, BookingService>(); 
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My App"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();


// create table books
// (
// 	id serial primary key,
// 	title varchar(50) not null,
// 	genre varchar(50) not null,
// 	publicationYear int,
// 	totalCopies int,
// 	availableCopies int
// );

// select * from books

// create table members
// (
// 	id serial primary key,
// 	fullname varchar(50) not null,
// 	phone varchar(50) unique not null,
// 	email varchar(100),
// 	membershipDate date
// );

// select * from members

// create table borrowings
// (
// 	id serial primary key,
// 	bookId int references books(id),
// 	memberId int references members(id),
// 	borrowDate date,
// 	dueDate date,
// 	returnDate date,
// 	fine decimal(10,2)
// );

// select * from borrowings