using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BrowserCompatibility.Web
{
	public partial class Login : System.Web.UI.Page
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
				LoginMultiView.SetActiveView(BrowserCheckerView);
			}
		}

		/// <summary>
		/// Handles the BrowserCompatibilityChecked event of the BrowserChecker control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
		protected void BrowserChecker_BrowserCompatibilityChecked(object sender, EventArgs e)
		{
			if (BrowserChecker.BrowserJavaScriptStatus == BrowserFeatureStatus.Enabled &&
				BrowserChecker.BrowserCookieStatus == BrowserFeatureStatus.Enabled)
			{
				if (Request.Browser != null && !string.IsNullOrEmpty(Request.Browser.Browser))
				{
					bool supportedBrowser = false;
					HttpBrowserCapabilities browserCapabilities = Request.Browser;
					switch (browserCapabilities.Browser.ToLower())
					{
						case "ie":
							if (browserCapabilities.MajorVersion >= 8)
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
						case "icabmobile":
							supportedBrowser = true;
							break;
						default:
							break;
					}
					if (supportedBrowser)
						LoginMultiView.SetActiveView(LoginView);
					else
					{
						ContinuePlaceHolder.Visible = true;
						LoginMultiView.SetActiveView(BrowserCompatibilityWarningView);
						InitBrowserCompabilityInformation();
					}
				}
				else
				{
					ContinuePlaceHolder.Visible = true;
					LoginMultiView.SetActiveView(BrowserCompatibilityWarningView);
					InitBrowserCompabilityInformation();
				}
			}
			else
			{
				ContinuePlaceHolder.Visible = (BrowserChecker.BrowserJavaScriptStatus == BrowserFeatureStatus.Enabled) ? true : false;
				LoginMultiView.SetActiveView(BrowserCompatibilityWarningView);
				InitBrowserCompabilityInformation();
			}
		}

		/// <summary>
		/// Handles the Click event of the ContinueButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
		protected void ContinueButton_Click(object sender, EventArgs e)
		{
			LoginMultiView.SetActiveView(LoginView);
		}

		/// <summary>
		/// Inits the browser compability information.
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		private void InitBrowserCompabilityInformation()
		{
			if (Request.Browser != null)
			{
				HttpBrowserCapabilities browserCapabilities = Request.Browser;
				if (!string.IsNullOrWhiteSpace(browserCapabilities.Browser))
				{
					bool supportedBrowser = false;
					bool oldVersionSupportedBrowser = true;
					switch (browserCapabilities.Browser.ToLower())
					{
						case "ie":
							supportedBrowser = true;
							if (browserCapabilities.MajorVersion >= 9)
								oldVersionSupportedBrowser = false;
							break;
						case "firefox":
							supportedBrowser = true;
							if (browserCapabilities.MajorVersion >= 5)
								oldVersionSupportedBrowser = false;
							break;
						case "chrome":
							supportedBrowser = true;
							if (browserCapabilities.MajorVersion >= 10)
								oldVersionSupportedBrowser = false;
							break;
						case "safari":
							supportedBrowser = true;
							if (browserCapabilities.MajorVersion >= 5)
								oldVersionSupportedBrowser = false;
							break;
						case "opera":
							supportedBrowser = true;
							if (browserCapabilities.MajorVersion >= 12)
								oldVersionSupportedBrowser = false;
							break;
						default:
							BrowserStatusLabel.Text = "Unsupported Browser. Please upgrade to one of our supported browsers.";
							BrowserStatusLabel.CssClass = "failed";
							break;
					}
					BrowserNameLabel.Text = browserCapabilities.Browser;
					BrowserVersionLabel.Text = browserCapabilities.Version;
					if (supportedBrowser)
					{
						BrowserStatusLabel.CssClass = "ok";
						BrowserNameLabel.CssClass = "ok";
						if (oldVersionSupportedBrowser)
						{
							BrowserTestResultsPanel.CssClass = "warning-box";
							BrowserStatusLabel.Text = "You are using an older browser version then we currently support. Our website may not function as expected.";
							BrowserFixPanel.Visible = true;
						}
						else
						{
							BrowserTestResultsPanel.CssClass = "success-box";
							BrowserStatusLabel.Text = "This Browser is supported";
						}
					}
					else
					{
						BrowserTestResultsPanel.CssClass = "error-box";
						BrowserStatusLabel.Text = "Unsupported Browser. Please upgrade to one of our supported browsers.";
						BrowserFixPanel.Visible = true;
					}
				}
				else
				{
					BrowserTestResultsPanel.CssClass = "error-box";
					BrowserStatusLabel.Text = "Unsupported Browser. Please upgrade to one of our supported browsers.";
					BrowserNameLabel.Text = "Unknown";
					BrowserVersionLabel.Text = "Unknown";
					BrowserFixPanel.Visible = true;
				}
			}
			else
			{
				BrowserTestResultsPanel.CssClass = "error-box";
				BrowserStatusLabel.Text = "Unsupported Browser. Please upgrade to one of our supported browsers.";
				BrowserStatusLabel.CssClass = "failed";
				BrowserNameLabel.Text = "Unknown";
				BrowserVersionLabel.Text = "Unknown";
				BrowserFixPanel.Visible = true;
			}
			if (Request.Browser != null && Request.Browser.EcmaScriptVersion.Major >= 1) //A Major version value greater than or equal to 1 implies JavaScript support.
			{
				if (BrowserChecker.BrowserJavaScriptStatus == BrowserFeatureStatus.Enabled)
				{
					JavaScriptTestResultsPanel.CssClass = "success-box";
					JavaScriptStatusLabel.Text = "Your browser supports JavaScript. JavaScript is enabled.";
					JavaScriptVersionLabel.Text = Request.Browser.EcmaScriptVersion.ToString();
				}
				else
				{
					JavaScriptTestResultsPanel.CssClass = "error-box";
					JavaScriptStatusLabel.Text = "Your browser supports JavaScript. JavaScript is disabled.";
					JavaScriptVersionLabel.Text = Request.Browser.EcmaScriptVersion.ToString();
					JavaScriptFixPanel.Visible = true;
				}
			}
			else
			{
				if (BrowserChecker.BrowserJavaScriptStatus == BrowserFeatureStatus.Enabled) //Why can't we detect emacscriptversion???
				{
					JavaScriptTestResultsPanel.CssClass = "success-box";
					JavaScriptStatusLabel.Text = "Your browser supports JavaScript. JavaScript is enabled.";
					JavaScriptVersionLabel.Text = "Unknown";
				}
				else
				{
					JavaScriptTestResultsPanel.CssClass = "error-box";
					JavaScriptStatusLabel.Text = "Your browser does not support JavaScript.";
					JavaScriptVersionLabel.Text = "Unknown";
					JavaScriptFixPanel.Visible = true;
				}
			}

			if (BrowserChecker.BrowserCookieStatus == BrowserFeatureStatus.Enabled)
			{
				CookieTestResultsPanel.CssClass = "success-box";
				CookieStatusLabel.Text = "Your browser supports cookies. Cookies are enabled.";
			}
			else
			{
				CookieStatusLabel.Text = "Your browser is not currently set to accept cookies.";
				CookieFixPanel.Visible = true;
				if (BrowserChecker.BrowserJavaScriptStatus != BrowserFeatureStatus.Enabled)
				{
					CookieTestResultsPanel.CssClass = "warning-box";
					CookieFixIgnoreContentPlaceHolder.Visible = true;
				}
				else
				{
					CookieTestResultsPanel.CssClass = "error-box";
					CookieFixContentPlaceHolder.Visible = true;
				}
			}
		}
	}
}