using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Data
{
    public partial class Person : BaseModel
    {
        public static Person New()
        {
            Person newPerson = new Person();
            newPerson.IsActive = true;

            return newPerson;
        }
    }
}
