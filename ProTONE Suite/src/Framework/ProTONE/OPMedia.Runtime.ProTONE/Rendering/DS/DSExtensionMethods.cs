using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE.Rendering.DS
{
    public static class DSExtensionMethods
    {
        public static void ReportGraph(this IGraphBuilder graphBuilder)
        {
            try
            {
                IEnumFilters enumFilters = null;
                int hr = graphBuilder.EnumFilters(out enumFilters);
                DsError.ThrowExceptionForHR(hr);

                IBaseFilter[] filters = new IBaseFilter[1];

                hr = enumFilters.Next(1, filters, IntPtr.Zero);
                while (hr == HRESULT.S_OK)
                {
                    FilterInfo fi;
                    int hr2 = filters[0].QueryFilterInfo(out fi);
                    if (hr2 == HRESULT.S_OK)
                    {
                        Logger.LogTrace($"Filter: {fi.achName}");
                    }

                    hr = enumFilters.Next(1, filters, IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
