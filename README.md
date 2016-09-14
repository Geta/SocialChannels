# Geta Social Channels libraries

![](http://tc.geta.no/app/rest/builds/buildType:(id:TeamFrederik_SocialChannels_Debug)/statusIcon)

This project contains 6 different packages for 5 different social media channels.

By default caching is enabled and set to 10 minutes.

 * [Facebook](https://www.facebook.com)
 * [Twitter](https://www.twitter.com)
 * [LinkedIn](https://www.linkedin.com)
 * [YouTube](https://www.youtube.com)
 * [Instagram](https://www.instagram.com)
 
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

	FacbookAuthorInformation GetInformation();

	FacebookFeedResponse GetFacebookFeed(FacebookFeedRequest facebookFeedRequest);
}

public class FacebookFeedRequest
{
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
var facebookService = new FacebookService(new Cache(), "appId", "appSecret", "facebookId")

var facebookFeed = facebookService.GetFacebookFeed(new FacebookFeedRequest());
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