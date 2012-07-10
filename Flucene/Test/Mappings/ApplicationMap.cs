using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Orm.Mapping;
using Lucene.Net.Orm.Test.Models;


namespace Lucene.Net.Orm.Test.Mappings
{
    public class ApplicationMap : DocumentMap<Application>
    {
        public ApplicationMap()
        {
            Map(x => x.ID);
            Map(x => x.Name, "AppName").Store().Analyze().Boost(2);

            CustomMap(
                x => x.Version.ToString(),
                (x, v) => x.Version = Version.Parse(v.FirstOrDefault()),
                "AppVersion").Store().NotAnalyze().Boost(0.3f);

            CustomField(x => x.Title.ToUpperInvariant(), "Title");

            Map(x => x.Description, "AppDescription").Store().Analyze().Boost(0.1f);
            Map(x => x.RegularPrice, "RegularPrice").Store().NotIndex();
            Map(x => x.UpgradePrice, "UpgradePrice").Store().NotIndex();
            Map(x => x.ReleaseDate, "ReleaseDate").Store().NotIndex();
            Map(x => x.Status, "Status").Store().NotIndex();
            Map(x => x.Tags, "tag").Store().Analyze().Boost(3);

            Reference(x => x.Category).Prefix("Category");
        }
    }
}
