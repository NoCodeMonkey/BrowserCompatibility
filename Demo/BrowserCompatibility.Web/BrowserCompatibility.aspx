<%@ Page Title="Checking Browser Compatibility" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BrowserCompatibility.aspx.cs" Inherits="BrowserCompatibility.Web.BrowserCompatibility" %>
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
		table {
			padding: 0;
			margin: 0;
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
		ul.minimum-requirements
		{
			margin-left: 2em;
			padding-left: 0;
			margin-bottom: 10px;
		}
		ul.minimum-requirements li 
		{
			list-style-type: decimal;
			list-style-position: outside;
		}
	</style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:MultiView ID="BrowserCompatibilityMultiView" runat="server">
		<asp:View ID="BrowserCheckerView" runat="server">
			<BT:BrowserChecker ID="BrowserChecker" runat="server" OnBrowserCompatibilityChecked="BrowserChecker_BrowserCompatibilityChecked"/>
		</asp:View>
		<asp:View ID="BrowserCompatibilityWarningView" runat="server">
			<h1>Browser Compatibility Notice</h1>
			<div class="notice">
				<span>The browser you are using <strong>might not be compatible</strong> with this application.</span>
				<asp:Button ID="ContinueButton" runat="server" Text="Continue Anyway" OnClick="ContinueButton_Click" />
			</div>
			<hr />
			<p>This site has been created to comply with current standards in web design, and hence is viewed best in compliant web browsers. It works best with browsers that support HTML5, CSS, and JavaScript. Optimum screen resolution is 1024x768 or higher.</p>
			<h2>Minimum requirements</h2>
			<p>We continually strive to make this site work just about anywhere, but we have some minimum requirements.</p>
			<ul class="minimum-requirements">
				<li>JavaScript must be enabled. Here are the <a href="http://www.enable-javascript.com/" target="_blank">instructions how to enable JavaScript in your web browser</a>.</li>
				<li>Cookies must be enabled. Here are the <a href="http://www.enablecookies.org/" target="_blank">instructions how to enable Cookies in your web browser</a>.</li>
				<li>A current browser version.</li>
			</ul>
			<h3 class="browser-support">Browser support</h3>
			<p>We recommend using Google Chrome. Below is a list of browsers we support:</p>
			<ul class="browser-icons">
				<li class="browser-chrome clearfix">
					<span><img width="27" height="27" src="images/blank.gif" alt="Spacer" /></span>
					<p><a href="http://www.google.com/chrome" target="_blank">Google Chrome</a> 10 and above</p>
				</li>
				<li class="browser-ff clearfix">
					<span><img width="27" height="27" src="images/blank.gif" alt="Spacer" /></span>
					<p><a href="http://www.getfirefox.net/" target="_blank">Firefox</a> 5 and above</p>
				</li>
				<li class="browser-safari clearfix">
					<span><img width="27" height="27" src="images/blank.gif" alt="Spacer" /></span>
					<p><a href="http://www.apple.com/safari/download/" target="_blank">Safari</a> 5 and above</p>
				</li>
				<li class="browser-ie clearfix">
					<span><img width="27" height="27" src="images/blank.gif" alt="Spacer" /></span>
					<p><a href="http://www.microsoft.com/windows/internet-explorer/default.aspx" target="_blank">Internet Explorer</a> 9 and above</p>
				</li>
				<li class="browser-opera clearfix last">
					<span><img width="27" height="27" src="images/blank.gif" alt="Spacer" /></span>
					<p><a href="http://www.opera.com/download/" target="_blank">Opera</a> 12 and above</p>
				</li>
			</ul>
		</asp:View>
	</asp:MultiView>
</asp:Content>
