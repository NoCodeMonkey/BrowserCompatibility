<%@ Page Title="Browser Compatibility Test" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BrowserCompatibility.Web.Default" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<a href="Login.aspx">Manual Test</a> - This is a manual test - useful where you don't want to run the HttpModule and only wan't to show it on a certain page.
	<br /><br />
	<asp:Literal ID="BrowserInformation" runat="server" />
	<br /><br />
	<asp:Literal ID="BrowserCapabilities" runat="server" />
</asp:Content>