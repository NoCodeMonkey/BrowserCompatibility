using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;

namespace BrowserCompatibility
{
	/// <summary>
	/// A custom IHttpModule that handles the Browser Checking.
	/// </summary>
	public class BrowserCheckHandler : IHttpModule
	{
		public const string TrackingCookieName = "__BROWSER__COMPATIBILITY__VERIFIED";

		/// <summary>
		/// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
			context.PreSendRequestHeaders += new EventHandler(context_PreSendRequestHeaders);
		}

		/// <summary>
		/// Handles the PreSendRequestHeaders event of the context control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void context_PreSendRequestHeaders(object sender, EventArgs e)
		{
			if (!IsStaticResource(sender))
			{
				HttpApplication application = sender as HttpApplication;
				if (application != null)
				{
					HttpRequest request = application.Request;
					HttpResponse response = application.Response;
					HttpBrowserCapabilities browser = request.Browser;
					if (browser.Cookies && ReadCookie(application.Context, TrackingCookieName) == null)
					{
						HttpCookie trackingCookie = new HttpCookie(TrackingCookieName);
						trackingCookie.Expires = DateTime.Now.AddYears(1);  // make this cookie last a while 
						trackingCookie.HttpOnly = true;
						trackingCookie.Path = "/";
						trackingCookie.Values["LastVisit"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
						response.Cookies.Add(trackingCookie);
					}
				}
			}
		}

		/// <summary>
		/// Handles the BeginRequest event of the context control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void context_BeginRequest(object sender, EventArgs e)
		{
			if (!IsStaticResource(sender))
			{
				HttpApplication application = sender as HttpApplication;
				if (application != null)
				{
					HttpRequest request = application.Request;
					// Check for tracking cookie: 
					if (!IsCrawler(request) && ReadCookie(application.Context, TrackingCookieName) == null)
					{
						string[] urlSegments = request.Url.Segments;
						string pageName = HttpUtility.UrlDecode(urlSegments[urlSegments.Length - 1]).ToLower();

						// Perform Browser Validation
						if (!string.IsNullOrWhiteSpace(pageName) && string.Compare(pageName, "BrowserCompatibility.aspx", StringComparison.OrdinalIgnoreCase) != 0)
						{
							application.Context.RewritePath(string.Format("~/BrowserCompatibility.aspx?ReturnUrl={0}", pageName), true);
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns true if the requested resource is one of the typical resources that needn't be processed.
		/// </summary>
		/// <param name="sender">The event sender, probably a http application.</param>
		/// <returns>
		/// True if the request targets a static resource file.
		/// </returns>
		/// <remarks>
		/// These are the file extensions considered to be static resources:
		/// .css
		/// .gif
		/// .png
		/// .jpg
		/// .jpeg
		/// .js
		/// .axd
		/// .ashx
		/// </remarks>
		protected static bool IsStaticResource(object sender)
		{
			HttpApplication application = sender as HttpApplication;
			if (application != null)
			{
				string path = application.Request.Path;
				string extension = VirtualPathUtility.GetExtension(path);

				if (extension == null) return false;

				switch (extension.ToLower())
				{
					case ".css":
					case ".gif":
					case ".png":
					case ".jpg":
					case ".jpeg":
					case ".bmp":
					case ".ico":
					case ".js":
					case ".axd":
					case ".ashx":
					case ".htm":
					case ".html":
					case ".rar":
					case ".zip":
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified request is crawler.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <remarks>
		/// Warning - this is not fool-proof! If you install certain versions of the Ask.com toolbar (in IE, at least) it will modify the user-agent to include 'Ask' in some form, causing false-positives
		/// </remarks>
		/// <returns>
		///   <c>true</c> if the specified request is crawler; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsCrawler(HttpRequest request)
		{
			// set next line to "bool isCrawler = false; to use this to deny certain bots 
			bool isCrawler = request.Browser.Crawler;

			// Microsoft doesn't properly detect several crawlers 
			if (!isCrawler)
			{
				// put any additional known crawlers in the Regex below 
				// you can also use this list to deny certain bots instead, if desired: 
				// just set bool isCrawler = false; for first line in method  
				// and only have the ones you want to deny in the following Regex list 

				//Regex regEx = new Regex("Google|msnbot|Rambler|Yahoo|AbachoBOT|accoona|AcioRobot|ASPSeek|CocoCrawler|Dumbot|FAST-WebCrawler|GeonaBot|Gigabot|Lycos|MSRBOT|Scooter|AltaVista|IDBot|eStyle|Scrubby|Slurp|Ask|teoma|TECNOSEEK", RegexOptions.IgnoreCase);

				// Get the user agent string from the web.config
				string userAgents = ConfigurationManager.AppSettings["CrawlerUserAgents"];
				if (!string.IsNullOrWhiteSpace(userAgents))
				{
					// Regular expression to identify all robots
					// robot strings need to be separated with "|" in the web.config
					Regex regex = new Regex("(" + userAgents + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);

					// Get the current user agent
					isCrawler = regex.Match(request.UserAgent).Success;
				}
			}
			return isCrawler;
		}

		/// <summary>
		/// Reads the cookie. When writing a cookie, use Response but reading may depend on your situation. Normally, you read from Request but if your application is attempting to get a cookie that has just been written or updated and the round trip to the browser has not occurred, you may need to read it form Response
		/// </summary>
		/// <param name="cookieName">Name of the cookie.</param>
		/// <returns></returns>
		public static string ReadCookie(HttpContext context, string cookieName)
		{
			foreach (string cookie in context.Response.Cookies.AllKeys)
			{
				if (cookie == cookieName)
					return context.Response.Cookies[cookie].Value;
			}
			foreach (string cookie in context.Request.Cookies.AllKeys)
			{
				if (cookie == cookieName)
					return context.Request.Cookies[cookie].Value;
			}
			return null;
		}
	}
}
