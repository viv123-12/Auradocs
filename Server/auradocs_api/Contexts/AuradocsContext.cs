namespace auradocs_api.Contexts;

using auradocs_api.Data;
using auradocs_api.Models;
using Microsoft.EntityFrameworkCore;

public class AuradocsContext : DbContext
{
    public AuradocsContext(DbContextOptions<AuradocsContext> options) :base(options)
    {}

    required public virtual DbSet<User> Users { get; set; }
    required public virtual DbSet<DropdownOptions> RegisterPageDomainNames { get; set; }
    required public virtual DbSet<DropdownOptionsGroup> RegisterPageDomainPracticeAreas { get; set; }
    required public virtual DbSet<ResetPasswordToken> ResetPasswordTokens{ get; set; }
    required public virtual DbSet<Document> Documents { get; set; }
    required public virtual DbSet<Folder> Folders { get; set; }
    required public virtual DbSet<DocumentFolder> DocumentFolders { get; set; }
    required public virtual DbSet<DocumentVersion> DocumentVersion { get; set; }
    required public virtual DbSet<DocumentSharedWithUser> DocumentsSharedWithUsers { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder){

        modelBuilder.Entity<DropdownOptions>(entity =>
        {
            entity.ToTable("domainnamedropdown");
            entity.HasKey(e => e.uId);
            entity.Property(e => e.strOption).HasColumnName("domainName");

            entity.HasIndex(u => u.uId).IsUnique().HasDatabaseName("IX_domainname_uId");
            modelBuilder.Entity<DropdownOptions>().HasData(
            new DropdownOptions { uId = 1, strOption = "IT" },
            new DropdownOptions { uId = 2, strOption = "Legal" },
            new DropdownOptions { uId = 3, strOption = "Healthcare"},
            new DropdownOptions { uId = 4, strOption = "Finance" },
            new DropdownOptions { uId = 5, strOption = "HR" },
            new DropdownOptions { uId = 6, strOption = "Marketing" },
            new DropdownOptions { uId = 7, strOption = "Consulting" });
        });

        modelBuilder.Entity<DropdownOptionsGroup>(entity =>
        {
           entity.ToTable("domainpracticeareadropdown");
           entity.HasKey(u => u.uId); 
           entity.Property(e => e.uKey).HasColumnName("domainId");
           entity.Property(e => e.strValue).HasColumnName("practicearea");

           entity.HasMany<DropdownOptionsGroup>()
              .WithOne()
              .HasForeignKey(x => x.uKey)
              .OnDelete(DeleteBehavior.Cascade)
              .HasConstraintName("FK_Dropdownoptionsgroup_ukey");

            modelBuilder.Entity<DropdownOptionsGroup>().HasData(
            // -------- IT --------
            new DropdownOptionsGroup { uId = 1, uKey = (int)DomainKeys.IT, strValue = "Web Development" },
            new DropdownOptionsGroup { uId = 2, uKey = (int)DomainKeys.IT ,strValue = "Mobile Development"},
            new DropdownOptionsGroup { uId = 3, uKey = (int)DomainKeys.IT, strValue = "Cloud Engineering"},
            new DropdownOptionsGroup { uId = 4, uKey = (int)DomainKeys.IT, strValue =  "DevOps"},
            new DropdownOptionsGroup { uId = 5, uKey = (int)DomainKeys.IT, strValue = "Data Engineering" },
            new DropdownOptionsGroup { uId = 6, uKey = (int)DomainKeys.IT, strValue = "Cybersecurity" },
            new DropdownOptionsGroup { uId = 7, uKey = (int)DomainKeys.IT, strValue =  "AI / Machine Learning" },
            new DropdownOptionsGroup { uId = 8, uKey = (int)DomainKeys.IT, strValue =  "QA / Testing" },
            new DropdownOptionsGroup { uId = 9, uKey = (int)DomainKeys.IT, strValue =  "QA / Testing" },
            // -------- Legal --------
            new DropdownOptionsGroup { uId = 10, uKey = (int)DomainKeys.LEGAL, strValue = "Corporate Law" },
            new DropdownOptionsGroup { uId = 11, uKey = (int)DomainKeys.LEGAL, strValue = "Criminal Defense" },
            new DropdownOptionsGroup { uId = 12, uKey = (int)DomainKeys.LEGAL, strValue = "Civil Litigation" },
            new DropdownOptionsGroup { uId = 13, uKey = (int)DomainKeys.LEGAL, strValue = "Family Law" },
            new DropdownOptionsGroup { uId = 14, uKey = (int)DomainKeys.LEGAL, strValue = "Real Estate Law" },
            new DropdownOptionsGroup { uId = 15, uKey = (int)DomainKeys.LEGAL, strValue = "Intellectual Property (IP)" },
            new DropdownOptionsGroup { uId = 16, uKey = (int)DomainKeys.LEGAL, strValue = "Employment & Labor Law" },
            new DropdownOptionsGroup { uId = 17, uKey = (int)DomainKeys.LEGAL, strValue = "Tax Law" },

            // -------- Healthcare --------
            new DropdownOptionsGroup { uId = 18, uKey = (int)DomainKeys.HEALTHCARE, strValue = "Cardiology" },
            new DropdownOptionsGroup { uId = 19, uKey = (int)DomainKeys.HEALTHCARE, strValue = "Neurology" },
            new DropdownOptionsGroup { uId = 20, uKey = (int)DomainKeys.HEALTHCARE, strValue = "Orthopedics" },
            new DropdownOptionsGroup { uId = 21, uKey = (int)DomainKeys.HEALTHCARE, strValue = "Pediatrics" },
            new DropdownOptionsGroup { uId = 22, uKey = (int)DomainKeys.HEALTHCARE, strValue = "Oncology" },
            new DropdownOptionsGroup { uId = 23, uKey = (int)DomainKeys.HEALTHCARE, strValue = "General Medicine" },

            // -------- Finance --------
            new DropdownOptionsGroup { uId = 24, uKey = (int)DomainKeys.FINANCE, strValue = "Tax Advisory" },
            new DropdownOptionsGroup { uId = 25, uKey = (int)DomainKeys.FINANCE, strValue = "Audit" },
            new DropdownOptionsGroup { uId = 26, uKey = (int)DomainKeys.FINANCE, strValue = "Investment Management" },
            new DropdownOptionsGroup { uId = 27, uKey = (int)DomainKeys.FINANCE, strValue = "Risk Management" },
            new DropdownOptionsGroup { uId = 28, uKey = (int)DomainKeys.FINANCE, strValue = "Wealth Management" },

            // -------- HR --------
            new DropdownOptionsGroup { uId = 29, uKey = (int)DomainKeys.HR, strValue = "Recruitment" },
            new DropdownOptionsGroup { uId = 30, uKey = (int)DomainKeys.HR, strValue = "Learning & Development" },
            new DropdownOptionsGroup { uId = 31, uKey = (int)DomainKeys.HR, strValue = "Employee Relations" },
            new DropdownOptionsGroup { uId = 32, uKey = (int)DomainKeys.HR, strValue = "Compensation & Benefits" },

            // -------- Marketing --------
            new DropdownOptionsGroup { uId = 33, uKey = (int)DomainKeys.MARKETING, strValue = "Digital Marketing" },
            new DropdownOptionsGroup { uId = 34, uKey = (int)DomainKeys.MARKETING, strValue = "Content Marketing" },
            new DropdownOptionsGroup { uId = 35, uKey = (int)DomainKeys.MARKETING, strValue = "Branding" },
            new DropdownOptionsGroup { uId = 36, uKey = (int)DomainKeys.MARKETING, strValue = "SEO" },
            new DropdownOptionsGroup { uId = 37, uKey = (int)DomainKeys.MARKETING, strValue = "Market Research" },

            // -------- Consulting --------
            new DropdownOptionsGroup { uId = 38, uKey = (int)DomainKeys.CONSULTING, strValue = "Business Strategy" },
            new DropdownOptionsGroup { uId = 39, uKey = (int)DomainKeys.CONSULTING, strValue = "Operations" },
            new DropdownOptionsGroup { uId = 40, uKey = (int)DomainKeys.CONSULTING, strValue = "IT Consulting" },
            new DropdownOptionsGroup { uId = 41, uKey = (int)DomainKeys.CONSULTING, strValue = "Supply Chain" },
            new DropdownOptionsGroup { uId = 42, uKey = (int)DomainKeys.CONSULTING, strValue = "Corporate Finance" }
            );
        });
        

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.uUid);
            entity.Property(u => u.strGuid).HasColumnName("Guid").HasMaxLength(100).IsRequired();
            entity.Property(u => u.strUserId).HasColumnName("Email").HasMaxLength(254).IsRequired();
            entity.Property(u => u.strPassword).HasColumnName("Password");
            entity.Property(u => u.strPasswordSalt).HasColumnName("PasswordSalt");
            entity.Property(u => u.strUserRole).HasColumnName("UserRole").IsRequired();
            entity.Property(u => u.strphoneNumber).HasColumnName("PhoneNumber").HasMaxLength(25);
            entity.Property(u => u.strAccountType).HasColumnName("AccountType");
            entity.Property(u => u.uDomainType).HasColumnName("DomainType");
            entity.Property(u => u.uPracticeArea).HasColumnName("PracticeArea");
            entity.Property(u => u.strFullName).HasColumnName("FullName").HasMaxLength(254).HasDefaultValue(null);
            entity.Property(u => u.strProfilePictureURL).HasColumnName("ProfilePictureUrl").HasDefaultValue(null);
            entity.Property(u => u.strAddress).HasColumnName("Address").HasMaxLength(500).HasDefaultValue(null);
            entity.Property(u => u.strDescription).HasColumnName("Description").HasMaxLength(256).HasDefaultValue(null);
            entity.Property(u => u.strOrganizationName).HasColumnName("strOrganizationName").HasMaxLength(256).HasDefaultValue(null);
            entity.Property(u => u.strJobTitle).HasColumnName("strJobTitle").HasMaxLength(256).HasDefaultValue(null);
            entity.Property(u => u.boolIsUserActivated).HasColumnName("IsUserActivated").HasDefaultValue(1);
            entity.Property(u => u.dtLastLogin).HasColumnName("dtLastLogin");
            entity.Property(u => u.dtAdded).HasColumnName("dtAdded");
            
