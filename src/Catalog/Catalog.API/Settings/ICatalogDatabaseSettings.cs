namespace Catalog.API.Settings
{
    public interface ICatalogDatabaseSettings
    {
        string ConnectionString { get; set; } //connection
        string DatabaseName { get; set; } //datenbankname
        string CollectionName { get; set; } //tabellenname
    }
}