# UriBuilderExtended
Extensions to .NET's `System.UriBuilder`.

## Introduction
A thing that is missing from .NET's `System.UriBuilder` is a good way to edit the query string. That is what made me look for others solution to this problem, and ultimately made me start this project.

Other good solutions exist that may cover your needs, but I was unable to find one that that covered my requirements:

1. Do anything `UriBuilder` does
2. Set the query string with support for multiple values
3. Be as simple as possible

I have built this with great inspiration from [URI.js](https://medialize.github.io/URI.js/), which is a very good library for modifying URIs in JavaScript.

## Documentation
Install the [package from NuGet](https://www.nuget.org/packages/UriBuilderExtended/)

Add `using UriBuilderExtended;` to your source files, and you will have the following methods available on your `UriBuilder` objects:

`bool HasQuery(string key)`
Check for the existence of a query with the given key

`bool HasQuery(string key, params string[] values)`
Check for the existence of a query with the given key and the given values

`UriBuilder RemoveQuery(string key)`
Remove any query with the given key

`UriBuilder SetQuery(string key, params string[] values)`
Sets the query parameter for a given key

`UriBuilder AddQuery(string key, params string[] values)`
Adds a query parameter for a given key

## Examples

```C#
    UriBuilder builder = new UriBuilder("http://www.mysite.net/");
    // URL is now http://www.mysite.net/
    
    // URL string  is obtained from builder.Uri.ToString(), not builder.ToString()
    // as it sometimes renders differently to what you'd expect

    builder.AddQuery("myKey", "myValue1");
    // URL is now http://www.mysite.net/?myKey=myValue1

    builder.AddQuery("myOtherKey", "myOtherValue1");
    // URL is now http://www.mysite.net/?myKey=myValue1&myOtherKey=myOtherValue1

    builder.AddQuery("myKey", "myValue2", "myValue3");
    // URL is now http://www.mysite.net/?myKey=myValue1&myKey=myValue2&myKey=myValue3&myOtherKey=myOtherValue1

    builder.SetQuery("myKey", "newValue1", "newValue2");
    // URL is now http://www.mysite.net/?myOtherKey=myOtherValue1&myKey=newValue1&myKey=newValue2

    builder.HasQuery("myKey");
    builder.HasQuery("myOtherKey");
    // true

    builder.HasQuery("myKey", "myValue1");
    // false

    builder.HasQuery("myKey", "newValue1");
    // true

    builder.HasQuery("notMymyKey");
    // false
```

## Future plans

I plan on adding the following methods:
##### `List<string> GetQueryValues(string key)`
Get all values for the given key
