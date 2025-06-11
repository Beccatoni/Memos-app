using MemosApplication.Api.DTOs;
using MemosApplication.Api.Models;
using MemosApplication.Api.Repository;

namespace MemosApplication.Api.Services;

public interface IMemoService
{
    IEnumerable<MemoDto> GetAllMemos();
    // MemoDto? GetMemoById(int id);
    void AddMemo(CreateMemoDto memo);
    void UpdateMemo(UpdateMemoDto memo, int id);
    void DeleteMemo(int id);
    // void SaveChages();
}
public class MemoService : IMemoService
{
    private readonly IMemoRepository _repository;
    
    public MemoService(IMemoRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<MemoDto> GetAllMemos()
    {
        var memos = _repository.GetAllMemos();
        return memos.Select(memo => new MemoDto
        {
            Id = memo.Id,
            Title = memo.Title,
            Content = memo.Content,
            CreatedAt = memo.CreatedAt,
            UpdatedAt = memo.UpdatedAt
        }).ToList();
    }

    public void AddMemo(CreateMemoDto newMemo)
    {
        var memo = new Memo
        {
            Title = newMemo.Title,
            Content = newMemo.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _repository.AddMemo(memo);
    }

    public void UpdateMemo(UpdateMemoDto memo, int id)
    {
        
        try
        {
            if (memo == null)
            {
                throw new ArgumentNullException(nameof(memo), "The memo update data cannot be null.");
            }
            _repository.UpdateMemo(memo, id);
        }
        catch (Exception e)
        {
            throw new KeyNotFoundException($"Failed to update memo with ID {id}: {e.Message}", e);
        }
    }

    public void DeleteMemo(int id)
    {
        var memos = _repository.GetAllMemos();
        if (!memos.Any(m => m.Id == id))
        {
            throw new KeyNotFoundException("No memo found with the given ID.");
        }
        
        _repository.DeleteMemo(id);
    }
}