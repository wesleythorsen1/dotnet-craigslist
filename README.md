dotnet-craigslist
=================

[![](https://img.shields.io/github/workflow/status/wesleythorsen1/dotnet-craigslist/Build%20and%20Publish)](https://github.com/wesleythorsen1/dotnet-craigslist/actions/workflows/build.yml)
![](https://img.shields.io/nuget/dt/DotnetCraigslist)
![](https://img.shields.io/nuget/v/DotnetCraigslist)

A simple `Craigslist <http://www.craigslist.org>`__ wrapper for dotnet.

License: `MIT-Zero <https://romanrm.net/mit-zero>`__.

Disclaimer
----------

* I don't work for or have any affiliation with Craigslist.
* This module was implemented for educational purposes. It should not be used for crawling or downloading data from Craigslist.
* This project was initially based on juliomalegria's `python-craigslist <https://github.com/juliomalegria/python-craigslist>`__ project, kudos for the excellent python wrapper and reference for this project.

Installation
------------

::

    dotnet install package DotnetCraigslist

Quick Start
----------

Example `Program.cs`:

.. code:: C#

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DotnetCraigslist;

    var client = new CraigslistClient();

    // Create search request
    var request = new SearchHousingRequest("seattle", SearchHousingRequest.Categories.ApartmentsHousingForRent)
    {
        MaxPrice = 1500,
        MinBedrooms = 1,
        ParkingOptions = new[]
        {
            SearchHousingRequest.Parking.Carport,
            SearchHousingRequest.Parking.AttachedGarage,
        },
        // "Miles From Location":
        PostalCode = "98101",
        SearchDistance = 3.5f,
    };

    // Send search request
    var searchResults = await client.SearchAsync(request);

    // Print search results
    foreach (var result in searchResults.Results)
    {
        Console.WriteLine($"{result.Date.ToString("T"),-13}{result.Price,-9}{result.Title,-27}");
    }

    // Find newest posting in search results
    var newestSearchResult = searchResults.Results
        .OrderByDescending(r => r.Date)
        .First();

    // Create a posting request from the search result
    var postingRequest = new PostingRequest(newestSearchResult);

    // Send posting request
    var posting = await client.GetPostingAsync(postingRequest);

    // Print the posting details
    Console.WriteLine($@"
    URL: {posting.PostingUri}
    ID: {posting.Id}
    Posted: {posting.Posted.ToString("O")}
    Price: {posting.Price}
    Title: {posting.Title}
    Location: ({posting.Location?.Latitude}, {posting.Location?.Longitude})
    Additional Attributes: {string.Join(", ", posting.AdditionalAttributes)}
    Description:
    {posting.Description}");

The above code demonstrates how to:

1. Create and send a search request (equivalent of searching Craigslist)
2. Creating a posting request from an item in the seach results, and sending the posting request (equivalent of clicking an item in the search results and viewing the posting on Craigslist)

Classes
-------

TODO

Other Examples
--------

Looking for a room in San Francisco?

.. code:: C#

    var client = new CraigslistClient();
    var request = new SearchHousingRequest("sfbay", SearchHousingRequest.Categories.RoomsAndShares)
    {
        MaxPrice = 1500,
        PrivateRoom = true,
    };
    var searchResults = await client.SearchAsync(request);

Maybe software engineering internships in Silicon Valley?

.. code:: C#

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

An event with free food in New York?

.. code:: C#

    var client = new CraigslistClient();
    var request = new SearchEventsRequest("newyork")
    {
        Free = true,
        FoodDrink = true,
    };
    var searchResults = await client.SearchAsync(request);
    var postingUrl = searchResults.Results.First().ListingUrl; // using System.Linq;
    var postingRequest = new PostingRequest(postingUrl);
    var eventPosting = await client.GetPostingAsync(postingRequest);

Where to get `filters` from?
----------------------------

Every subclass has its own set of filters. To get a list of all the filters
supported by a specific subclass, use the ``.show_filters()`` class-method:

.. code:: C#

   // TODO

Where to get ``site`` and ``area`` from?
----------------------------------------

When initializing any of the subclasses, you'll need to provide the ``site``, and optionall the ``area``, from where you want to query data.

To get the correct ``site``, follow these steps:

1. Go to `craigslist.org/about/sites <https://www.craigslist.org/about/sites>`__.
2. Find the country or city you're interested on, and click on it.
3. You'll be directed to ``<site>.craigslist.org``. The value of ``<site>`` in the URL is the one you should use.

Not all sites have areas. To check if your site has areas, check for links next to the title of the Craigslist page, on the top center. For example, for New York you'll see:

.. image:: https://user-images.githubusercontent.com/1008637/45307206-bb404d80-b51e-11e8-8e6d-edfbdbd0a6fa.png

Click on the one you're interested, and you'll be redirected to ``<site>.craigslist.org/<area>``. The value of ``<area>`` in the URL is the one you should use. If there are no areas next to the title, it means your site has no areas, and you can leave that argument unset.

Where to get ``category`` from?
-------------------------------

You can additionally provide a ``category`` when initializing any of the subclasses. To get a list of all the categories
supported by a specific subclass, use the ``.show_categories()`` class-method:

.. code:: C#
    
    // TODO

Is there a limit for the number of results?
--------------------------------------------

Yes, Craigslist caps the results for any search to 3000.

Support
-------

If you find any bug or you want to propose a new feature, please use the `issues tracker <https://github.com/wesleythorsen1/dotnet-craigslist/issues>`__. I'll be happy to help you! :-)
