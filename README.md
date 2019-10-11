# Geta Social Channels libraries

![](http://tc.geta.no/app/rest/builds/buildType:(id:TeamFrederik_SocialChannels_Debug)/statusIcon)
[![Platform](https://img.shields.io/badge/Platform-.NET%204.6.1-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/Episerver-%2011-orange.svg?style=flat)](http://world.episerver.com/cms/)

## Description
This project contains 6 different packages for 5 different social media channels.

* [Facebook](https://www.facebook.com)
* [Twitter](https://www.twitter.com)
* [LinkedIn](https://www.linkedin.com)
* [YouTube](https://www.youtube.com)
* [Instagram](https://www.instagram.com)

## Features
* Integrations to social media channels
* Caching of response - by default caching is enabled and set to 10 minutes
* Facebook: get Facebook account information by username
* Facebook: get X posts by username
* Twitter: get X tweets by username
* Instagram: get X posts by self
* Instagram: get X posts by username
* Instagram: get X posts by tag
* LinkedIn: EPiServer feed block
* LinkedIn: get X feed by company id
* YouTube: get X feed by channel id

## Installation
Available through Geta's NuGet feed:
- Geta.SocialChannels
- Geta.SocialChannels.Facebook
- Geta.SocialChannels.Twitter
- Geta.SocialChannels.LinkedIn
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

To install LinkedIn
```
Install-Package Geta.SocialChannels.LinkedIn
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
Gets the instagram posts of the user associated with the token, a different user, or from a hashtag.
To retrieve the access token, see the documentation from Instagram: [https://www.instagram.com/developer/authentication/](https://www.instagram.com/developer/authentication/). Pro tip, simplest way is to just follow the client side instructions.

```csharp
public interface IInstagramService
{
    void Config(bool useCache, int cacheDurationInMinutes);
    InstagramResponse GetPostsBySelf(int maxCount);
    InstagramResponse GetPostsByUser(GetPostsRequest request);
    InstagramResponse GetPostsByTag(GetPostsRequest request);
}
```
```csharp
public class GetPostsRequest
{
    public string Query { get; set; }
    public int MaxCount { get; set; }
}
```
```csharp
public class InstagramResponse
{
    public Datum[] Data { get; set; }
    public Pagination Page { get; set; }
}

public class Datum
{
    public object[] Tags { get; set; }
    public double CreatedTime { get; set; }
    public string Link { get; set; }
    public Likes Likes { get; set; }
    public Images Images { get; set; }
    public Videos Videos { get; set; }
    public Caption Caption { get; set; }
    public User User { get; set; }
    public string Id { get; set; }
}

public class Caption
{
    public double CreatedTime { get; set; }
    public string Text { get; set; }
    public PostFrom From { get; set; }
    public string Id { get; set; }
}

public class Images
{
    public StandardResolutionImage Picture { get; set; }
    public LowResolutionImage LowResPicture { get; set; }
    public ThumbnailImage ThumbnailPicture { get; set; }
}

public class Likes
{
    public int Count { get; set; }
}

public class LowBandwidthVideo
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class LowResolutionImage
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class LowResolutionVideo
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class Pagination
{
    public string NextUrl { get; set; }
    public string NextMaxId { get; set; }
}

public class PostFrom
{
    public string UserName { get; set; }
    public string ProfilePicture { get; set; }
    public string Id { get; set; }
    public string FullName { get; set; }
}

public class StandardResolutionImage
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class StandardResolutionVideo
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class ThumbnailImage
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class User
{
    public string UserName { get; set; }
    public string ProfilePicture { get; set; }
    public string Id { get; set; }
    public string FullName { get; set; }
}

public class Videos
{
    public LowResolutionVideo LowResVideo { get; set; }
    public StandardResolutionVideo StandardResVideo { get; set; }
    public LowBandwidthVideo LowBandwidthVideo { get; set; }
}
```
### Examples
```csharp
var instagramService = new InstagramService(new Cache(), "instagramAccessToken");

var postsBySelf = instagramService.GetPostsBySelf(10);
var postsByUser = instagramService.GetPostsByUser(new GetPostsRequest { Query = "geta", MaxCount = 10 });
var postsByTag = instagramService.GetPostsByTag(new GetPostsRequest { Query = "development", MaxCount = 10 });
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
