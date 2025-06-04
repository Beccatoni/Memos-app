using MemosApplication.Api.Models;
using MemosApplication.Api.Repository;
using Microsoft.EntityFrameworkCore;


namespace MemosApplication.Tests.RepositoryTests;

[TestFixture]
public class MemoRepositoryTests
{
    private AppDbContext _context;
    private MemoRepository _repository;

   
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;
        _context = new AppDbContext(options);
        _repository = new MemoRepository(_context);
    }

    [Test]
    public void GetAllMemos_ReturnsAllMemos()
    {
        _context.Memos.AddRange(new List<Memo>
        {
            new Memo {Id = 1, Title = "Testing", Content = "Always testing your application is what can save you time."},
            new Memo {Id = 2, Title = "Containerize", Content = "Containerize your application to make it portable and easy to deploy."},
        });
        _context.SaveChanges();

        var result = _repository.GetAllMemos().ToList();
        
        Assert.That(result.Count, Is.EqualTo(2));
        
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }


   
}