            entity.HasOne<DropdownOptions>().WithMany().HasForeignKey(e => e.uDomainType).HasConstraintName("FK_Users_uDomainType");
            entity.HasOne<DropdownOptionsGroup>().WithMany().HasForeignKey(e => e.uPracticeArea).HasConstraintName("FK_Users_uPracticeArea");
            entity.HasIndex(u => u.strUserId).IsUnique().HasDatabaseName("IX_Users_Email");
            entity.HasIndex(u => u.strphoneNumber).IsUnique().HasDatabaseName("IX_Users_PhoneNumber");
        });

        modelBuilder.Entity<ResetPasswordToken>(entity =>
        {
            entity.ToTable("ResetPasswordToken");
            entity.HasKey(e => e.uId);
            entity.Property(e => e.strGuid).HasColumnName("strGuid");
            entity.Property(e => e.strUserId).HasColumnName("strUserId");
            entity.Property(e => e.strToken).HasColumnName("strToken");
            entity.Property(e => e.boolIsUsed).HasColumnName("boolIsUsed");
            entity.Property(e => e.boolIsVerified).HasColumnName("boolIsVerified");
            entity.Property(e => e.dtExpiresAt).HasColumnName("dtExpiredAt");
            entity.Property(e => e.dtCreatedAt).HasColumnName("dtCreatedAt");

            entity.HasIndex(u => u.strUserId).HasDatabaseName("IX_ResetPasswordToken_strUserId");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Document");
            entity.HasKey(e => e.uId);
            entity.Property(e => e.strGuid).HasColumnName("strGuid");
            entity.Property(e => e.strTitle).HasColumnName("strTitle");
            entity.Property(e => e.strContent).HasColumnName("strContent");
            entity.Property(e => e.uStatusId).HasColumnName("uStatusId");
            entity.Property(e => e.uCurrentVersionId).HasColumnName("uCurrentVersionId");
            entity.Property(e => e.uCreatedBy).HasColumnName("uCreatedBy");
            entity.Property(e => e.uOwnerUserId).HasColumnName("uOwnerUserId");
            entity.Property(e => e.boolIsDeleted).HasColumnName("boolIsDeleted");
            entity.Property(e => e.dtUpdatedOn).HasColumnName("dtUpdatedOn");
            entity.Property(e => e.dtCreatedOn).HasColumnName("dtCreatedOn");

            entity.HasOne<User>().WithMany().HasForeignKey(e => e.uOwnerUserId).HasConstraintName("FK_Folder_uOwnerUserId");
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.uCreatedBy).HasConstraintName("FK_Folder_uCreatedBy");

            entity.HasIndex(u => u.uStatusId).HasDatabaseName("IX_Document_uStatusId");
            entity.HasIndex(u => u.uStatusId).HasDatabaseName("IX_Document_boolIsDeleted");
        });

        modelBuilder.Entity<Folder>(entity =>
        {
            entity.ToTable("Folder");
            entity.HasKey(e => e.uId);
            entity.Property(e => e.strGuid).HasColumnName("strGuid");
            entity.Property(e => e.strTitle).HasColumnName("strTitle");
            entity.Property(e => e.uParentFolderId).HasColumnName("uParentFolderId");
            entity.Property(e => e.uOwnerUserId).HasColumnName("uOwnerUserId");
            entity.Property(e => e.boolIsDeleted).HasColumnName("boolIsDeleted");
            entity.Property(e => e.uCreatedBy).HasColumnName("uCreatedBy");
            entity.Property(e => e.dtCreatedOn).HasColumnName("dtCreatedOn");

            entity.HasOne<User>().WithMany().HasForeignKey(e => e.uOwnerUserId).HasConstraintName("FK_Folder_uOwnerUserId");
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.uCreatedBy).HasConstraintName("FK_Folder_uCreatedBy");

            entity.HasIndex(i => i.uOwnerUserId).HasDatabaseName("IX_Folder_uOwnerUSerId");
            entity.HasIndex(i => i.uParentFolderId).HasDatabaseName("IX_Folder_uParentFolderId");
        });

        modelBuilder.Entity<DocumentVersion>(entity =>
        {
            entity.ToTable("DocumentVersion");
            entity.HasKey(e => e.uId);
            entity.Property(e => e.strGuid).HasColumnName("strGuid");
            entity.Property(e => e.uVersion).HasColumnName("uVersion");
            entity.Property(e => e.uDocumentId).HasColumnName("uDocumentId");
            entity.Property(e => e.strContent).HasColumnName("strContent");
            entity.Property(e => e.uUpdatedBy).HasColumnName("uUpdatedBy");
            entity.Property(e => e.dtUpdatedOn).HasColumnName("dtUpdatedOn");

            entity.HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.uUpdatedBy).
            OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_DocumentVersion_uUpdatedBy");

            entity.HasOne<Document>()
            .WithMany()
            .HasForeignKey(e => e.uDocumentId).
            OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_DocumentVersion_uDocumentId");

            entity.HasIndex(i => i.uDocumentId).HasDatabaseName("IX_Folder_uDocumentId");
            entity.HasIndex(i => i.uUpdatedBy).HasDatabaseName("IX_Folder_uUpdatedBy");
        });
  
        modelBuilder.Entity<DocumentFolder>(entity =>
        {
            entity.ToTable("DocumentFolder");
            entity.HasKey(e => e.uId);
            entity.Property(e => e.strGuid).HasColumnName("strGuid");
            entity.Property(e => e.uDocumentId).HasColumnName("uDocumentId");
            entity.Property(e => e.uFolderId).HasColumnName("uFolderId");
            entity.Property(e => e.dtCreatedAt).HasColumnName("dtCreatedAt");

            entity.HasOne<Document>().WithMany().HasForeignKey(e => e.uDocumentId).HasConstraintName("FK_DocumentFolder_uDocumentId");
            entity.HasOne<Folder>().WithMany().HasForeignKey(e => e.uFolderId).HasConstraintName("FK_DocumentFolder_uFolderId");

            entity.HasIndex(i => i.uDocumentId).HasDatabaseName("IX_DocumentFolder_uDocumentId");
            entity.HasIndex(i => i.uFolderId).HasDatabaseName("IX_DocumentFolder_uFolderId");
        });

        modelBuilder.Entity<DocumentSharedWithUser>(entity =>
        {
            entity.ToTable("DocumentsSharedWithUsers");
            entity.HasKey(e => e.uId);
            entity.Property(e => e.uSharedDocumentId).HasColumnName("uSharedDocumentId");
            entity.Property(e => e.uSharedWith).HasColumnName("uSharedWith");
            entity.Property(e => e.uSharedBy).HasColumnName("uSharedBy");
            entity.Property(e => e.uAccessgiven).HasColumnName("uAccessgiven");
            entity.Property(e => e.dtAccessGivenOn).HasColumnName("dtAccessGivenOn");

            entity.HasIndex(e => e.uSharedWith).HasDatabaseName("IX_DocumentsSharedWithUser_uSharedWith");
            entity.HasIndex(e => e.uSharedBy).HasDatabaseName("IX_DocumentsSharedWithUser_uSharedBy");

            entity.HasOne<User>().WithMany().HasForeignKey(e => e.uSharedBy).HasConstraintName("FK_DocumentsSharedWithUser_uSharedBy");
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.uSharedWith).HasConstraintName("FK_DocumentsSharedWithUser_uSharedWith");
            entity.HasOne<Document>().WithMany().HasForeignKey(e => e.uSharedDocumentId).HasConstraintName("FK_DocumentsSharedWithUser_uSharedDocumentId");
        });
      }
}