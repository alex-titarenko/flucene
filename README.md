# Flucene

Object Document Mapper for Lucene.Net

## Key Features
* Property mapping
    * string
    * int, double, float, decimal etc...
    * DateTime
    * enum
* Collection mapping
    * array
    * List

## Example of usage
Simple mapping class:
```C#
public class ApplicationMap : DocumentMap<Application>
{
    public ApplicationMap()
    {
        Map(x => x.ID);
        Map(x => x.Name, "AppName").Store.Yes().Index.Analyze().Optional().Boost(3);

        Map(x => x.Version.ToString(),
            (x, v) => x.Version = Version.Parse(v.FirstOrDefault()),
            "AppVersion").Store.Yes().Index.NotAnalyze().Boost(0.3f);

        Map(x => x.Title.ToUpperInvariant(), "Title");

        Map(x => x.AdditionalFields
            .Select(p => new KeyValuePair<string, object>(p.Key, p.Value)))
            .Boost(2).Store.Yes().Index.Analyze();

        Map(x => x.Description, "AppDescription").Store.Yes().Index.Analyze().Boost(0.1f);
        Map(x => x.RegularPrice, "RegularPrice").Store.Yes().Index.No();
        Map(x => x.UpgradePrice, "UpgradePrice").Store.Yes().Index.No();
        Map(x => x.ReleaseDate, "ReleaseDate").Store.Yes().Index.No();
        Map(x => x.Status, "Status").Store.Yes().Index.No();
        Map(x => x.Tags, "tag").Store.Yes().Index.Analyze().Boost(3);

        Embedded(x => x.Category).Prefix("Category");

        Boost(x => x.Name.Length);
    }
}
```
Embeded mapping class:
```C#
public class CategoryMap : DocumentMap<Category>
{
    public CategoryMap()
    {
        Map(x => x.ID);
        Map(x => x.Name, "ShopName").Store.Yes().Index.Analyze().Boost(1.5f);
        Map(x => x.IsRoot).Store.Yes().Index.No();
    }
}
```
Usage:
```C#
// Create a model
Application expected = new Application();
expected.ID = 2;
expected.Name = "Flucene";
expected.Description =
@"Flucene - ORM for lucene.net. This project based on the opened sources of Iveonik Systems ltd.
    This library provide more fine mapping as compared with other existed libraries. Library's name
    was created as acronym of ""FLUent luCENE"".";
expected.Category = new Category() { IsRoot = true, Name = "Security" };
expected.Version = new Version(1, 0, 0, 0);
expected.ReleaseDate = DateTime.Today;
expected.RegularPrice = 0.00m;
expected.UpgradePrice = 59.95m;
expected.Status = PublishStatus.Active;
expected.Tags = new List<string>() { "lucene", "search", "ORM", "mapping" };
expected.AdditionalFields = new List<KeyValuePair<string, string>>()
{
    new KeyValuePair<string, string>("Publisher", "Iveonik Systems Ltd."),
    new KeyValuePair<string, string>("License", "Apache License 2.0"),
    new KeyValuePair<string, string>("Language", "English")
};

// Initialize mapping service
IMappingsService mappingService = new FluentMappingsService(Assembly.GetExecutingAssembly());
((FluentMappingsService)mappingService).Mapper = new ReflectionDocumentMapper();

// Get document by model
Document actualDoc = mappingService.GetDocument(expected);
// Get model by document
Application actual = mappingService.GetModel<Application>(actualDoc);
```


## Get it on NuGet!

    Install-Package Flucene

## License
Flucene is under the [Apache License 2.0](LICENSE.md).
