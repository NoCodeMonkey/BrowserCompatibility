using System;
using System.Web;
using System.Web.UI;

namespace BrowserCompatibility.Web
{
	public partial class BrowserCompatibility : System.Web.UI.Page
	{
		/// <summary>
		/// Handles the Load event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				BrowserCompatibilityMultiView.SetActiveView(BrowserCheckerView);
			}
		}

		/// <summary>
		/// Handles the BrowserCompatibilityChecked event of the BrowserChecker control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void BrowserChecker_BrowserCompatibilityChecked(object sender, EventArgs e)
		{
			HttpBrowserCapabilities browser = Request.Browser;

			if (BrowserChecker.BrowserJavaScriptStatus == BrowserFeatureStatus.Enabled &&
				BrowserChecker.BrowserCookieStatus == BrowserFeatureStatus.Enabled)
			{
				bool supportedBrowser = false;
				HttpBrowserCapabilities browserCapabilities = Request.Browser;
				switch (browserCapabilities.Browser.ToLower())
				{
					case "ie":
						if (browserCapabilities.MajorVersion >= 9)
							supportedBrowser = true;
						break;
					case "firefox":
						if (browserCapabilities.MajorVersion >= 5)
							supportedBrowser = true;
						break;
					case "chrome":
						if (browserCapabilities.MajorVersion >= 10)
							supportedBrowser = true;
						break;
					case "safari":
						if (browserCapabilities.MajorVersion >= 5)
							supportedBrowser = true;
						break;
					case "opera":
						if (browserCapabilities.MajorVersion >= 12)
							supportedBrowser = true;
						break;
					default:
						break;
				}
				if (supportedBrowser)
					Response.Redirect(GetReturnUrl("Default.aspx"));
				else
					BrowserCompatibilityMultiView.SetActiveView(BrowserCompatibilityWarningView);
			}
			else
				BrowserCompatibilityMultiView.SetActiveView(BrowserCompatibilityWarningView);
		}

		/// <summary>
		/// Handles the Click event of the ContinueButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void ContinueButton_Click(object sender, EventArgs e)
		{
			Response.Redirect(GetReturnUrl("Default.aspx"));
		}

		/// <summary>
		/// checks for a return url specified in the query string
		/// </summary>
		/// <param name="defaultUrl">default value if no url found in query string</param>
		/// <returns>The return url specified, or the default url if none found in query string.</returns>
		public static string GetReturnUrl(string defaultUrl)
		{
			HttpContext context = HttpContext.Current;
			if (context == null)
				return defaultUrl;
			string url = context.Request.QueryString.GetValue<string>("ReturnUrl", defaultUrl); ;
			return url;
		}
	}
}