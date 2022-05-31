using StockChat.API.Models.Csv;
using StockChat.Domain.Dto.External;

namespace StockChat.API.Services
{
    public interface IStockService
    {
        Task<StockCsv> GetStock(string stockCode, string caller);
    }
}
