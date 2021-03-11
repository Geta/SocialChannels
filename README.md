# Geta Social Channels libraries

* Master<br>
![](http://tc.geta.no/app/rest/builds/buildType:(id:GetaPackages_GetaSocialChannels_00ci),branch:master/statusIcon)
## Description
This project contains 4 different packages for 3 different social media channels.

* [Twitter](https://www.twitter.com)
* [YouTube](https://www.youtube.com)
* [Instagram](https://www.instagram.com)

## Features
* Integrations to social media channels
* Twitter: get X tweets by username
* Instagram: get posts by your Instagram Business Account
* Instagram: get recent posts by hashtag
* YouTube: get X feed by channel id

## Installation
Available through Geta's NuGet feed:
- Geta.SocialChannels
- Geta.SocialChannels.Twitter
- Geta.SocialChannels.YouTube
- Geta.SocialChannels.Instagram

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

To install Facebook
```
Install-Package Geta.SocialChannels.Facebook
```

## Twitter

Setting App Consumer Key and App Consumer Secret:
Step 1. Go to your app (https://apps.twitter.com/)
Step 2. Click on Keys and Access Tokens tab

More info: https://developer.twitter.com/en/docs/twitter-api

### Examples

```csharp
var twitterService = new TwitterService("apiKey", "secretKey", new Cache());
var tweets = twitterService.GetTweets(new GetTweetsRequest{ UserName = "Geta_digital"})
```

## YouTube
Gets the videos for a channel. You'll need an [API key](https://developers.google.com/youtube/v3/getting-started) and also your channel ID.

To find Channel Id, sign in to Youtube and check [advanced your setting page](https://www.youtube.com/account_advanced)


### Examples

```csharp
var youTubeService = new YouTubeService("youtubeKey", new Cache());
var feed = youTubeService.GetYoutubeFeed(new GetYoutubeFeedRequest{ ChannelId = "channelId"});
```

## Instagram
Gets the instagram posts of the business account associated with the token, or from a hashtag.
To retrieve the access token, see the documentation from Instagram: [https://developers.facebook.com/docs/facebook-login/access-tokens/](https://developers.facebook.com/docs/facebook-login/access-tokens/). 
Make sure to use [Long-Lived Page Access Token](https://developers.facebook.com/docs/facebook-login/access-tokens/refreshing/#get-a-long-lived-page-access-token)

### Examples
```csharp
var instagramService = new InstagramService("instagramAccessToken", "InstagramBusinessAccountId", new Cache());
var postsBySelf = instagramService.GetMedia();
var postsByTag = instagramService.GetMediaByHashTag("geta");
```

## Facebook
Gets Facebook feed by user name or account information by username. 
To retrieve the access token, see the documentation from Facebook: [https://developers.facebook.com/docs/facebook-login/access-tokens/](https://developers.facebook.com/docs/facebook-login/access-tokens/).
Make sure to use [Long-Lived Page Access Token](https://developers.facebook.com/docs/facebook-login/access-tokens/refreshing/#get-a-long-lived-page-access-token)

If you want to access other public pages, which are not associated with your App and token you need have following permission:
[Page Public Content Access](https://developers.facebook.com/docs/apps/features-reference/page-public-content-access)

####Useful tools:
[Graph API Explorer](https://developers.facebook.com/tools/explorer/)

[Access token debugger](https://developers.facebook.com/tools/debug/accesstoken/)

### Examples
```csharp
var facebookService = new FacebookService("accessToken", new Cache());
var facebookFeed = facebookService.GetFacebookFeed(new FacebookFeedRequest
{
 UserName = "getatesting"
});
var accountInformation = facebookService.GetInformation("getatesting");```
```
## Changelog
[Changelog](CHANGELOG.md)
