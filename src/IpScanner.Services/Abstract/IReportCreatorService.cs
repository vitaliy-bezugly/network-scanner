using FluentResults;
using IpScanner.Models.Enums;
using IpScanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace IpScanner.Services.Abstract
{
    public interface IReportCreatorService<T>
    {
        Task<IResult<Report<T>>> CreateReportAsync(T source, ContentFormat format, CancellationToken cancellationToken);
    }
}
