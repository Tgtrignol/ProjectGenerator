using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator
{
    class SolutionData
    {
        public string directoryPath;
        public string solutionName;    //Starts with SOLUTION_NAME
        public string projectName;     //After ProjectType
        public ProjectType projectType;//Starts with PROJECT_TYPE
        public List<ClassData> classes = new List<ClassData>();
    }
}
