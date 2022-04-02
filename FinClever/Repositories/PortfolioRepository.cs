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
            var prices = await RawSqlQuery("exec portfolioHistory @userId,@dates,@showHistoricalProfit", x =>
            {
                long date;
                double price;
                if(long.TryParse(x[0].ToString(), out date) && double.TryParse(x[1].ToString() ?? "0", out price))
                {
                    return new PriceItem(date, price);
                }
                return null;
            }, new SqlParameter("@userId", userId),
            new SqlParameter("@dates", datesStr),
            new SqlParameter("@showHistoricalProfit", showHistoricalProfitValue));
            return prices.Where(x => x != null) as IEnumerable<PriceItem>;
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
