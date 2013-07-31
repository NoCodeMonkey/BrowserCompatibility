<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BrowserCompatibility.Web.Login" %>
<%@ Register Assembly="BrowserCompatibility" Namespace="BrowserCompatibility" TagPrefix="BT" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.0.3.min.js"></script>
	<script>window.jQuery || document.write('<script src="js/jquery-2.0.3.min.js">\x3C/script>')</script>
	<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js"></script>
	<link type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/themes/redmond/jquery-ui.css" rel="Stylesheet" /> 
	<script type="text/javascript">
	<!--
	$(document).ready(function () {
		$("a[target='_blank']").click(function (e) {
			var targetUrl = $(this).attr("href");
			e.stopPropagation();
			e.preventDefault();
			$('<div />').html('Please note: you are navigating to an external site that may not be up to date.').dialog({
				autoOpen: true,
				buttons:
				{
					"Ok": function () {
						$(this).dialog('close');
						window.open(targetUrl);
					},
					"Cancel": function () {
						$(this).dialog("close");
					}
				}
			});
		});
	});
	//-->
	</script>
	<style type="text/css">
		.success-box, .warning-box, .error-box {
			margin-bottom: 10px;
			padding: 10px;
			position: relative;
			overflow: hidden;
			min-height: 34px;
			padding-left: 60px;
		}
		.success-box .icon, .warning-box .icon, .error-box .icon {
			background-image: url(/images/sprite-main.png);
			background-position: left center;
			position: absolute;
			left: 10px;
			top: 10px;
			width: 32px;
			height: 32px;
		}
		.success-box {
			border: 1px solid #9ABF86;
			background-color: #D7F3BD;
			display: block;
		}
		.success-box .icon {
			background-position: -144px -64px;
		}
		.warning-box {
			border: 1px solid #FCED3B;
			background-color: #FDF9CE;
		}
		.warning-box .icon {
			background-position: -144px -32px;
		}
		.error-box {
			border: 1px solid #F16166;
			background-color: #FBCDCE;
			color: #333333 !important;
		}
		.error-box .icon {
			background-position: -144px 0px;
		}
		h3.browser-support {
			margin-bottom: 10px;
		}
		.browser-icons li {
			border-bottom-color: #CCCCCC;
			border-bottom-style: solid;
			border-bottom-width: 1px;
			clear: both;
			height: 35px;
			list-style-image: none;
			list-style-position: outside;
			list-style-type: none;
			margin-bottom: 0;
			margin-left: 0;
			margin-right: 0;
			margin-top: 0;
		}
		.browser-icons li span, .browser-icons li p {
			display: block;
			float: left;
			line-height: 27px;
			margin-top: 5px;
		}
		.browser-icons li span {
			margin-right: 10px;
		}
		.browser-icons img {
			background-image: url("images/browsers.png");
		}
		.browser-icons .last {
			border-bottom-color: -moz-use-text-color;
			border-bottom-style: none;
			border-bottom-width: medium;
		}
		.browser-icons .browser-ie img {
			background-position: 0px 0px;
		}
		.browser-icons .browser-ff img {
			background-position: -27px 0px;
		}
		.browser-icons .browser-chrome img {
			background-position: -54px 0px;
		}
		.browser-icons .browser-safari img {
			background-position: -81px 0px;
		}
		.browser-icons .browser-opera img {
			background-position: -108px 0px;
		}
		.notice {
			-moz-border-bottom-colors:none;
			-moz-border-image:none;
			-moz-border-left-colors:none;
			-moz-border-right-colors:none;
			-moz-border-top-colors:none;
			border-style:solid;
			border-width:2px;
			border-color:#FFD324;
			border-top-width:2px;
			margin-bottom:1em;
			background-attachment:scroll;
			background-clip:border-box;
			background-color:#FFF6BF;
			background-image:none;
			background-origin:padding-box;
			background-position:0 0;
			background-repeat:repeat;
			background-size:auto auto;
			border-bottom-color:#FFD324;
			color:#514721;
			padding:.8em;
			}
		.notice a {
			-moz-text-blink: none;
			-moz-text-decoration-color: -moz-use-text-color;
			-moz-text-decoration-line: underline;
			-moz-text-decoration-style: solid;
			color: #514721;
		}
		.clearfix:after {
			clear: both;
			content: ".";
			display: block;
			font-size: 0;
			height: 0;
			visibility: hidden;
		}
		.clearfix {
		}
		.retry-button {
			display: inline-block;
			padding: 20px 0px;
			text-decoration: none;
			color: #666666;
			font-size: 1.75em;
			width: 300px;
			border: 1px solid #FBB317;
			background-color: #FCCC51;
			text-align: center;
			-webkit-border-radius: 5px;
			-moz-border-radius: 5px;
			border-radius: 5px;
		}
	</style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:MultiView ID="LoginMultiView" runat="server">
		<asp:View ID="BrowserCheckerView" runat="server">
			<BT:BrowserChecker ID="BrowserChecker" runat="server" OnBrowserCompatibilityChecked="BrowserChecker_BrowserCompatibilityChecked"/>
		</asp:View>
		<asp:View ID="BrowserCompatibilityWarningView" runat="server">
			<h1>Browser Compatibility Notice</h1>
			<p>Your browser failed some of our compatibility tests. These tests are used to verify that your web browser settings are valid for using specific features of our web site. If you can see a red cross below, it indicates a test failure and our site may not function correctly.</p>
			<p>To continue, resolve each issue by following the solution presented and then click the "Retest My Settings" button towards the bottom of the page.</p>	
			<asp:PlaceHolder ID="ContinuePlaceHolder" runat="server" Visible="false">
				<div class="notice">
					<span>The browser you are using <strong>might not be compatible</strong> with this application.</span>
					<asp:Button ID="ContinueButton" runat="server" UseSubmitBehavior="true"  Text="Continue Anyway" OnClick="ContinueButton_Click" />
				</div>
			</asp:PlaceHolder>
			<h2>Browser Test Results</h2>
			<asp:Panel ID="BrowserTestResultsPanel" runat="server">
				<div class="icon"></div>
				<h2>Browser Test Results</h2>
			</asp:Panel>
			<table border="0">
				<tr>
					<td style="width: 120px">Status</td>
					<td><asp:Label ID="BrowserStatusLabel" runat="server" /></td>
				</tr>
				<tr>
					<td>Browser Name</td>
					<td><asp:Label ID="BrowserNameLabel" runat="server" /></td>
				</tr>
				<tr>
					<td>Browser Version</td>
					<td><asp:Label ID="BrowserVersionLabel" runat="server" /></td>
				</tr>
			</table>
			<asp:Panel ID="BrowserFixPanel" runat="server" Visible="false">
				<br />
				<strong>How to resolve this issue:</strong>
				<p>Below is a list of browsers we support although we recommend using Google Chrome. Click the link to access the download for the browser.</p>
				<ol class="browser-icons">
					<li class="browser-chrome clearfix">
						<span><img width="27" height="27" src="/images/blank.gif" alt="Google Chrome" /></span>
						<p><a href="http://www.google.com/chrome" target="_blank">Google Chrome</a> 10 and above</p>
					</li>
					<li class="browser-ff clearfix">
						<span><img width="27" height="27" src="/images/blank.gif" alt="Firefox" /></span>
						<p><a href="http://www.getfirefox.net/" target="_blank">Firefox</a> 5 and above</p>
					</li>
					<li class="browser-safari clearfix">
						<span><img width="27" height="27" src="/images/blank.gif" alt="Safari" /></span>
						<p><a href="http://www.apple.com/safari/download/" target="_blank">Safari</a> 5 and above</p>
					</li>
					<li class="browser-ie clearfix">
						<span><img width="27" height="27" src="/images/blank.gif" alt="Internet Explorer" /></span>
						<p><a href="http://www.microsoft.com/windows/internet-explorer/default.aspx" target="_blank">Internet Explorer</a> 9 and above</p>
					</li>
					<li class="browser-opera clearfix last">
						<span><img width="27" height="27" src="/images/blank.gif" alt="Opera" /></span>
						<p><a href="http://www.opera.com/download/" target="_blank">Opera</a> 12 and above</p>
					</li>
				</ol>
				<br />
			</asp:Panel>
			<br />
			<asp:Panel ID="JavaScriptTestResultsPanel" runat="server">
				<div class="icon"></div>
				<h2>JavaScript Test Results</h2>
			</asp:Panel>
			<table border="0">
				<tr>
					<td style="width: 120px">Status</td>
					<td><asp:Label ID="JavaScriptStatusLabel" runat="server" /></td>
				</tr>
				<tr>
					<td>Version</td>
					<td><asp:Label ID="JavaScriptVersionLabel" runat="server" /></td>
				</tr>
			</table>
			<asp:Panel ID="JavaScriptFixPanel" runat="server" Visible="false">
				<br />
				<strong>How to resolve this issue:</strong>
				<p>Here are the <a class="help" href="http://www.enable-javascript.com/" target="_blank">instructions to enable JavaScript in your web browser</a>.</p>
				<br />
			</asp:Panel>
			<br />
			<asp:Panel ID="CookieTestResultsPanel" runat="server">
				<div class="icon"></div>
				<h2>Cookie Test Results</h2>
			</asp:Panel>
			<table border="0">
				<tr>
					<td style="width: 120px">Status</td>
					<td><asp:Label ID="CookieStatusLabel" runat="server" /></td>
				</tr>
			</table>
			<asp:Panel ID="CookieFixPanel" runat="server" Visible="false">
				<br />
				<strong>How to resolve this issue:</strong>
				<p>
					<asp:PlaceHolder ID="CookieFixContentPlaceHolder" runat="server" Visible="false">
						Here are the <a class="help" href="http://www.enablecookies.org/" target="_blank">instructions to enable Cookies in your web browser</a>.
					</asp:PlaceHolder>
					<asp:PlaceHolder ID="CookieFixIgnoreContentPlaceHolder" runat="server" Visible="false">
						You can ignore this issue as it may be related to the JavaScript issue mentioned above.
					</asp:PlaceHolder>
				</p>
				<br />
			</asp:Panel>
			<div style="text-align: center;">
				<a href="Login.aspx" class="retry-button">Retest My Settings</a>
			</div>
			<br />
			<h2>Additional information</h2>
			<p>Optimum screen resolution is 1024x768 or higher.</p>
		</asp:View>
		<asp:View ID="LoginView" runat="server">
			Login
			<asp:Login ID="Login1" runat="server" ViewStateMode="Disabled" RenderOuterTable="false">
				<LayoutTemplate>
					<p class="validation-summary-errors">
						<asp:Literal runat="server" ID="FailureText" />
					</p>
					<fieldset>
						<legend>Log in Form</legend>
						<ol>
							<li>
								<asp:Label ID="Label1" runat="server" AssociatedControlID="UserName">User name</asp:Label>
								<asp:TextBox runat="server" ID="UserName" />
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="The user name field is required." />
							</li>
							<li>
								<asp:Label ID="Label2" runat="server" AssociatedControlID="Password">Password</asp:Label>
								<asp:TextBox runat="server" ID="Password" TextMode="Password" />
								<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="The password field is required." />
							</li>
							<li>
								<asp:CheckBox runat="server" ID="RememberMe" />
								<asp:Label ID="Label3" runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Remember me?</asp:Label>
							</li>
						</ol>
						<asp:Button ID="Button1" runat="server" CommandName="Login" Text="Log in" />
					</fieldset>
				</LayoutTemplate>
			</asp:Login>
			<p>
				<asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register</asp:HyperLink> if you don't have an account.
			</p>
		</asp:View>
	</asp:MultiView>
</asp:Content>
