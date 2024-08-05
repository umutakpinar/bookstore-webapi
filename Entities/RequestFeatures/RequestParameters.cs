using System.ComponentModel.DataAnnotations;

namespace Entities.RequestFeatures;

public abstract class RequestParameters
{
    private const int _maxPageSize = 50;

    private int _pageSize = 15;

    public int PageNumber { get; set; } = 1;
    
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value > _maxPageSize ? _maxPageSize : value;
        }
    }

    public string? OrderBy { get; set; }
    public String? Fields { get; set; }
}