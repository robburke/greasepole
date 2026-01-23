using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SilverLegend.Web
{
    public static class AnalyticsUtil
    {
        public static string GetIpAddressOfCurrentOperationContext()
        {
            OperationContext context = OperationContext.Current;
            if (context == null) return "unknown";
            MessageProperties messageProperties = context.IncomingMessageProperties;

            RemoteEndpointMessageProperty endpointProperty =
                messageProperties[RemoteEndpointMessageProperty.Name]
                as RemoteEndpointMessageProperty;

            return endpointProperty.Address;

        }

    }
}