using Dapper;
using Dymitro.Contracts;
using Dymitro.DAL.Dapper.Context;
using Dymitro.Models.Domain;
using Dymitro.Models.DTOs;
using System.Globalization;

namespace Dymitro.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly DapperContext _context;

        public SalaryService(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertSalaryAsync(SalaryDto salary)
        {
            const string sql = @"
                INSERT INTO salary (date, company, taxes, net, gross, course, net_in_euro)
                VALUES (@Date, @Company, @Taxes, @Net, @Gross, @Course, @NetInEuro)";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                salary.Date,
                salary.Company,
                salary.Taxes,
                salary.Net,
                salary.Gross,
                salary.Course,
                salary.NetInEuro
            });

            return rows > 0;
        }

        public async Task<IEnumerable<SalaryListDto>> GetSalariesAsync()
        {
            const string sql = "SELECT id, date, company, taxes, net, gross, course, net_in_euro AS NetInEuro FROM public.salary ORDER BY date DESC";

            using var connection = _context.CreateConnection();
            var dbResult = await connection.QueryAsync<Salary>(sql);

            var salaryDtos = dbResult.Select(s => new SalaryDto
            {
                Company = s.Company,
                Date = s.Date,
                Taxes = s.Taxes,
                Net = s.Net,
                Gross = s.Gross,
                Course = s.Course,
                NetInEuro = s.NetInEuro,
                MonthYear = CalculateMonthOfSalary(s.Date)
            }).ToList();

            var statsByYear = CalculateSalaryByYears(salaryDtos);

            var result = new SalaryListDto
            {
                salariesviewmodels = salaryDtos,
                statsByYear = statsByYear,
                TotalRSDNet = salaryDtos.Sum(x => x.Net) ?? 0,
                TotalEurNet = salaryDtos.Sum(x => x.NetInEuro) ?? 0
            };

            return new List<SalaryListDto> { result };
        }

        private static string CalculateMonthOfSalary(DateTime date)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            if ((daysInMonth - date.Day) <= 7)
                return $"{date.ToString("MMMM", CultureInfo.InvariantCulture)} '{date.Year}";

            if (date.Day <= 7)
                return $"{date.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture)} '{date.Year}";

            return $"{date.ToString("MMMM", CultureInfo.InvariantCulture)} '{date.Year}";
        }

        private static IEnumerable<SalaryListDto.StatsByYear> CalculateSalaryByYears(IEnumerable<SalaryDto> salaries)
        {
            return salaries
                .GroupBy(x => x.Date.Year)
                .Select(g => new SalaryListDto.StatsByYear
                {
                    Year = g.Key.ToString(),
                    TotalSalary = g.Sum(x => x.NetInEuro)
                });
        }
    }
}
