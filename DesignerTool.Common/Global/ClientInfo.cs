using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Global
{
    public static class ClientInfo
    {
        public static int Code { get; set; }
        public static bool IsNewInstallation { get; set; }
        

        /* TODO:
                 private int _clientCode = 0;
        public int ClientCode
        {
            get
            {
                if (this._clientCode == 0)
                {
                    using (DesignerToolDbEntities ctx = new DesignerToolDbEntities())
                    {
                        int clientCode;
                        if (Int32.TryParse(ctx.SystemSettings.First(ss => ss.Setting == "ClientCode").Value, out clientCode))
                        {
                            this._clientCode = clientCode;
                        }
                        else
                        {
                            this._clientCode = 0;
                        }
                    }
                }
                return this._clientCode;
            }
        }
         */
    }
}
