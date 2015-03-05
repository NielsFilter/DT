using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.VMTests
{
    public class VMTestBase
    {
        public IDesignerToolContext Context { get; set; }

        public VMTestBase()
        {
            new TestSession();
            this.Context = new DesignerToolDbEntities();
        }
    }
}
