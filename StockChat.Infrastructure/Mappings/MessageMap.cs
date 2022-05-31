using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockChat.Domain.Models;

namespace StockChat.Infrastructure.Mappings
{
    public class MessageMap : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CreationDate)
               .IsRequired();

            builder.Property(b => b.Owner)
                .IsRequired();

            builder.Property(b => b.Content)
                .IsRequired()
                .HasMaxLength(800);
        }
    }
}
