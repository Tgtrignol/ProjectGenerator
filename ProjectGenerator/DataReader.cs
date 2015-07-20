using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGenerator
{
    class DataReader
    {
        public static SolutionData ReadSolutionData()
        {
            SolutionData solutionData = new SolutionData();

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "C:\\";
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                solutionData.directoryPath = fbd.SelectedPath;
            }
            else
            {
                ErrorHandling.handleWrongUsage();
            }

            string[] lines = null;
            try
            {
                lines = System.IO.File.ReadAllLines(solutionData.directoryPath + "\\source.txt");

                foreach (string line in lines)
                {
                    string[] parts = line.Trim().Split(' ');

                    if (parts.Length == 0)
                        continue;

                    if (parts[0] == "SOLUTION_NAME")
                    {
                        solutionData.solutionName = parts[1];
                    }
                    else if (parts[0] == "CS_FORMS")
                    {
                        solutionData.projectType = ProjectType.CsForms;
                        solutionData.projectName = parts[1];
                    }
                    else if (parts[0] == "CS_CONSOLE")
                    {
                        solutionData.projectType = ProjectType.CsConsole;
                        solutionData.projectName = parts[1];
                    }
                    else if (parts[0] == "CPP_EMPTY")
                    {
                        solutionData.projectType = ProjectType.CppEmpty;
                        solutionData.projectName = parts[1];
                    }
                    else if (parts[0] == "CLASS")
                    {
                        ClassData classData = new ClassData();
                        classData.className = parts[1];
                        for (int i = 2; i < parts.Length; i += 2)
                        {
                            if (parts[i] == ":")
                            {
                                classData.superClassName = parts[i + 1];
                            }
                            else
                            {
                                classData.interfaceNames.Add(parts[i + 1]);
                            }
                        }
                        solutionData.classes.Add(classData);
                    }
                    else if (parts[0] == "ENUM")
                    {
                        ClassData classData = new ClassData();
                        classData.classType = ClassType.Enum;
                        classData.className = parts[1];
                        solutionData.classes.Add(classData);
                    }
                    else if (parts[0] == "FORM")
                    {
                        ClassData classData = new ClassData();
                        classData.classType = ClassType.Form;
                        classData.className = parts[1];
                        solutionData.classes.Add(classData);
                    }
                    else if (parts[0] == "INTERFACE")
                    {
                        ClassData classData = new ClassData();
                        classData.classType = ClassType.Interface;
                        classData.className = parts[1];
                        solutionData.classes.Add(classData);
                    }
                }
            }
            catch
            {
                ErrorHandling.handleWrongUsage();
            }

            return solutionData;
        }
    }
}
