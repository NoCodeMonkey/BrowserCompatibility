using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BrowserCompatibility
{
	/// <summary>
	/// Version / Feature Browser Checker
	/// </summary>
	[DefaultEvent("BrowserCompatibilityChecked"),
	DefaultProperty("BrowserCheckMessage"),
	AspNetHostingPermission(System.Security.Permissions.SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
	AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
	ToolboxData(@"<{0}:BrowserChecker runat=""server"" \>")]
	public class BrowserChecker : CompositeControl
	{
		#region Fields

		private Literal browserCheckWarningLiteral;
		private AccessibleLinkButton browserCheckLinkButton;
		private Literal browserCheckLinkLiteral1;
		private Literal browserCheckLinkLiteral2;
		private HiddenField javascriptEnabledHiddenField;
		private HtmlGenericControl div;
		private Button submitButton;
		private BrowserFeatureStatus browserCookieStatus = BrowserFeatureStatus.Unknown;
		private BrowserFeatureStatus browserJavaScriptStatus = BrowserFeatureStatus.Unknown;
		private const string checkValue = "1";
		private const string cookieName = "ChildCarersCookieCheck31415";
		private static readonly object eventBrowserCompatibilityCheckedKey = new object();
		private string browserCheckMessage = "Please wait while we detect your browser compatibility. You will be redirected automatically in 2 seconds.";
		private string browserCheckLinkButtonText = "If this does not occur, please {0}.";
		private string browserCheckLinkButtonClickText = "click this link";

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets or sets the browser check message.
		/// </summary>
		/// <value>
		/// The browser check message.
		/// </value>
		[DefaultValue(BrowserFeatureStatus.Unknown)]
		[Bindable(true)]
		[Category("Appearance")]
		public string BrowserCheckMessage
		{
			get { return browserCheckMessage; }
			set { browserCheckMessage = value; }
		}

		/// <summary>
		/// Gets or sets the browser cookie status.
		/// </summary>
		/// <value>
		/// The browser cookie status.
		/// </value>
		[DefaultValue(BrowserFeatureStatus.Unknown)]
		[Bindable(true)]
		[Category("Default")]
		public BrowserFeatureStatus BrowserCookieStatus
		{
			get { return browserCookieStatus; }
			set { browserCookieStatus = value; }
		}

		/// <summary>
		/// Gets or sets the browser java script status.
		/// </summary>
		/// <value>
		/// The browser java script status.
		/// </value>
		[DefaultValue(BrowserFeatureStatus.Unknown)]
		[Bindable(true)]
		[Category("Default")]
		public BrowserFeatureStatus BrowserJavaScriptStatus
		{
			get { return browserJavaScriptStatus; }
			set { browserJavaScriptStatus = value; }
		}

		/// <summary>
		/// Gets or sets the browser link button text.
		/// </summary>
		/// <value>
		/// The browser link button text.
		/// </value>
		[DefaultValue("If this does not occur, please {0}.")]
		[Bindable(true)]
		[Category("Appearance")]
		public string BrowserCheckLinkButtonText
		{
			get { return browserCheckLinkButtonText; }
			set { browserCheckLinkButtonText = value; }
		}

		/// <summary>
		/// Gets or sets the browser link button click text.
		/// </summary>
		/// <value>
		/// The browser link button click text.
		/// </value>
		[DefaultValue("click this link")]
		[Bindable(true)]
		[Category("Appearance")]
		public string BrowserCheckLinkButtonClickText
		{
			get { return browserCheckLinkButtonClickText; }
			set { browserCheckLinkButtonClickText = value; }
		}

		#endregion Properties

		#region Methods

		#region Protected Methods

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!this.Page.IsPostBack)
			{
				bool noscript = false;
				bool.TryParse(Context.Request.QueryString["noscript"], out noscript);
				if (noscript)
				{
					browserJavaScriptStatus = BrowserFeatureStatus.Disabled;
					browserCookieStatus = BrowserFeatureStatus.Unknown;

					OnBrowserCompatibilityChecked(EventArgs.Empty);
				}
				else
				{
					//string url = HttpContext.Current.Request.Url.ToString(); DOESNT WORK IN AZURE
					//string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Headers["Host"] + HttpContext.Current.Request.Url.PathAndQuery;
					string url = GetRealRequestUri().ToString();
					string newUrl = "";
					if (url.IndexOf("?") >= 0)
					{
						newUrl = url.Substring(0, url.IndexOf("?"));
						string queryString = url.Substring(url.IndexOf("?"));
						NameValueCollection parameters = HttpUtility.ParseQueryString(queryString);
						if (parameters != null && parameters.Count > 0)
						{
							if (parameters.AllKeys.Contains("noscript"))
								parameters.Remove("noscript");
							if (parameters.Count > 0)
							{
								newUrl = newUrl + "?noscript=true&";
								newUrl = newUrl + String.Join("&", parameters.AllKeys.SelectMany(key => parameters.GetValues(key).Select(value => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))).ToArray());
							}
							else
							{
								newUrl = newUrl + "?noscript=true";
							}
						}
						else
						{
							newUrl = newUrl + "?noscript=true";
						}
					}
					else
					{
						newUrl = url + "?noscript=true";
					}
					HtmlGenericControl ctrl = new HtmlGenericControl("NOSCRIPT");
					ctrl.InnerHtml = string.Format("<meta http-equiv=REFRESH content=0;URL={0}>", newUrl);
					Page.Header.Controls.Add(ctrl);
				}
			}
		}

		/// <summary>
		/// Recreates the child controls.
		/// </summary>
		protected override void RecreateChildControls()
		{
			EnsureChildControls();
		}

		/// <summary>
		/// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			Controls.Clear();
			browserCheckWarningLiteral = new Literal();
			browserCheckWarningLiteral.Text = browserCheckMessage;

			var browserLinkText = string.Format(browserCheckLinkButtonText, browserCheckLinkButtonClickText);
			// Starting at the start of the source text (this could be easily placed with RegEx) 
			int currentPosition = 0;
			// While the source text still contains more instances of the string we're searching for 
			if (browserLinkText.IndexOf(browserCheckLinkButtonClickText, currentPosition) != -1)
			{
				// Grab the position of the required string 
				int keyPosition = browserLinkText.IndexOf(browserCheckLinkButtonClickText, currentPosition);

				browserCheckLinkLiteral1 = new Literal();
				browserCheckLinkLiteral1.Text = browserLinkText.Substring(currentPosition, keyPosition - currentPosition);

				// Create the required link button and add it to the place holder 
				browserCheckLinkButton = new AccessibleLinkButton();
				browserCheckLinkButton.ID = "BrowserCheckWarningLinkButton";
				browserCheckLinkButton.Text = browserCheckLinkButtonClickText;
				browserCheckLinkButton.Click += new EventHandler(BrowserCheckLinkButton_Click);


				// Update the current search start position to be the end of the current required string instance 
				currentPosition = keyPosition + browserCheckLinkButtonClickText.Length;
			}
			// If there's any content left which we haven't outputted then do it now 
			if (currentPosition < browserLinkText.Length - 1)
			{
				browserCheckLinkLiteral2 = new Literal();
				browserCheckLinkLiteral2.Text = browserLinkText.Substring(currentPosition);
			}

			javascriptEnabledHiddenField = new HiddenField();
			javascriptEnabledHiddenField.ID = "JavascriptEnabled";
			javascriptEnabledHiddenField.Value = string.Empty;
			div = new HtmlGenericControl("div");
			div.Attributes.Add("style", "display:none");
			submitButton = new Button();
			submitButton.ID = "SubmitButton";
			submitButton.Click += new EventHandler(SubmitButton_Click);
			this.Controls.Add(browserCheckWarningLiteral);
			if (browserCheckLinkLiteral1 != null)
				this.Controls.Add(browserCheckLinkLiteral1);
			if (browserCheckLinkButton != null)
				this.Controls.Add(browserCheckLinkButton);
			if (browserCheckLinkLiteral2 != null)
				this.Controls.Add(browserCheckLinkLiteral2);
			this.Controls.Add(javascriptEnabledHiddenField);
			div.Controls.Add(submitButton);
			this.Controls.Add(div);
		}

		/// <summary>
		/// Handles the Click event of the BrowserCheckLinkButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void BrowserCheckLinkButton_Click(object sender, EventArgs e)
		{
			CheckBrowserCompatibility();
		}

		/// <summary>
		/// Handles the Click event of the SubmitButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void SubmitButton_Click(object sender, EventArgs e)
		{
			CheckBrowserCompatibility();
		}

		/// <summary>
		/// Occurs when browser compatibility has been checked.
		/// </summary>
		[Category("Action"), Description("Raised when the the browser compatibility is validated.")]
		public event EventHandler BrowserCompatibilityChecked
		{
			add
			{
				Events.AddHandler(eventBrowserCompatibilityCheckedKey, value);
			}
			remove
			{
				Events.RemoveHandler(eventBrowserCompatibilityCheckedKey, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Submit"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void OnBrowserCompatibilityChecked(EventArgs e)
		{
			EventHandler BrowserCompatibilityCheckedHandler = (EventHandler)Events[eventBrowserCompatibilityCheckedKey];
			if (BrowserCompatibilityCheckedHandler != null)
			{
				BrowserCompatibilityCheckedHandler(this, e);
			}
		}

		/// <summary>
		/// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			//PostBackOptions pbo = new PostBackOptions(this);
			browserCheckWarningLiteral.RenderControl(writer);
			writer.WriteLine();
			if (browserCheckLinkLiteral1 != null)
			{
				browserCheckLinkLiteral1.RenderControl(writer);
				writer.WriteLine();
			}
			if (browserCheckLinkButton != null)
			{
				browserCheckLinkButton.RenderControl(writer);
				writer.WriteLine();
			}
			if (browserCheckLinkLiteral2 != null)
			{
				browserCheckLinkLiteral2.RenderControl(writer);
				writer.WriteLine();
			}
			javascriptEnabledHiddenField.RenderControl(writer);
			writer.WriteLine();
			div.RenderControl(writer);
			writer.WriteLine();
			//At Design Mode, by pass the following code. Otherwise
			// when calling SetCookie there'll be error in design view,
			// because the HttpContext is not available in this case.
			if (DesignMode)
				return;

			if (!this.Page.IsPostBack)
			{
				RegisterScript();
				SetCookie();
			}
		}

		#endregion Protected Methods

		#region Private Methods

		/// <summary>
		/// Registers the script.
		/// </summary>
		private void RegisterScript()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<script type=\"text/javascript\">");
			stringBuilder.AppendLine("// <!--");
			stringBuilder.AppendLine("$(document).ready(function() {");
			stringBuilder.AppendFormat("	$(\"#{0}\").val('{1}');{2}", javascriptEnabledHiddenField.ClientID, checkValue, Environment.NewLine);
			stringBuilder.AppendFormat("	$(\"#{0}\").click();{1}", submitButton.ClientID, Environment.NewLine);
			stringBuilder.AppendLine("});");
			stringBuilder.AppendLine("// -->");
			stringBuilder.AppendLine("</script>");
			this.Page.ClientScript.RegisterStartupScript(this.GetType(), "BROWSER_CHECKER", stringBuilder.ToString(), false);
		}

		/// <summary>
		/// Sets the cookie.
		/// </summary>
		private void SetCookie()
		{
			Context.Response.Cookies[cookieName].Value = checkValue;
		}

		/// <summary>
		/// Checks the browser compatibility.
		/// </summary>
		private void CheckBrowserCompatibility()
		{
			string javascriptEnabled = Context.Request.Form[javascriptEnabledHiddenField.UniqueID];
			browserJavaScriptStatus = (!string.IsNullOrWhiteSpace(javascriptEnabled) && string.Compare(javascriptEnabled, checkValue, StringComparison.OrdinalIgnoreCase) == 0) ? BrowserFeatureStatus.Enabled : BrowserFeatureStatus.Disabled;

			HttpCookie cookie = Context.Request.Cookies[cookieName];
			browserCookieStatus = (cookie != null && string.Compare(cookie.Value, checkValue, StringComparison.OrdinalIgnoreCase) == 0) ? BrowserFeatureStatus.Enabled : BrowserFeatureStatus.Disabled;

			OnBrowserCompatibilityChecked(EventArgs.Empty);
		}

		/// <summary>
		/// Gets the real request URI.
		/// </summary>
		/// <returns></returns>
		private static Uri GetRealRequestUri()
		{
			if ((HttpContext.Current == null) || (HttpContext.Current.Request == null))
				throw new ApplicationException("Cannot get current request.");
			return GetRealRequestUri(HttpContext.Current.Request);
		}

		/// <summary>
		/// Gets the real request URI.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		private static Uri GetRealRequestUri(HttpRequest request)
		{
			if (String.IsNullOrEmpty(request.Headers["Host"]))
				return request.Url;
			UriBuilder ub = new UriBuilder(request.Url);
			string[] realHost = request.Headers["Host"].Split(':');
			string host = realHost[0]; ub.Host = host; string portString = realHost.Length > 1 ? realHost[1] : "";
			int port;
			if (int.TryParse(portString, out port))
				ub.Port = port;
			return ub.Uri;
		}

		#endregion Private Methods

		#endregion Methods
	}
}
