dotnet-craigslist
=================

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

    dotnet install package dotnet-craigslist

Classes
-------

TODO

Examples
--------

Looking for a room in San Francisco?

.. code:: C#

    // TODO

Maybe a software engineering internship in Silicon Valley?

.. code:: C#

    // TODO

Events with free food in New York?

.. code:: C#

    // TODO

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
