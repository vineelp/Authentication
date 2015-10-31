<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserReport.aspx.cs" Inherits="Authentication.UserReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" BorderStyle="Ridge" Font-Names="Verdana" Font-Size="8pt" Height="300px" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="820px" PageCountMode="Actual" ShowBackButton="False" ShowPageNavigationControls="False" ShowZoomControl="False">
            <LocalReport ReportPath="Report1.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="Authentication.DataSet1TableAdapters.View_UsersTableAdapter"></asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
