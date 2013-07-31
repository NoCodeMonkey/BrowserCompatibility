using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace BrowserCompatibility.Web
{
	public static class Extensions
	{
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="existingValue">The existing value.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static T GetValue<T>(this NameValueCollection collection, ref T existingValue, string key, T defaultValue)
		{
			if (existingValue.Equals(default(T)))
				existingValue = GetValue(collection, key, defaultValue);

			return existingValue;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static T GetValue<T>(this NameValueCollection collection, string key, T defaultValue)
		{
			if (collection == null)
			{
				return defaultValue;
			}

			string value = collection[key];
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}

			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
			if (converter == null || !converter.CanConvertTo(typeof(string)))
			{
				return defaultValue;
			}

			try
			{
				return (T)converter.ConvertFrom(value);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="existingValue">The existing value.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static T GetValue<T>(this NameValueCollection collection, ref T existingValue, string key)
		{
			if (existingValue.Equals(default(T)))
				existingValue = GetValue<T>(collection, key);

			return existingValue;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static T GetValue<T>(this NameValueCollection collection, string key)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}

			string value = collection[key];

			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentOutOfRangeException("key");
			}

			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

			if (converter == null || !converter.CanConvertTo(typeof(string)))
			{
				throw new ArgumentException(String.Format("Cannot convert '{0}' to {1}", value, typeof(T)));
			}

			try
			{
				return (T)converter.ConvertFrom(value);
			}
			catch (Exception)
			{
				throw new ArgumentException(String.Format("Failed convert '{0}' to {1}", value, typeof(T)));
			}
		}
	}
}