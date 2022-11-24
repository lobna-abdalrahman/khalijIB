<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowReport.aspx.cs" Inherits="InternetBanking_v1.Views.Shared.ShowReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>HBI Application</title>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
          rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
          rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">
    <div style="vertical-align: top; text-align: center">
        <table style="width: 678px">
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="lblconfirm" runat="server" Font-Bold="True" ForeColor="Red" Width="677px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Tahoma"
                                   Font-Size="10pt" ForeColor="Blue" NavigateUrl="~/MainPage.aspx" Width="146px">Main Page</asp:HyperLink></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <CR:CrystalReportViewer ID="Viewer" runat="server" AutoDataBind="true" 
                                            HasCrystalLogo="False" HasToggleGroupTreeButton="False" ToolPanelView="None" HasToggleParameterPanelButton="false"/>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
    
    </div>
</form>
</body>
</html>

