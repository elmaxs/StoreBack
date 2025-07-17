using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Configurtions
{
    public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ProductReviewEntity> builder)
        {
            builder.HasKey(pr => pr.Id);

            builder.HasOne(pr => pr.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(pr => pr.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
