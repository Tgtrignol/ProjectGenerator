using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator
{
    class ErrorHandling
    {
        public static void handleWrongUsage()
        {
            Console.WriteLine("You have not selected a directory, there was no source.txt present or your source.txt was uncorrectly formatted.");

            Console.WriteLine("EXAMPLE 1: CS_FORMS_EXAMPLE");
            Console.WriteLine("---------------------------");

            Console.WriteLine("SOLUTION_NAME solutionName3");
            Console.WriteLine("CS_FORMS projectName");
            Console.WriteLine("");
            Console.WriteLine("CLASS ClassName : SuperClassName ; InterfaceName");
            Console.WriteLine("ENUM EnumName");
            Console.WriteLine("FORM FormName");
            Console.WriteLine("INTERFACE InterfaceName");

            Console.WriteLine("");
            Console.WriteLine("EXAMPLE 2: CS_CONSOLE_EXAMPLE");
            Console.WriteLine("---------------------------");

            Console.WriteLine("SOLUTION_NAME solutionName3");
            Console.WriteLine("CS_CONSOLE projectName");
            Console.WriteLine("");
            Console.WriteLine("CLASS ClassName : SuperClassName ; InterfaceName");
            Console.WriteLine("ENUM EnumName");
            Console.WriteLine("INTERFACE InterfaceName");

            Console.WriteLine("");
            Console.WriteLine("EXAMPLE 3: CPP_EMPTY_EXAMPLE");
            Console.WriteLine("---------------------------");

            Console.WriteLine("SOLUTION_NAME solutionName3");
            Console.WriteLine("CS_CONSOLE projectName");
            Console.WriteLine("");
            Console.WriteLine("CLASS ClassName : SuperClassName ; InterfaceName");
            Console.WriteLine("ENUM EnumName");

            Console.WriteLine("");
            Console.WriteLine("Press enter to exit..");
            Console.ReadLine();

            Environment.Exit(-1);
        }
    }
}
