using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Roadmap.Models.Db;

public class DbUser
{
    public const string TableName = "Users";

    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int Status { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
}

public class DbUserConfiguration : IEntityTypeConfiguration<DbUser>
{
    public void Configure(EntityTypeBuilder<DbUser> builder)
    {
        builder.
            ToTable(DbUser.TableName);

        builder.
            HasKey(p => p.Id);

        builder
            .Property(p => p.FirstName)
            .IsRequired();

        builder
            .Property(p => p.LastName)
            .IsRequired();
    }
}