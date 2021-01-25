# Geta Social Channels libraries

* Master<br>
![](http://tc.geta.no/app/rest/builds/buildType:(id:Kolumbus_Main_00ci),branch:master/statusIcon)
[![Platform](https://img.shields.io/badge/Platform-.NET%204.6.1-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/Episerver-%2011-orange.svg?style=flat)](http://world.episerver.com/cms/)

## Description
This project contains 6 different packages for 5 different social media channels.

* [Facebook](https://www.facebook.com)
* [Twitter](https://www.twitter.com)
* [YouTube](https://www.youtube.com)
* [Instagram](https://www.instagram.com)

## Features
* Integrations to social media channels
* Caching of response - by default caching is enabled and set to 10 minutes
* Facebook: get Facebook account information by username
* Facebook: get X posts by username
* Twitter: get X tweets by username
* Instagram: get posts by your Instagram Business Account
* Instagram: get recent posts by hashtag
* YouTube: get X feed by channel id

## Installation
Available through Geta's NuGet feed:
- Geta.SocialChannels
- Geta.SocialChannels.Facebook
- Geta.SocialChannels.Twitter
- Geta.SocialChannels.YouTube
- Geta.SocialChannels.Instagram

To install Facebook
```
Install-Package Geta.SocialChannels.Facebook
```

To install Twitter
```
Install-Package Geta.SocialChannels.Twitter
```

To install YouTube
```
Install-Package Geta.SocialChannels.YouTube
```

To install Instagram
```
Install-Package Geta.SocialChannels.Instagram
```

## Facebook
1.	Setting App ID and App Secret:

 Step 1. Go to your app (https://developers.facebook.com/apps )
 Step 2. Click to view detail


2.	To find Page ID:

 Step 1. Go to your page
 Step 2. Click "Settings"
 Step 3. Click "Page Info"
 Step 4. See "Facebook Page ID"


```csharp
public interface IFacebookService
{
	void Config(bool useCache, int cacheDurationInMinutes);

	FacbookAuthorInformation GetInformation(string username);

	FacebookFeedResponse GetFacebookFeed(FacebookFeedRequest facebookFeedRequest);
}

public class FacebookFeedRequest
{
    public string Username { get; set; }
	public int MaxCount { get; set; } = 10;
}

public class FacebookFeedResponse
{
	public List<FacebookPostItem> Data;
}

public class FacbookAuthorInformation
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Url => $"https://www.facebook.com/{Id}";
}

public class FacebookPostItem
{
	public string Id { get; set; }

	public string Message { get; set; }

	[JsonProperty(PropertyName = "Created_Time")]
	public DateTime CreatedTime { get; set; }

	public string CreatedTimeSince => CreatedTime.ToTimeSinceString();

	public FacbookAuthorInformation From { get; set; }
}
```

### Examples
```csharp
var facebookService = new FacebookService(new Cache(), "appId", "appSecret")

var facebookFeed = facebookService.GetFacebookFeed(new FacebookFeedRequest { Username = "geta" });
```

## Twitter

Setting App Consumer Key and App Consumer Secret:
Step 1. Go to your app (https://apps.twitter.com/)
Step 2. Click on Keys and Access Tokens tab


```csharp
public interface ITwitterService
{
	void Config(bool useCache, int cacheDurationInMinutes);

	GetTweetsResponse GetTweets(GetTweetsRequest getTweetsRequest);
}
```

```csharp
public class GetTweetsRequest
{
	public string UserName { get; set; }

	public int MaxCount { get; set; } = 10;
}
```

```csharp
public class GetTweetsResponse
{
	public bool Success { get; set; }

	public string ErrorMessage { get; set; }

	public List<TweetItemModel> Tweets { get; set; }
}
```

```csharp
public class TweetItemModel
{
	public string StatusId { get; set; }

	public string Text { get; set; }

	public string Link { get; set; }

	public string CreatedDate { get; set; }

	public string CreatedTimeSince
	{
		get
		{
			DateTime dateTime;

			if (!string.IsNullOrEmpty(CreatedDate) && DateTime.TryParseExact(CreatedDate, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
			{
				return dateTime.ToTimeSinceString();
			}

			return string.Empty;
		}
	}
}
```

### Examples

```csharp
var twitterService = new TwitterService(new Cache(), "consumerKey", "secretKey");

var tweets = twitterService.GetTweets(new GetTweetsRequest{ UserName = "Geta_digital"})

```

## YouTube
Gets the videos for a channel. You'll need an [API key](https://developers.google.com/youtube/v3/getting-started) and also your channel ID.

To find Channel Id, sign in to Youtube and check [advanced your setting page](https://www.youtube.com/account_advanced)

```csharp
public interface IYoutubeService
{
	void Config(bool useCache, int cacheDurationInMinutes);

	GetYoutubeFeedResponse GetYoutubeFeed(GetYoutubeFeedRequest getYoutubeFeedRequest);
}

public class GetYoutubeFeedRequest
{
	public string ChannelId { get; set; }

	public int MaxCount { get; set; } = 10;
}

public class GetYoutubeFeedResponse
{
	 public List<YoutubeDetailModel> Data { get; set; }
}

public class YoutubeDetailModel
{
	public string Title { get; set; }

	public string Description { get; set; }

	public string ImageUrl { get; set; }

	public string ViewCount { get; set; }

	public string LikeCount { get; set; }

	public string DislikeCount { get; set; }

	public string FavoriteCount { get; set; }

	public DateTime PublishDate { get; set; }

	public string VideoUrl { get; set; }

	public string CreatedTimeSince => PublishDate.ToTimeSinceString();
}

public class YoutubeModel
{
	public class PageInfo
	{
		public int TotalResults { get; set; }
		public int ResultsPerPage { get; set; }
	}

	public class Default
	{
		public string Url { get; set; }
	}

	public class Medium
	{
		public string Url { get; set; }
	}

	public class High
	{
		public string Url { get; set; }
	}

	public class Thumbnails
	{
		public Default Default { get; set; }
		public Medium Medium { get; set; }
		public High High { get; set; }
	}

	public class Snippet
	{
		public DateTime PublishedAt { get; set; }
		public string ChannelId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Thumbnails Thumbnails { get; set; }
		public string ChannelTitle { get; set; }
		public string LiveBroadcastContent { get; set; }
	}

	public class Item
	{
		public string Kind { get; set; }
		public string Etag { get; set; }
		public Snippet Snippet { get; set; }
		public Statistics Statistics { get; set; }
	}

	public class RootObject
	{
		public string Kind { get; set; }

		public string Etag { get; set; }

		public PageInfo PageInfo { get; set; }

		public List<Item> Items { get; set; }

		public Statistics Statistics { get; set; }
	}

	public class Statistics
	{
		public string ViewCount { get; set; }

		public string LikeCount { get; set; }

		public string DislikeCount { get; set; }

		public string FavoriteCount { get; set; }

		public string CommentCount { get; set; }
	}
}
```

### Examples

```csharp
var youTubeService = new YouTubeService(new Cache(), "youtubeKey");

var feed = youTubeService.GetYoutubeFeed(new GetYoutubeFeedRequest{ ChannelId = "channelId"});

```

## Instagram
Gets the instagram posts of the business account associated with the token, or from a hashtag.
To retrieve the access token, see the documentation from Instagram: [https://developers.facebook.com/docs/facebook-login/access-tokens/](https://developers.facebook.com/docs/facebook-login/access-tokens/). Pro tip, simplest way is to just follow the client side instructions. See more here about Facebook Login [https://developers.facebook.com/docs/facebook-login/](https://developers.facebook.com/docs/facebook-login/).

```csharp
public interface IInstagramService
{
    void Config(bool useCache, int cacheDurationInMinutes);
    List<Media> GetMedia();
    List<Media> GetMediaByHashTag(string tag);
}
```
```csharp
public class CommentData
{
    [JsonProperty("text")]
    public string text { get; set; }

    [JsonProperty("id")]
    public string id { get; set; }
}

public class Cursors
{
    [JsonProperty("after")]
    public string after { get; set; }
}

public class Paging
{
    [JsonProperty("cursors")]
    public Cursors cursors { get; set; }

    [JsonProperty("next")]
    public string next { get; set; }

    public Paging()
    {
        this.cursors = new Cursors();
    }
}

public class Comments
{
    [JsonProperty("data")]
    public List<CommentData> data { get; set; }

    [JsonProperty("paging")]
    public Paging paging { get; set; }

    public Comments()
    {
        this.paging = new Paging();
        this.data = new List<CommentData>();
    }
}

public class MediaData
{
    [JsonProperty("caption")]
    public string caption { get; set; }

    [JsonProperty("media_url")]
    public string media_url { get; set; }

    [JsonProperty("media_type")]
    public string media_type { get; set; }

    [JsonProperty("comments_count")]
    public int comments_count { get; set; }

    [JsonProperty("like_count")]
    public int like_count { get; set; }

    [JsonProperty("permalink")]
    public string permalink { get; set; }

    [JsonProperty("comments")]
    public Comments comments { get; set; }

    [JsonProperty("id")]
    public string id { get; set; }

    [JsonProperty("timestamp")]
    public DateTime timestamp { get; set; }
}

public class Media
{
    [JsonProperty("data")]
    public List<MediaData> data { get; set; }

    [JsonProperty("paging")]
    public Paging paging { get; set; }

    public Media()
    {
        this.data = new List<MediaData>();
        this.paging = new Paging();
    }
}

public class HashtagSearchResult
{
    [JsonProperty("data")]
    public List<MediaData> data { get; set; }
}

public class InstagramResult
{
    [JsonProperty("media")]
    public Media media { get; set; }

    [JsonProperty("id")]
    public string id { get; set; }
}
```
### Examples
```csharp
var instagramService = new InstagramService(new Cache(), "instagramAccessToken", "InstagramBusinessAccountId");

var postsBySelf = instagramService.GetMedia();
var postsByTag = instagramService.GetMediaByHashTag("geta");
```

## Local development setup

See description in [shared repository](https://github.com/Geta/package-shared/blob/master/README.md#local-development-set-up) regarding how to setup local development environment.

### Docker hostnames

Instead of using the static IP addresses the following hostnames can be used out-of-the-box.

- http://socialchannels.getalocaltest.me

## Package maintainer
https://github.com/m-kovacina

## Changelog
[Changelog](CHANGELOG.md)
