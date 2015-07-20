using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator
{
    class ClassData
    {
        public ClassType classType;
        public string className;                                   //After ClassType
        public string superClassName;                              //After a " : " behind the className
        public List<string> interfaceNames = new List<string>();   //After a " ; " behind the className
    }
}
