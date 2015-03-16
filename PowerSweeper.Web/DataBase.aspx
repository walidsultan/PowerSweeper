<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataBase.aspx.cs" Inherits="PowerSweeper.Web.DataBase" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnStopServer" runat="server" Text="Stop" 
            onclick="btnStopServer_Click" /><asp:Label ID="lblLogrecordsCount" runat="server" Text=""></asp:Label>

             <asp:Button ID="btnShowUsers" runat="server" Text="Show Users" 
            onclick="btnStopShowUsers_Click" />
    </div>

    <div>
        <asp:GridView ID="grdUsers" runat="server">
        </asp:GridView>
    </div>
    </form>
</body>
</html>
