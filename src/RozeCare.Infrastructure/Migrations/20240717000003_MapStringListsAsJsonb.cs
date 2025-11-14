using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RozeCare.Infrastructure.Migrations
{
    public partial class MapStringListsAsJsonb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE \"PatientProfiles\"
SET \"Conditions\" = CASE
        WHEN \"Conditions\" IS NULL OR btrim(\"Conditions\") = '' THEN '[]'
        WHEN left(btrim(\"Conditions\"), 1) = '[' THEN \"Conditions\"
        ELSE to_json(string_to_array(\"Conditions\", ';'))::text
    END,
    \"Allergies\" = CASE
        WHEN \"Allergies\" IS NULL OR btrim(\"Allergies\") = '' THEN '[]'
        WHEN left(btrim(\"Allergies\"), 1) = '[' THEN \"Allergies\"
        ELSE to_json(string_to_array(\"Allergies\", ';'))::text
    END,
    \"PreferredProviders\" = CASE
        WHEN \"PreferredProviders\" IS NULL OR btrim(\"PreferredProviders\") = '' THEN '[]'
        WHEN left(btrim(\"PreferredProviders\"), 1) = '[' THEN \"PreferredProviders\"
        ELSE to_json(string_to_array(\"PreferredProviders\", ';'))::text
    END,
    \"EmergencyContacts\" = CASE
        WHEN \"EmergencyContacts\" IS NULL OR btrim(\"EmergencyContacts\") = '' THEN '[]'
        WHEN left(btrim(\"EmergencyContacts\"), 1) = '[' THEN \"EmergencyContacts\"
        ELSE to_json(string_to_array(\"EmergencyContacts\", ';'))::text
    END;
");

            migrationBuilder.Sql(@"
UPDATE \"Encounters\"
SET \"Diagnoses\" = CASE
        WHEN \"Diagnoses\" IS NULL OR btrim(\"Diagnoses\") = '' THEN '[]'
        WHEN left(btrim(\"Diagnoses\"), 1) = '[' THEN \"Diagnoses\"
        ELSE to_json(string_to_array(\"Diagnoses\", ';'))::text
    END,
    \"Prescriptions\" = CASE
        WHEN \"Prescriptions\" IS NULL OR btrim(\"Prescriptions\") = '' THEN '[]'
        WHEN left(btrim(\"Prescriptions\"), 1) = '[' THEN \"Prescriptions\"
        ELSE to_json(string_to_array(\"Prescriptions\", ';'))::text
    END;
");

            migrationBuilder.Sql(@"
UPDATE \"Documents\"
SET \"Tags\" = CASE
        WHEN \"Tags\" IS NULL OR btrim(\"Tags\") = '' THEN '[]'
        WHEN left(btrim(\"Tags\"), 1) = '[' THEN \"Tags\"
        ELSE to_json(string_to_array(\"Tags\", ';'))::text
    END;
");

            migrationBuilder.Sql(@"
UPDATE \"Consents\"
SET \"Scopes\" = CASE
        WHEN \"Scopes\" IS NULL OR btrim(\"Scopes\") = '' THEN '[]'
        WHEN left(btrim(\"Scopes\"), 1) = '[' THEN \"Scopes\"
        ELSE to_json(string_to_array(\"Scopes\", ';'))::text
    END;
");

            migrationBuilder.AlterColumn<string>(
                name: "Conditions",
                table: "PatientProfiles",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Allergies",
                table: "PatientProfiles",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PreferredProviders",
                table: "PatientProfiles",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContacts",
                table: "PatientProfiles",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Diagnoses",
                table: "Encounters",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Prescriptions",
                table: "Encounters",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "Documents",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Scopes",
                table: "Consents",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE \"PatientProfiles\"
SET \"Conditions\" = CASE
        WHEN \"Conditions\" IS NULL THEN ''
        WHEN jsonb_typeof(\"Conditions\") = 'array' THEN COALESCE(array_to_string(ARRAY(SELECT jsonb_array_elements_text(\"Conditions\")), ';'), '')
        ELSE COALESCE(\"Conditions\"::text, '')
    END,
    \"Allergies\" = CASE
        WHEN \"Allergies\" IS NULL THEN ''
        WHEN jsonb_typeof(\"Allergies\") = 'array' THEN COALESCE(array_to_string(ARRAY(SELECT jsonb_array_elements_text(\"Allergies\")), ';'), '')
        ELSE COALESCE(\"Allergies\"::text, '')
    END,
    \"PreferredProviders\" = CASE
        WHEN \"PreferredProviders\" IS NULL THEN ''
        WHEN jsonb_typeof(\"PreferredProviders\") = 'array' THEN COALESCE(array_to_string(ARRAY(SELECT jsonb_array_elements_text(\"PreferredProviders\")), ';'), '')
        ELSE COALESCE(\"PreferredProviders\"::text, '')
    END,
    \"EmergencyContacts\" = CASE
        WHEN \"EmergencyContacts\" IS NULL THEN ''
        WHEN jsonb_typeof(\"EmergencyContacts\") = 'array' THEN COALESCE(array_to_string(ARRAY(SELECT jsonb_array_elements_text(\"EmergencyContacts\")), ';'), '')
        ELSE COALESCE(\"EmergencyContacts\"::text, '')
    END;
");

            migrationBuilder.Sql(@"
UPDATE \"Encounters\"
SET \"Diagnoses\" = CASE
        WHEN \"Diagnoses\" IS NULL THEN ''
        WHEN jsonb_typeof(\"Diagnoses\") = 'array' THEN COALESCE(array_to_string(ARRAY(SELECT jsonb_array_elements_text(\"Diagnoses\")), ';'), '')
        ELSE COALESCE(\"Diagnoses\"::text, '')
    END,
    \"Prescriptions\" = CASE
        WHEN \"Prescriptions\" IS NULL THEN ''
        WHEN jsonb_typeof(\"Prescriptions\") = 'array' THEN COALESCE(array_to_string(ARRAY(SELECT jsonb_array_elements_text(\"Prescriptions\")), ';'), '')
        ELSE COALESCE(\"Prescriptions\"::text, '')
    END;
");

            migrationBuilder.Sql(@"
UPDATE \"Documents\"
SET \"Tags\" = CASE
        WHEN \"Tags\" IS NULL THEN ''
        WHEN jsonb_typeof(\"Tags\") = 'array' THEN COALESCE(array_to_string(ARRAY(SELECT jsonb_array_elements_text(\"Tags\")), ';'), '')
        ELSE COALESCE(\"Tags\"::text, '')
    END;
");

            migrationBuilder.Sql(@"
UPDATE \"Consents\"
SET \"Scopes\" = CASE
        WHEN \"Scopes\" IS NULL THEN ''
        WHEN jsonb_typeof(\"Scopes\") = 'array' THEN COALESCE(array_to_string(ARRAY(SELECT jsonb_array_elements_text(\"Scopes\")), ';'), '')
        ELSE COALESCE(\"Scopes\"::text, '')
    END;
");

            migrationBuilder.AlterColumn<string>(
                name: "Conditions",
                table: "PatientProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Allergies",
                table: "PatientProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "PreferredProviders",
                table: "PatientProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContacts",
                table: "PatientProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Diagnoses",
                table: "Encounters",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Prescriptions",
                table: "Encounters",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "Documents",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Scopes",
                table: "Consents",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");
        }
    }
}
