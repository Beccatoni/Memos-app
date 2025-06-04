using MemosApplication.Api.Controllers;
using MemosApplication.Api.DTOs;
using MemosApplication.Api.Models;
using MemosApplication.Api.Repository;
using MemosApplication.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemosApplication.Tests.ControllersTests;

[TestFixture]
public class MemoControllerTests
{
    private MemoController _controller;
    private AppDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb").Options;
        _context = new AppDbContext(options);

        _context.Memos.Add(new Memo { Id = 1, Title = "First memo", Content = "Of course the first is the best!" });
        _context.SaveChanges();

        var repository = new MemoRepository(_context);
        var service = new MemoService(repository);
        _controller = new MemoController(service);
    }

    [Test]
    public void GetMemos_ReturnsOkWithAllMemos()
    {
        var response = _controller.GetMemos();
        
        Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = response.Result as OkObjectResult;
        var memos = (okResult.Value as IEnumerable<MemoDto>).ToList();
        
        Assert.Multiple(()=>
        {
            Assert.That(memos, Is.Not.Empty);
            Assert.That(memos.Count(), Is.EqualTo(1));
        });
        
    }
    
    [Test]
    
    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

}