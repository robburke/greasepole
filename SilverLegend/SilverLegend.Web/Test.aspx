<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="SilverLegend.Web.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Leaderboard test page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>
            Keeping track of Greasepole excellence...
        </p>
        <p>
            Max total achievements on record:
            <asp:label runat="server" text="Score" id="maxTotalAchievement"></asp:label>
        </p>
        <p>
            Longest game on record:
            <asp:label runat="server" text="Score" id="longestGame"></asp:label>
        </p>
        <p>
            Top ten longest recorded games:
            <asp:label runat="server" text="Score" id="topTenScoresLabel"></asp:label>
        </p>
    </div>
    </form>
</body>
</html>
