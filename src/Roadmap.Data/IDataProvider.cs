using Microsoft.EntityFrameworkCore;
using Roadmap.Models.Db;

namespace Roadmap.Data;

public interface IDataProvider : IBaseDataProvider
{
    DbSet<DbUser> Users { get; set; }
}