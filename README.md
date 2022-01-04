dotnet-craigslist
=================

[![](https://img.shields.io/github/workflow/status/wesleythorsen1/dotnet-craigslist/Build%20and%20Publish)](https://github.com/wesleythorsen1/dotnet-craigslist/actions/workflows/build.yml)
[![](https://sonarcloud.io/api/project_badges/measure?project=wesleythorsen1_dotnet-craigslist&metric=alert_status)](https://sonarcloud.io/dashboard?id=wesleythorsen1_dotnet-craigslist)
[![](https://img.shields.io/nuget/dt/DotnetCraigslist)](https://www.nuget.org/packages/DotnetCraigslist/)
[![](https://img.shields.io/nuget/v/DotnetCraigslist)](https://www.nuget.org/packages/DotnetCraigslist/)

A simple [Craigslist](http://www.craigslist.org) client for dotnet.

License: [MIT](https://opensource.org/licenses/MIT)

Disclaimer
----------

* I don't work for or have any affiliation with Craigslist.
* This module was implemented for educational purposes. It should not be used for crawling or downloading data from Craigslist.
* This project was inspired by juliomalegria's [python-craigslist](https://github.com/juliomalegria/python-craigslist) project, kudos for the excellent python client and inspiration for this project.

Installation
------------

    dotnet add package DotnetCraigslist

Quick Start
-----------

Search Craigslist:

```C#
using DotnetCraigslist;

var client = new CraigslistClient();

var request = new SearchHousingRequest(
    "seattle", 
    SearchHousingRequest.Categories.ApartmentsHousingForRent)
    {
        MaxPrice = 1500,
        MinBedrooms = 1
    };

var searchResults = await client.SearchAsync(request);
```
Get Posting:

```C#
using DotnetCraigslist;

var postingRequest = new PostingRequest(
    "seattle", 
    SearchHousingRequest.Categories.ApartmentsHousingForRent,
    "posting_id_here");

var posting = await client.GetPostingAsync(postingRequest);
```
Posting IDs can be found in the `searchResults` object, or in a posting's URL if viewing with a browser. 

Streaming
---------

The `CraigslistStreamingClient` can be used to continuously stream new search results and postings.

* `.StreamSearchResults(...)` will periodically search Craigslist and return any new search results.
* `.StreamPostings(...)` will do the same, however it will perform an additional request per new search result to get the posting's full details.

```C#
using DotnetCraigslist;

var client = new CraigslistStreamingClient();

var request = new SearchForSaleRequest(
    "losangeles", 
    SearchForSaleRequest.Categories.Electronics)
    {
        SearchText = "graphics card",
    };

await foreach(var posting in client.StreamPostings(request))
{
    // process posting...
}
```

Notes on streaming methods:

* The `IAsyncEnumerable` returned by `.StreamSearchResults(...)` and `.StreamPostings(...)` will never end, unless the cancelationToken is requested or there is a fatal error. You should not use any LINQ methods that need to perform a full enumeration on these methods (`.Count()`, `.ToList()`, `.Reverse()` etc.).
* `.StreamSearchResults(...)` and `.StreamPostings(...)` will refresh the search results every 5 minutes. However, Craigslist only indexes new postings every 15 minutes. In general you will only see new results every 15 minutes. Be patient.
* Items returned in stream will be in the reverse order listed on search page. (returned in chronological order, oldest to newest)
* When `.StreamSearchResults(...)` or `.StreamPostings(...)` is initially called, it will return the first 5 search results, even if they are not new. This allows the client to determine which subsequent results are new. If you wish to see ONLY new results after the stream is started, skip the first 5 results returned.
* If `.StreamPostings(...)` is used, an additional request per new search result will be made in order to get the full posting details. Be aware that this could cause a large number of requests, resulting in a Craigslist ban, or the stream falling behind. To avoid this, try narrowing down the initial search request to limit the number of results returned.

Limitations
-----------

Sending a large number of / frequent requests to Craigslist will result in a IP ban. To avoid this, the default constructors of the `CraigslistClient` and `CraigslistStreamingClient` inject a request rate limiter, and will force all requests to wait 5 seconds from the previous request before sending. This helps ensure that out-of-the-box users do not unintentionally get banned. If you wish to change this behavior, provide your own `HttpClient` to the constructors of the `CraigslistClient` or `CraigslistStreamingClient` classes.

Other Examples
--------

Looking for software engineering internships in Silicon Valley?

```C#
var client = new CraigslistClient();
var request = new SearchJobsRequest("sfbay", "sby", SearchJobsRequest.Categories.SoftwareQaDbaEtc)
{
    Internship = true,
    EmploymentTypes = new[]
    {
        SearchJobsRequest.EmploymentType.FullTime,
        SearchJobsRequest.EmploymentType.PartTime,
    }
};
var searchResults = client.Search(request);
```

An event with free food in New York?

```C#
var client = new CraigslistClient();
var request = new SearchEventsRequest("newyork")
{
    Free = true,
    FoodDrink = true,
};
var searchResults = await client.SearchAsync(request);

var postingUrl = searchResults.Results.First().ListingUrl;
var postingRequest = new PostingRequest(postingUrl);
var eventPosting = await client.GetPostingAsync(postingRequest);
```

Classes
-------

* `CraigslistClient`
  * For sending search and posting requests.
* `CraigslistStreamingClient`
  * For continuously streaming new search results or postings
* Requests
  * `SearchRequest`
    * A base search request type. Can be used instead of the derived classes. Use these to "search" Craigslist.
    * `SearchCommunityRequest`
    * `SearchEventsRequest`
    * `SearchForSaleRequest`
    * `SearchGigsRequest`
    * `SearchHousingRequest`
    * `SearchJobsRequest`
    * `SearchResumesRequest`
    * `SearchServicesRequest`
  * `PostingRequest`
    * A request type for getting postings on Craigslist. Use this to get the full details on individual postings.
* Responses
  * `SearchResults`
    * Contains a "page" of search results from Craigslist. Use the `.NextUrl` property to navigate to the next page of results.
  * `SearchResult`
    * An individual search result that is displayed on the Search Results page.
  * `Posting`
    * Represents a posting on Craigslist. Contains all information that is on a posting.

Where to get `site` and `area` from?
------------------------------------

When initializing any of the request types, you'll need to provide the `site`, and optionally `area`, from where you want to query data.

To get the correct `site`, follow these steps:

1. Go to [craigslist.org/about/sites](https://www.craigslist.org/about/sites).
2. Find the country or city you're interested on, and click on it.
3. You'll be directed to `<site>.craigslist.org`. The value of `<site>` in the URL is the one you should use.

Not all sites have areas. To check if your site has areas, check for links next to the title of the Craigslist page, on the top center. For example, for New York you'll see:

![](https://user-images.githubusercontent.com/1008637/45307206-bb404d80-b51e-11e8-8e6d-edfbdbd0a6fa.png)

Click on the one you're interested, and you'll be redirected to `<site>.craigslist.org/<area>`. The value of `<area>` in the URL is the one you should use. If there are no areas next to the title, it means your site has no areas, and you can leave that argument unset.

Where to get `category` from?
-----------------------------

Each of the SearchRequest classes contains a static class `Categories` with constant values for all valid categories. They can be accesed like: `SearchForSaleRequest.Categories.Electronics`.

Alternatively, you can specifiy your own category. They can be found in the url when searching Craigslist: `<site>.craigslist.org/search/<area>/<category>`

Support
-------

If you find any bug or you want to propose a new feature, please use the [issues tracker](https://github.com/wesleythorsen1/dotnet-craigslist/issues). I'll be happy to help you! :-)
