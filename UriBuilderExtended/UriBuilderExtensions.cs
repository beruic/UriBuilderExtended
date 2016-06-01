using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace UriBuilderExtended
{
    public static class UriBuilderExtensions
    {
        /// <summary>
        /// Check for the existence of a query with the given key
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/></param>
        /// <param name="key">The key to look for</param>
        /// <returns>True or false</returns>
        public static bool HasQuery(this UriBuilder uri, string key)
        {
            // Get the collection of query keys and values
            NameValueCollection queryValues = uri.ParseQuery();

            string values = queryValues.Get(key);

            // Clear current values for key
            return values != null;
        }

        /// <summary>
        /// Check for the existence of a query with the given key and the given values
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/></param>
        /// <param name="key">The key to look for</param>
        /// <param name="values">The values to look for</param>
        /// <returns></returns>
        public static bool HasQuery(this UriBuilder uri, string key, params string[] values)
        {
            if (!uri.HasQuery(key))
            {
                return false;
            }

            // Get the collection of query keys and values
            NameValueCollection queryValues = uri.ParseQuery();

            // Check that each given value exists
            foreach (string value in values)
            {
                // TODO: This may be slow
                if (!queryValues.GetValues(key).Contains(value))
                {
                    return false;
                }
            }

            // If nothing failed
            return true;
        }

        /// <summary>
        /// Remove any query with the given key
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/></param>
        /// <param name="key">The key to modify</param>
        /// <returns>The modified <see cref="Uri"/></returns>
        public static UriBuilder RemoveQuery(this UriBuilder uri, string key)
        {
            // Get the collection of query keys and values
            NameValueCollection queryValues = uri.ParseQuery();

            // Clear current values for key
            queryValues.Remove(key);

            // Set query
            uri.Query = queryValues.ToString();

            // Add and return
            return uri;
        }

        /// <summary>
        /// Sets the query parameter for a given key
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/></param>
        /// <param name="key">The key to modify</param>
        /// <param name="values">Values to set</param>
        /// <returns>The modified <see cref="Uri"/></returns>
        public static UriBuilder SetQuery(this UriBuilder uri, string key, params string[] values)
        {
            // Remove, add and return
            return uri.RemoveQuery(key).AddQuery(key, values);
        }

        /// <summary>
        /// Adds a query parameter for a given key
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/></param>
        /// <param name="key">The key to modify</param>
        /// <param name="values">Values to set</param>
        /// <returns>The modified <see cref="Uri"/></returns>
        public static UriBuilder AddQuery(this UriBuilder uri, string key, params string[] values)
        {
            // Get the collection of query keys and values
            NameValueCollection queryValues = uri.ParseQuery();

            // Add missing values
            foreach (string value in values)
            {
                if (queryValues.GetValues(key) == null || !queryValues.GetValues(key).Contains(value))
                {
                    queryValues.Add(key, value);
                }
            }

            // Set query
            uri.Query = queryValues.ToString();

            // Return
            return uri;
        }

        /// <summary>
        /// Get all keys in the query
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/></param>
        /// <returns>A collection with all keys in the current query</returns>
        public static ICollection<string> GetQueryKeys(this UriBuilder uri)
        {
            HashSet<string> result = new HashSet<string>();

            NameValueCollection queryValues = uri.ParseQuery();
            foreach (string key in queryValues.AllKeys)
            {
                result.Add(key);
            }

            return result;
        }

        /// <summary>
        /// Get all values for the given key
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/></param>
        /// <param name="key">The key to look up on</param>
        /// <returns>A collection of values or an empty list if the key does not exist</returns>
        public static ICollection<string> GetQueryValues(this UriBuilder uri, string key)
        {
            HashSet<string> result = new HashSet<string>();

            if (!uri.HasQuery(key))
            {
                return result;
            }

            NameValueCollection queryValues = uri.ParseQuery();
            foreach (string value in queryValues.GetValues(key))
            {
                result.Add(value);
            }

            return result;
        }

        /// <summary>
        /// Parses the query string of the <see cref="UriBuilder"/> into a <see cref="NameValueCollection"/>.
        /// </summary>
        static NameValueCollection ParseQuery(this UriBuilder uri)
        {
            return HttpUtility.ParseQueryString(uri.Query);
        }
    }
}
