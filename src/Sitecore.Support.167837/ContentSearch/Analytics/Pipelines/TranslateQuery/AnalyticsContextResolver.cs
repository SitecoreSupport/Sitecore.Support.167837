namespace Sitecore.Support.ContentSearch.Analytics.Pipelines.TranslateQuery
{
    using Sitecore.Analytics;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using Sitecore.ContentSearch.Pipelines.TranslateQuery;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Xdb.Configuration;
    public class AnalyticsContextResolver : Sitecore.ContentSearch.Analytics.Pipelines.TranslateQuery.AnalyticsContextResolver
    {
        protected override IEnumerable<SearchStringModel> Search(TranslateQueryArgs args)
        {
            if (XdbSettings.Tracking.Enabled)
            {
                for (int i = 0; i < args.SearchStringModel.Count<SearchStringModel>(); i++)
                {
                    if (Tracker.Current != null && Tracker.Current.Interaction != null && Tracker.Current.Interaction.LocationReference != null)
                    {
                        args.SearchStringModel.ElementAt(i).Value = args.SearchStringModel.ElementAt(i).Value.Replace("$CurrentLocation", Tracker.Current.Interaction.LocationReference.Country);
                        args.SearchStringModel.ElementAt(i).Value = args.SearchStringModel.ElementAt(i).Value.Replace("$ExternalUser", Tracker.Current.Interaction.LocationReference.Identifier);
                        args.SearchStringModel.ElementAt(i).Value = args.SearchStringModel.ElementAt(i).Value.Replace("$AreaCode", Tracker.Current.Interaction.GeoData.AreaCode);
                        args.SearchStringModel.ElementAt(i).Value = args.SearchStringModel.ElementAt(i).Value.Replace("$BrowserName", Tracker.Current.Interaction.BrowserInfo.BrowserMajorName);
                        args.SearchStringModel.ElementAt(i).Value = args.SearchStringModel.ElementAt(i).Value.Replace("$CurrentCity", Tracker.Current.Interaction.GeoData.City);
                        args.SearchStringModel.ElementAt(i).Value = args.SearchStringModel.ElementAt(i).Value.Replace("$DeviceId", Tracker.Current.Interaction.DeviceId.ToString());
                        if (Tracker.Current.Interaction.Ip != null)
                        {
                            args.SearchStringModel.ElementAt(i).Value = args.SearchStringModel.ElementAt(i).Value.Replace("$GeoIP", Encoding.Default.GetString(Tracker.Current.Interaction.Ip));
                        }
                        else
                        {
                            Diagnostics.Log.Warn(
                                "Cannot resolve context GeoIP for current interaction " +
                                Tracker.Current.Interaction.InteractionId, this);
                        }
                        args.SearchStringModel.ElementAt(i).Value = args.SearchStringModel.ElementAt(i).Value.Replace("$UserAgentID", Tracker.Current.Interaction.UserAgent);
                    }
                }
            }
            return args.SearchStringModel;
        }
    }
}