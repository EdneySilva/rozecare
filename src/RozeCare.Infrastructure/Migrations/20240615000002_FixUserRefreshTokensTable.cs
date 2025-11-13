using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RozeCare.Infrastructure.Migrations
{
    public partial class FixUserRefreshTokensTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_tables
                        WHERE schemaname = current_schema()
                          AND tablename = 'RefreshTokens'
                    ) THEN
                        EXECUTE 'ALTER TABLE ""RefreshTokens"" RENAME TO ""UserRefreshTokens""';
                    END IF;
                END
                $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_tables
                        WHERE schemaname = current_schema()
                          AND tablename = 'UserRefreshTokens'
                    ) THEN
                        EXECUTE 'ALTER TABLE ""UserRefreshTokens"" RENAME TO ""RefreshTokens""';
                    END IF;
                END
                $$;
            ");
        }
    }
}
