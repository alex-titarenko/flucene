using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Test.Models;


namespace Lucene.Net.Odm.Test.Mappings
{
    public class ApplicationMap : DocumentMap<Application>
    {
        public ApplicationMap()
        {
            Map(x => x.ID);
            Map(x => x.Name, "AppName").Store.Yes().Index.Analyze().Optional().Boost(3);
            
            Map(
                x => x.Version.ToString(),
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
}
