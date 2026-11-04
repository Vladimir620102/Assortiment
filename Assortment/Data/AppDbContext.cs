using Assortment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assortment.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Season> Seasons => Set<Season>();
        public DbSet<Region> Regions => Set<Region>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<State> States => Set<State>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Collection> Collections => Set<Collection>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<Model> Models => Set<Model>();
        public DbSet<ModelMaterial> ModelMaterials => Set<ModelMaterial>();
        public DbSet<Counterparty> Counterparties => Set<Counterparty>();
        public DbSet<Factory> Factories => Set<Factory>();
        public DbSet<CounterpartyFactory> CounterpartyFactories => Set<CounterpartyFactory>();
        public DbSet<FactoryImporterBeneficiary> FactoryImporterBeneficiaries => Set<FactoryImporterBeneficiary>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Season>(e =>
            {
                e.HasKey(s => s.Id);

                e.Property(s => s.Name).HasMaxLength(100).IsRequired();
                e.Property(s => s.Translate).HasMaxLength(100);
                e.Property(s => s.ExternalCode).HasMaxLength(50);
                e.Property(s => s.LabelName).HasMaxLength(100);

                // Уникальность: один тип сезона + название не может повторяться
                // (Name включает год, например "Лето 2027")
                e.HasIndex(s => new { s.Kind, s.Name }).IsUnique();

                // Внешний код уникален, если задан
                e.HasIndex(s => s.ExternalCode)
                    .IsUnique()
                    .HasFilter("[ExternalCode] IS NOT NULL");

                // Для фильтрации
                e.HasIndex(s => new { s.Kind, s.IsActive });

                // Seed-данные
                e.HasData(SeedSeasons());
            });

            b.Entity<Region>(e =>
            {
                e.HasKey(r => r.Id);
                e.Property(r => r.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(r => r.Name).IsUnique();
                e.HasData(SeedRegions());
            });

            b.Entity<Country>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).HasMaxLength(100).IsRequired();
                e.Property(c => c.IsoCode).HasMaxLength(2).IsRequired();
                e.HasIndex(c => c.IsoCode).IsUnique();
                e.HasIndex(c => new { c.RegionId, c.Name }).IsUnique();
                e.HasOne(c => c.Region).WithMany(r => r.Countries)
                    .HasForeignKey(c => c.RegionId).OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<State>(e =>
            {
                e.HasKey(s => s.Id);
                e.Property(s => s.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(s => new { s.CountryId, s.Name }).IsUnique();
                e.HasOne(s => s.Country).WithMany(c => c.States)
                    .HasForeignKey(s => s.CountryId).OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<City>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(c => new { c.StateId, c.Name }).IsUnique();
                e.HasOne(c => c.State).WithMany(s => s.Cities)
                    .HasForeignKey(c => c.StateId).OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<Brand>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();
            });

            b.Entity<Collection>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(x => new { x.BrandId, x.SeasonId, x.Name }).IsUnique();
                e.HasOne(x => x.Brand).WithMany()
                    .HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Season).WithMany()
                    .HasForeignKey(x => x.SeasonId).OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<Material>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Composition).HasMaxLength(200);
                e.HasIndex(x => x.Name).IsUnique();
            });

            b.Entity<Model>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Code).HasMaxLength(50).IsRequired();
                e.Property(x => x.Name).HasMaxLength(200).IsRequired();
                e.HasIndex(x => x.Code).IsUnique();
                e.HasIndex(x => new { x.CollectionId, x.IsActive });
                e.HasOne(x => x.Collection).WithMany()
                    .HasForeignKey(x => x.CollectionId).OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<ModelMaterial>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.ModelId, x.ComponentType, x.MaterialId }).IsUnique();
                e.HasOne(x => x.Model).WithMany(m => m.Materials)
                    .HasForeignKey(x => x.ModelId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Material).WithMany()
                    .HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Counterparty ----------
            b.Entity<Counterparty>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(200).IsRequired();
                e.Property(x => x.FullName).HasMaxLength(300);
                e.Property(x => x.Inn).HasMaxLength(50);
                e.Property(x => x.RegistrationCountry).HasMaxLength(100);
                e.Property(x => x.LegalAddress).HasMaxLength(500);
                e.Property(x => x.BankAccount).HasMaxLength(100);
                e.Property(x => x.BankName).HasMaxLength(200);
                e.Property(x => x.Swift).HasMaxLength(50);
                e.Property(x => x.ContactPerson).HasMaxLength(200);
                e.Property(x => x.Phone).HasMaxLength(100);
                e.Property(x => x.Email).HasMaxLength(200);

                e.HasIndex(x => x.Name);
                e.HasIndex(x => x.Inn).HasFilter("[Inn] IS NOT NULL");
                e.HasIndex(x => x.IsActive);
            });

            // ---------- Factory ----------
            b.Entity<Factory>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Code).HasMaxLength(50).IsRequired();
                e.Property(x => x.Name).HasMaxLength(200).IsRequired();
                e.Property(x => x.Address).HasMaxLength(500);
                e.Property(x => x.ContactPerson).HasMaxLength(200);
                e.Property(x => x.Phone).HasMaxLength(100);
                e.Property(x => x.Email).HasMaxLength(200);

                e.HasIndex(x => x.Code).IsUnique();
                e.HasIndex(x => new { x.Specialty, x.IsActive });
                e.HasIndex(x => x.CityId);

                e.HasOne(x => x.City).WithMany()
                    .HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- CounterpartyFactory ----------
            b.Entity<CounterpartyFactory>(e =>
            {
                e.HasKey(x => x.Id);

                // Один контрагент не может иметь одну роль на одной фабрике дважды
                e.HasIndex(x => new { x.CounterpartyId, x.FactoryId, x.Role }).IsUnique();
                e.HasIndex(x => new { x.FactoryId, x.Role, x.IsActive });

                e.HasOne(x => x.Counterparty).WithMany(c => c.Factories)
                    .HasForeignKey(x => x.CounterpartyId).OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Factory).WithMany(f => f.Counterparties)
                    .HasForeignKey(x => x.FactoryId).OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- FactoryImporterBeneficiary ----------
            b.Entity<FactoryImporterBeneficiary>(e =>
            {
                e.HasKey(x => x.Id);

                // Для пары (фабрика, импортёр) — только одно правило по умолчанию
                e.HasIndex(x => new { x.FactoryId, x.ImporterId })
                    .IsUnique()
                    .HasFilter("[IsDefault] = 1 AND [IsActive] = 1");

                e.HasIndex(x => new { x.BeneficiaryId, x.IsActive });

                e.HasOne(x => x.Factory).WithMany(f => f.ImporterBeneficiaryRules)
                    .HasForeignKey(x => x.FactoryId).OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Importer).WithMany(c => c.ImporterRules)
                    .HasForeignKey(x => x.ImporterId).OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Beneficiary).WithMany(c => c.BeneficiaryRules)
                    .HasForeignKey(x => x.BeneficiaryId).OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static Season[] SeedSeasons() => new[]
        {
            new Season
            {
                Id = Guid.Parse("599CC2A3-8ADB-4458-9D7E-D0ED2B593C0F"),
                Kind = SeasonKind.Demi,
                Name = "Демисезон",
                Translate = "Demi-season",
                ExternalCode = "0",
                LabelName = "ДЕМИ",
                IsActive = true,
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            },
            new Season
            {
                Id = Guid.Parse("382D486A-E64B-40BD-A8E5-04D21C1616FB"),
                Kind = SeasonKind.SpringSummer,
                Name = "Весна-Лето",
                Translate = "Spring-Summer",
                ExternalCode = "1",
                LabelName = "ВЕСНА-ЛЕТО",
                IsActive = true,
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            },
            new Season
            {
                Id = Guid.Parse("417FCEB4-59C6-4868-89FB-039031E6B781"),
                Kind = SeasonKind.AutumnWinter,
                Name = "Осень-Зима",
                Translate = "Autumn-Winter",
                ExternalCode = "2",
                LabelName = "ОСЕНЬ-ЗИМА",
                IsActive = true,
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            },
            new Season
            {
                Id = Guid.Parse("7A230551-5F5B-47B0-A36D-1BBAD8010D85"),
                Kind = SeasonKind.AllSeasons,
                Name = "Круглосезонная",
                Translate = "All seasons",
                ExternalCode = "3",
                LabelName = "КРУГЛОСЕЗОН",
                IsActive = true,
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            }
        };
        private static Region[] SeedRegions() => new[]
        {
            new Region { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Европа",
                Translate = "Europe", ExternalCode = "EU", IsActive = true,
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero) },
            new Region { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Азия",
                Translate = "Asia", ExternalCode = "AS", IsActive = true,
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero) },
            new Region { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "СНГ",
                Translate = "CIS", ExternalCode = "CIS", IsActive = true,
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero) },
            new Region { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Ближний Восток",
                Translate = "Middle East", ExternalCode = "ME", IsActive = true,
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero) }
        };
    }
}