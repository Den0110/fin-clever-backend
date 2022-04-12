using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinClever.Models;
using FinClever.Models.invest;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FinClever.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly FinCleverDbContext context;

        public PortfolioRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<string>> GetAllTickets()
        {
            return await context.InvestOperations
                .GroupBy(o => o.Ticker)
                .Select(x => x.Key)
                .ToListAsync();
        }

        public async Task<IEnumerable<PortfolioStock>> GetStocks(string userId, long? date = null)
        {
            if(date == null)
            {
                date = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            }

            var stocks = await RawSqlQuery("exec portfolioStocks @userId,@date", x =>
            {
                string ticker = x[0].ToString() ?? "";
                double usdPurchasePrice;
                double purchasePrice;
                int amount;
                if (double.TryParse(x[1].ToString(), out usdPurchasePrice) &&
                    double.TryParse(x[2].ToString() ?? "0", out purchasePrice) &&
                    int.TryParse(x[3].ToString() ?? "0", out amount)
                )
                {
                    return new PortfolioStock(ticker, usdPurchasePrice, purchasePrice, amount);
                }
                return null;
            }, new SqlParameter("@userId", userId), new SqlParameter("@date", date));
            return stocks.Where(x => x != null) as IEnumerable<PortfolioStock>;
        }

        public async Task<IEnumerable<PriceItem>> GetPortfolioHistory(string userId, IEnumerable<long> dates, bool showHistoricalProfit)
        {
            var datesStr = string.Join(',', dates);
            var showHistoricalProfitValue = 0;
            if (showHistoricalProfit)
            {
                showHistoricalProfitValue = 1;
            }
            Console.WriteLine($"@userId={userId}");
            Console.WriteLine($"@datesStr={datesStr}");
            Console.WriteLine($"@showHistoricalProfit={showHistoricalProfitValue}");
            return await context.PortfolioHistoryCache
                .Where(x => dates.Contains(x.Date) &&
                    x.UserId == userId &&
                    x.ShowHistoricalProfit == showHistoricalProfitValue
                )
                .Select(x => new PriceItem(x.Date, x.Price))
                .ToListAsync();
        }

        public async Task UpdatePortfoliosCache(IEnumerable<long> dates, string range)
        {
            var datesStr = string.Join(',', dates);
            await RawSqlQuery(
                "exec portfolioHistoryCaching @dates,@range",
                new SqlParameter("@dates", datesStr),
                new SqlParameter("@range", range)
            );
        }

        private async Task RawSqlQuery(string query, params SqlParameter[] parameters)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 120;

                context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    await result.ReadAsync();
                }
            }
        }

        private async Task<List<T>> RawSqlQuery<T>(string query, Func<DbDataReader, T> map, params SqlParameter[] parameters)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 120;

                context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (await result.ReadAsync())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }
        }
    }
}
