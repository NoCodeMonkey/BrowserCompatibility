using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrowserCompatibility
{
	/// <summary>
	/// Alternative to LinkButton which is more accessible in that it will
	/// fall back to a Button, which does not require javascript for
	/// postbacks, if client does not support javascript.
	/// Composite control consisting of a LinkButton and a Button, where
	/// the LinkButton will be hidden at render time (element style display
	/// set to "none") and made visible again by javascript in the client
	/// (element display set to the value stored in LinkVisibleDisplayStyle).
	/// The noscript section instead displays the Button (while the
	/// LinkButton remains hidden.
	/// The public properties (like CommandArgument and CssClass) affect both
	/// the LinkButton and the Button. The events (Click and Command) reflect
	/// the corresponding events of both the Button and the LinkButton.
	/// </summary>
	[
	DefaultProperty("Text"),
	DefaultEvent("Click"),
	ParseChildren(true, "Text"),
	ToolboxData("<{0}:AccessibleLinkButton id=\"accessibleLinkButton\" runat=\"server\"></{0}:AccessibleLinkButton>"),
	ToolboxItem(true)
	]
	public class AccessibleLinkButton : Control
	{
		#region Fields

		private Button _button;
		private LinkButton _link;
		private string _linkVisibleDisplayStyle;
		private Literal _scriptContainer;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AccessibleLinkButton"/> class.
		/// </summary>
		public AccessibleLinkButton()
		{
			// Default value
			this.LinkVisibleDisplayStyle = "inline";

			// Create members
			this.Link = new LinkButton();
			this.Button = new Button();
			this.ScriptContainer = new Literal();

			// Set Link to hidden as default
			this.Link.Attributes.CssStyle["display"] = "none";

			// Register event proxies
			this.Link.Click += new EventHandler(EventProxy_Click);
			this.Button.Click += new EventHandler(EventProxy_Click);
			this.Link.Command += new CommandEventHandler(EventProxy_Command);
			this.Button.Command += new CommandEventHandler(EventProxy_Command);

			// Build control tree
			Literal html; // Used to inject html

			this.Controls.Add(this.Link);

			html = new Literal();
			html.Text = "<script type=\"text/javascript\" language=\"javascript\">";
			this.Controls.Add(html);

			this.Controls.Add(this.ScriptContainer); // Contents of ScriptContainer is set later

			html = new Literal();
			html.Text = "</script><noscript>";
			this.Controls.Add(html);

			this.Controls.Add(this.Button);

			html = new Literal();
			html.Text = "</noscript>";
			this.Controls.Add(html);
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Gets or sets the access key.
		/// </summary>
		/// <value>
		/// The access key.
		/// </value>
		[
		Category("Accessibility"),
		DefaultValue(""),
		Description("Gets or sets the keyboard shortcut key (AccessKey) for setting focus to the Web server control.")
		]
		public string AccessKey
		{
			get
			{
				return this.Link.AccessKey;
			}
			set
			{
				this.Button.AccessKey = this.Link.AccessKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the button.
		/// </summary>
		/// <value>
		/// The button.
		/// </value>
		protected Button Button
		{
			get { return _button; }
			set { _button = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether causes validation to fire.
		/// </summary>
		/// <value>
		///   <c>true</c> to causes validation to fire; otherwise, <c>false</c>.
		/// </value>
		[
		Category("Behavior"),
		DefaultValue(true),
		Description("Whether button causes validation to fire.")
		]
		public bool CausesValidation
		{
			get
			{
				return this.Link.CausesValidation;
			}
			set
			{
				this.Button.CausesValidation = this.Link.CausesValidation = value;
			}
		}

		/// <summary>
		/// Gets or sets the command argument.
		/// </summary>
		/// <value>
		/// The command argument.
		/// </value>
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The command argument associated with the button.")
		]
		public string CommandArgument
		{
			get
			{
				return this.Link.CommandArgument;
			}
			set
			{
				this.Button.CommandArgument = this.Link.CommandArgument = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the command.
		/// </summary>
		/// <value>
		/// The name of the command.
		/// </value>
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The command associated with the button.")
		]
		public string CommandName
		{
			get
			{
				return this.Link.CommandName;
			}
			set
			{
				this.Button.CommandName = this.Link.CommandName = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class.
		/// </summary>
		/// <value>
		/// The CSS class.
		/// </value>
		[
		Category("Appearance"),
		DefaultValue(""),
		Description("Applies a CSS style to the Button.")
		]
		public string CssClass
		{
			get { return Link.CssClass; }
			set { Button.CssClass = Link.CssClass = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AccessibleLinkButton"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		[
		Category("Behavior"),
		DefaultValue(true),
		Description("Enabled state of the control.")
		]
		public bool Enabled
		{
			get
			{
				return this.Link.Enabled;
			}
			set
			{
				this.Button.Enabled = this.Link.Enabled = value;
			}
		}

		/// <summary>
		/// Gets or sets the link.
		/// </summary>
		/// <value>
		/// The link.
		/// </value>
		protected LinkButton Link
		{
			get { return _link; }
			set { _link = value; }
		}

		/// <summary>
		/// The value of the display attribute in the linkbutton when it is
		/// visible (when hidden, display="none"), defaults to "inline"
		/// </summary>
		/// <value>
		/// The link visible display style.
		/// </value>
		public string LinkVisibleDisplayStyle
		{
			get { return _linkVisibleDisplayStyle; }
			set { _linkVisibleDisplayStyle = value; }
		}

		/// <summary>
		/// Gets or sets the on client click.
		/// </summary>
		/// <value>
		/// The on client click.
		/// </value>
		[
		Category("Behavior"),
		DefaultValue(""),
		Description(".")
		]
		public string OnClientClick
		{
			get
			{
				return this.Link.OnClientClick;
			}
			set
			{
				this.Button.OnClientClick = this.Link.OnClientClick = value;
			}
		}

		/// <summary>
		/// Gets or sets the post back URL.
		/// </summary>
		/// <value>
		/// The post back URL.
		/// </value>
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The URL to post to when the button is clicked.")
		]
		public string PostBackUrl
		{
			get
			{
				return this.Link.PostBackUrl;
			}
			set
			{
				this.Button.PostBackUrl = this.Link.PostBackUrl = value;
			}
		}

		/// <summary>
		/// Gets or sets the script container.
		/// </summary>
		/// <value>
		/// The script container.
		/// </value>
		protected Literal ScriptContainer
		{
			get { return _scriptContainer; }
			set { _scriptContainer = value; }
		}

		/// <summary>
		/// Gets or sets the index of the tab.
		/// </summary>
		/// <value>
		/// The index of the tab.
		/// </value>
		public short TabIndex
		{
			get
			{
				return this.Link.TabIndex;
			}
			set
			{
				this.Button.TabIndex = this.Link.TabIndex = value;
			}
		}

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>
		/// The text.
		/// </value>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Description("The text to be shown for the link."),
		Localizable(true),
		PersistenceMode(PersistenceMode.InnerDefaultProperty)
		]
		public string Text
		{
			get
			{
				return Link.Text;
			}
			set
			{
				Button.Text = Link.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the tool tip.
		/// </summary>
		/// <value>
		/// The tool tip.
		/// </value>
		public string ToolTip
		{
			get
			{
				return this.Link.ToolTip;
			}
			set
			{
				this.Button.ToolTip = this.Link.ToolTip = value;
			}
		}

		/// <summary>
		/// Gets or sets the validation group.
		/// </summary>
		/// <value>
		/// The validation group.
		/// </value>
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The group that should be validated when the control causes a postback")
		]
		public string ValidationGroup
		{
			get
			{
				return this.Link.ValidationGroup;
			}
			set
			{
				this.Button.ValidationGroup = this.Link.ValidationGroup = value;
			}
		}

		#endregion Properties

		#region Delegates and Events

		#region Events

		/// <summary>
		/// Raised whenever the Click event of the contained Linkbutton or
		/// Button is raised, forwarding the EventArgs
		/// </summary>
		[
		Category("Action"),
		Description("Fires when the button is clicked.")
		]
		public event EventHandler Click;

		/// <summary>
		/// Raised whenever the Command event of the contained Linkbutton or
		/// Button is raised, forwarding the CommandEventArgs.
		/// </summary>
		[
		Category("Action"),
		Description("Fires when the button is clicked and an associated command is defined.")
		]
		public event CommandEventHandler Command;

		#endregion Events

		#endregion Delegates and Events

		#region Methods

		#region Protected Methods

		/// <summary>
		/// Handles the Click event of the EventProxy control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void EventProxy_Click(object sender, EventArgs e)
		{
			if (this.Click != null)
			{
				// Forward event
				this.Click(this, e);
			}
		}

		/// <summary>
		/// Handles the Command event of the EventProxy control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Web.UI.WebControls.CommandEventArgs"/> instance containing the event data.</param>
		protected void EventProxy_Command(object sender, CommandEventArgs e)
		{
			if (this.Command != null)
			{
				// Forward event
				this.Command(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			// Insert the script which shows the linkbutton
			string script = "var obj = document.getElementById('{0}'); obj.style.display='{1}';";
			this.ScriptContainer.Text = String.Format(script, Link.ClientID, LinkVisibleDisplayStyle);
		}

		#endregion Protected Methods

		#endregion Methods
	}
}
