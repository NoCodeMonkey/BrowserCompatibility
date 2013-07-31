using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace BrowserCompatibility.Web
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("User Agent: {0}{1}", Request.ServerVariables["http_user_agent"].ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Browser: {0}{1}", Request.Browser.Browser.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Version: {0}{1}", Request.Browser.Version.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Major Version: {0}{1}", Request.Browser.MajorVersion.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Minor Version: {0}{1}", Request.Browser.MinorVersion.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Platform: {0}{1}", Request.Browser.Platform.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("ECMA Script version: {0}{1}", Request.Browser.EcmaScriptVersion.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Type: {0}{1}", Request.Browser.Type.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("-------------------------------------------------------------------------------{0}", Environment.NewLine);
			stringBuilder.AppendFormat("ActiveX Controls: {0}{1}", Request.Browser.ActiveXControls.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Background Sounds: {0}{1}", Request.Browser.BackgroundSounds.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("AOL: {0}{1}", Request.Browser.AOL.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Beta: {0}{1}", Request.Browser.Beta.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("CDF: {0}{1}", Request.Browser.CDF.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("ClrVersion: {0}{1}", Request.Browser.ClrVersion.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Cookies: {0}{1}", Request.Browser.Cookies.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Crawler: {0}{1}", Request.Browser.Crawler.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Frames: {0}{1}", Request.Browser.Frames.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Tables: {0}{1}", Request.Browser.Tables.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("JavaApplets: {0}{1}", Request.Browser.JavaApplets.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("JavaScript: {0}{1}", Request.Browser.EcmaScriptVersion.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("MSDomVersion: {0}{1}", Request.Browser.MSDomVersion.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("TagWriter: {0}{1}", Request.Browser.TagWriter.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("VBScript: {0}{1}", Request.Browser.VBScript.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("W3CDomVersion: {0}{1}", Request.Browser.W3CDomVersion.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Win16: {0}{1}", Request.Browser.Win16.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("Win32: {0}{1}", Request.Browser.Win32.ToString(), Environment.NewLine);
			stringBuilder.AppendFormat("-------------------------------------------------------------------------------{0}", Environment.NewLine);
			stringBuilder.AppendFormat("MachineName: {0}{1}", Environment.MachineName, Environment.NewLine);
			stringBuilder.AppendFormat("OSVersion: {0}{1}", Environment.OSVersion, Environment.NewLine);
			stringBuilder.AppendFormat("ProcessorCount: {0}{1}", Environment.ProcessorCount, Environment.NewLine);
			stringBuilder.AppendFormat("UserName: {0}{1}", Environment.UserName, Environment.NewLine);
			stringBuilder.AppendFormat("Version: {0}{1}", Environment.Version, Environment.NewLine);
			stringBuilder.AppendFormat("UserInteractive: {0}{1}", Environment.UserInteractive, Environment.NewLine);
			stringBuilder.AppendFormat("UserDomainName: {0}{1}", Environment.UserDomainName, Environment.NewLine);
			BrowserInformation.Text = stringBuilder.ToString().Replace(Environment.NewLine, "<br />");

			var propInfo = typeof(BrowserCapabilitiesFactory).GetProperty("BrowserElements", BindingFlags.NonPublic | BindingFlags.Instance);
			Hashtable browserDefinitions = (Hashtable)propInfo.GetValue(new BrowserCapabilitiesFactory(), null);

			stringBuilder = new StringBuilder();
			foreach (var key in browserDefinitions.Keys)
			{
				stringBuilder.AppendFormat("{0}{1}", key, Environment.NewLine);
			}
			BrowserCapabilities.Text = stringBuilder.ToString().Replace(Environment.NewLine, "<br />");
		}

		/// <summary>
		/// Gets the browser capabilities.
		/// </summary>
		/// <param name="userAgent">The user agent.</param>
		/// <param name="headers">The headers.</param>
		/// <returns></returns>
		public HttpBrowserCapabilities GetBrowserCapabilities(string userAgent, NameValueCollection headers)
		{
			HttpBrowserCapabilities browserCaps = new HttpBrowserCapabilities();
			Hashtable hashtable = new Hashtable(180, StringComparer.OrdinalIgnoreCase);
			hashtable[string.Empty] = userAgent; // The actual method uses client target
			browserCaps.Capabilities = hashtable;

			var capsFactory = new System.Web.Configuration.BrowserCapabilitiesFactory();
			capsFactory.ConfigureBrowserCapabilities(headers, browserCaps);
			capsFactory.ConfigureCustomCapabilities(headers, browserCaps);
			return browserCaps;
		}
	}
}