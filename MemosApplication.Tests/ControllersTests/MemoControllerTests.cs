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
    public void CreateMemo_ReturnsCreatedResult()
    {
        //Arrange 
        var newMemo = new CreateMemoDto
        {
            Title = "New memo",
            Content = "I am a new memo with new content."
        };
        
        //Act
        var response = _controller.CreateMemo(newMemo);
        
        // Assert
        Assert.That(response, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = response as CreatedAtActionResult;

        var createdMemo = _context.Memos.FirstOrDefault(memo => memo.Title == newMemo.Title);
        Assert.That(createdMemo, Is.Not.Null);
        
        Assert.Multiple(() =>
        {
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdMemo.Title, Is.EqualTo(newMemo.Title));
            Assert.That(createdMemo.Content, Is.EqualTo(newMemo.Content));
        });
    }

    [Test]
    public void CreateMemo_WithInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var newMemo = new CreateMemoDto
        {
            Title = "",
            Content = ""
        };
        
        //Act
        _controller.ModelState.AddModelError("Title", "Title is required.");
        _controller.ModelState.AddModelError("Content", "Content is required.");
        var response = _controller.CreateMemo(newMemo);
        
        //Assert 
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = response as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.TypeOf<SerializableError>());
    }

    [Test]
    public void UpdateMemo_ReturnsOkResult()
    {
        //Arrange
        var updateMemo = new UpdateMemoDto
        {
            Title = "Updated memo",
            Content = "I am an updated memo with updated content."
        };
        
        //Act
        var response = _controller.UpdateMemo(1, updateMemo);
        var updatedMemo = _context.Memos.FirstOrDefault(memo => memo.Id == 1);
        Assert.That(updatedMemo, Is.Not.Null);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.InstanceOf<NoContentResult>());
            Assert.That(updatedMemo.Title, Is.EqualTo(updateMemo.Title));
            Assert.That(updatedMemo.Content, Is.EqualTo(updateMemo.Content));
        });
        
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

}