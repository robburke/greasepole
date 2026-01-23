<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SilverLegend.Web.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Legend of the Greasepole - Online Edition</title>
    <meta name="keywords" content="Greasepole, Legend of the Greasepole, Queen's University, Robert Burke, Rob Burke, Carrington Technologies, .NET, Silverlight" />
    <meta name="description" content="The Legend of the Greasepole - Multimedia Tribute to Queen's Applied Science traditions - all the engineering shenanigans you can eat." />
    <link href="/images/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
	    font-family: Arial, Helvetica, sans-serif;
	    background-color: #444444;
	    color: #222222;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
      a:link
        {
            color: #222222;
            text-decoration: none;
        }
        a:active
        {
            color: #222222;
            text-decoration: none;
        }
        a:visited
        {
            color: #222222;
            text-decoration: none;
        }
        a:hover
        {
            color: #A3A3A3;
            text-decoration: none;
        }
    #silverlightControlHost {
	    height: 80%;
	    text-align:center;
	    padding:10px;
	    vertical-align:bottom;
    }
    #explanationHost {
	    height: 20%;
	    text-align:center;
	    padding:6px;
	    margin:10px;
    }
    </style>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="height:100%">
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param name="source" value="ClientBin/SilverLegend.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="black" />
		  <param name="minRuntimeVersion" value="4.0.50401.0" />
          <param name="onSourceDownloadProgressChanged" value="onSourceDownloadProgressChanged" />
          <param name="splashscreensource" value="SplashScreen.xaml" />
          <param name="enableGPUAcceleration" value="true" />
          <param name="enableCacheVisualization" value="false" />
		  <param name="autoUpgrade" value="true" />
            <div style="text-align: center; width: 100%;">
                <div style="width: 400px; margin-left: auto; margin-right: auto; padding: 40px;">
                    <p>
                    To play the shiny new online edition of the world famous Legend of the Greasepole, you'll need to install Microsoft Silverlight.
                    </p>
                    <p>
                    You know you want to.
                    </p>
		                <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration:none" title="Get Microsoft Silverlight">
<%-- <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>--%>
 			                <img src="images/SLMedallion_ENU.png" alt="Get Microsoft Silverlight" style="border-style:none"/>
		                </a>
                </div>
            </div>

	    </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe></div>
    <div id="explanation">
            <div style="text-align: center; width: 100%;">
            <p>
            <a target="_blank" href="http://engsoc.queensu.ca/polegame/LegendWeb/PoleGame/index.htm" title="Legend of the Greasepole at Queen's Engineering Society">How to stall the froshies</a>
            </p>
            <p>
            <a target="_blank" href="http://engsoc.queensu.ca/polegame/technical.htm" title="Legend of the Greasepole Tech Docs">Technical</a> 
            <a target="_blank" href="http://robburke.net/projects/greasepole" title="rob burke's site (Silverlight etc.)"> stuff</a>
            </p>
            <p>
            <a target="_blank" href="http://engsoc.queensu.ca/polegame/LegendWeb/Legends/index.htm" title="The LegendWeb">Sci 99</a>
            </p>
            <p>
                <asp:hyperlink runat="server" id="HighScoresToolTip" target="_blank" ToolTip="Think you can beat these?">
                </asp:hyperlink> 
            </p>
            </div>
    </div>
    </form>
</body>
</html>
