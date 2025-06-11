using MemosApplication.Api.DTOs;
using MemosApplication.Api.Models;

namespace MemosApplication.Api.Repository;

public interface IMemoRepository
{
  IEnumerable<Memo> GetAllMemos();
  Memo? GetMemoById(int id);
  Memo FindMemoById(int id);
  void AddMemo(Memo memo);
  void UpdateMemo(UpdateMemoDto memo, int id);
  void DeleteMemo(int id);
  
}
public class MemoRepository : IMemoRepository
{
  private readonly AppDbContext _context;
  

  public MemoRepository(AppDbContext context)
  {
    _context = context;
  }
  public IEnumerable<Memo> GetAllMemos() => _context.Memos.ToList();

  public void AddMemo(Memo memo)
  {
    _context.Memos.Add(memo);
    _context.SaveChanges();
  }

  public Memo FindMemoById(int id)
  {
    var _memo = _context.Memos.FirstOrDefault(m => m.Id == id);
    if (_memo == null)
    {
      throw new KeyNotFoundException("Memo not found");
    }

    return _memo;
  }

  public Memo GetMemoById(int id)
  {
    var _memo = this.FindMemoById(id);
    return _memo;
  }
  
  public void UpdateMemo(UpdateMemoDto memo, int id)
  {
    var availableMemo = this.FindMemoById(id);

    if (memo.Title != null)
    {
      availableMemo.Title = memo.Title;
    }

    if (memo.Content != null)
    {
      availableMemo.Content = memo.Content;
    }
    
    availableMemo.UpdatedAt = DateTime.UtcNow; 
    _context.SaveChanges();
    
  }

  public void DeleteMemo(int id)
  {
    var availableMemo = this.FindMemoById(id);
    _context.Memos.Remove(availableMemo);
    _context.SaveChanges();
  }

}