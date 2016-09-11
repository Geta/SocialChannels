using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using EPiServer.Logging.Compatibility;
using EPiServer.ServiceLocation;
using Newtonsoft.Json;

namespace Geta.SocialChannels.Twitter
{
    [ServiceConfiguration(typeof(ITwitterService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class TwitterService : ITwitterService
    {
        private const string TimelineApi = "https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&trim_user=1&exclude_replies=1";
        private const string TokenApi = "https://api.twitter.com/oauth2/token";
        private const string TwitterLink = "https://twitter.com/{0}/status/{1}";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(TwitterService));
        private readonly ICache _cache;
        private string _twitterUserName;
        private string _twitterConsumerKey;
        private string _twitterConsumerSecretKey;

        private readonly bool _useCache;
        private readonly int _cacheDuration;
        private readonly int _numberFeedItems;

        public TwitterService(ICache cache)
        {
            this._cache = cache;

            //var startPage = ContentReference.StartPage.Get<StartPage>();

            //_numberFeedItems = startPage.SocialFeedSettings.NumberSocialFeedItems > 0 ? startPage.SocialFeedSettings.NumberSocialFeedItems : Constants.DefaultNumberSocialFeedItems;
            //_cacheDuration = startPage.SocialFeedSettings.SocialFeedCacheDuration > 0 ? startPage.SocialFeedSettings.SocialFeedCacheDuration : Constants.DefaultSocialFeedCacheDurationForMinutes;
            //_useCache = startPage.SocialFeedSettings.EnableSocialFeedCache;
        }

        public void Config(string appConsumerKey, string appConsumerSecret, string userName)
        {
            _twitterConsumerKey = appConsumerKey;
            _twitterConsumerSecretKey = appConsumerSecret;
            _twitterUserName = userName;
        }

        public List<TweetItemModel> GetTweets()
        {
            var key = $"tweet_items_{_numberFeedItems}_{_twitterUserName}";

            if (_cache.Exists(key) && _useCache)
            {
                return _cache.Get<List<TweetItemModel>>(key);
            }

            var accessToken = GetAccessKey();

            if (accessToken == null)
            {
                return null;
            }

            var request = WebRequest.Create(string.Format(TimelineApi, _numberFeedItems, _twitterUserName));
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            try
            {
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream == null)
                {
                    return null;
                }

                using (var sd = new StreamReader(stream, Encoding.UTF8))
                {
                    var resultString = sd.ReadToEnd();
                    dynamic json = JsonConvert.DeserializeObject<object>(resultString);
                    var enumerableTwitts = (json as IEnumerable<dynamic>);

                    if (enumerableTwitts == null)
                    {
                        return null;
                    }

                    response.Close();

                    var items = enumerableTwitts.Select(t => new TweetItemModel
                    {
                        StatusId = (string)(t["id_str"].ToString()),
                        Link = GetTweetLink((string)(t["id_str"].ToString())),
                        CreatedDate = t["created_at"].ToString(),
                        Text = t["text"] != null ? WebUtility.HtmlDecode(t["text"].ToString()) : ""
                    }).ToList();

                    if (_useCache && items.Any())
                    {
                        _cache.Add(key, items, new TimeSpan(0, _cacheDuration, 0));
                    }

                    return items;
                }
            }
            catch (Exception e)
            {
                Logger.Error("TwitterService", e);
            }

            return null;
        }
        private string GetTweetLink(string statusId)
        {
            return string.Format(TwitterLink, _twitterUserName, statusId);
        }

        public string GetAccessKey()
        {
            var key = $"tweet_token_key_{_twitterConsumerKey}";

            if (_cache.Exists(key) && _useCache)
            {
                return _cache.Get<string>(key);
            }
            if (string.IsNullOrEmpty(_twitterConsumerKey) || string.IsNullOrEmpty(_twitterConsumerSecretKey))
            {
                return string.Empty;
            }

            var request = WebRequest.Create(TokenApi);
            request.Method = "POST";
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(_twitterConsumerKey + ":" + _twitterConsumerSecretKey));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    var content = Encoding.ASCII.GetBytes("grant_type=client_credentials");
                    stream.Write(content, 0, content.Length);
                }

                var response = request.GetResponse();
                var stream2 = response.GetResponseStream();
                if (stream2 == null)
                {
                    return null;
                }

                using (var sd = new StreamReader(stream2))
                {
                    var resultString = sd.ReadToEnd();
                    dynamic item = JsonConvert.DeserializeObject<object>(resultString);

                    response.Close();

                    var tokenKey = item["access_token"];

                    if (_useCache && tokenKey != null)
                    {
                        _cache.Add<string>(key, tokenKey, new TimeSpan(0, _cacheDuration, 0));
                    }

                    return item["access_token"];
                }
            }
            catch (Exception e)
            {
                Logger.Error("TwitterService", e);
            }

            return string.Empty;
        }
    }
}