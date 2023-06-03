using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_test
{
    public class Definition
    {
        public string type;
        public object value;

        public Definition(string type, object value)
        {
            this.type = type;
            this.value = value;
        }
    }
}
