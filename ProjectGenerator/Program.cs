using EnvDTE;
using EnvDTE80;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGenerator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            GenerateSolution(DataReader.ReadSolutionData());
        }

        private static void GenerateSolution(SolutionData solutionData)
        {
            try
            {
                MessageFilter.Register();
                System.Type type = System.Type.GetTypeFromProgID("VisualStudio.DTE");
                Object obj = System.Activator.CreateInstance(type, true);
                EnvDTE.DTE dte = (EnvDTE.DTE)obj;
                dte.MainWindow.Visible = false;

                // create a new solution
                dte.Solution.Create(solutionData.directoryPath + "\\", solutionData.solutionName);
                dte.ExecuteCommand("File.SaveAll");

                Solution2 solution = (Solution2)dte.Solution;
                createProject(solutionData, solution, dte);

                EnvDTE.Project project = solution.Projects.Item(1);
                DTE2 dte2 = (DTE2)dte;

                addContentToProject(solutionData, project, dte2);

                // save and quit
                dte.ExecuteCommand("File.SaveAll");
                dte.Quit();
                MessageFilter.Revoke();
            }
            catch
            {
                ErrorHandling.handleWrongUsage();
            }
        }

        private static void addContentToProject(SolutionData solutionData, Project project, DTE2 dte2)
        {
            #region CPP Writing
            if (solutionData.projectType == ProjectType.CppEmpty) //CPP Example
            {
                Directory.CreateDirectory(solutionData.directoryPath + "\\Source Files");
                Directory.CreateDirectory(solutionData.directoryPath + "\\Header Files");

                foreach (ClassData classData in solutionData.classes)
                {
                    #region Class
                    if (classData.classType == ClassType.Class)
                    {
                        Document doc = dte2.ItemOperations.NewFile("General\\Text File", classData.className).Document;
                        TextSelection txtsel = (TextSelection)doc.Selection;
                        txtsel.Text = "";
                        txtsel.Insert("#include \"" + classData.className + ".h\"\n\n" + classData.className + "::" + classData.className + "()\n{\n}\n\n" + classData.className + "::~" + classData.className + "()\n{\n}");
                        doc.Save(solutionData.directoryPath + "\\Source Files\\" + classData.className + ".cpp");
                        project.ProjectItems.AddFromFile(solutionData.directoryPath + "\\Source Files\\" + classData.className + ".cpp");

                        Document doc2 = dte2.ItemOperations.NewFile("General\\Text File", classData.className).Document;
                        TextSelection txtsel2 = (TextSelection)doc2.Selection;
                        txtsel2.Text = "";
                        txtsel2.Insert("#pragma once");
                        if (classData.superClassName != "")
                        {
                            txtsel2.Insert("\n#include \"" + classData.superClassName + "\"");
                        }
                        foreach (string interfaceName in classData.interfaceNames)
                        {
                            txtsel2.Insert("\n#include \"" + interfaceName + "\"");
                        }
                        txtsel2.Insert("\n\nclass " + classData.className);
                        if (classData.superClassName != "")
                        {
                            txtsel2.Insert(" : public " + classData.superClassName);

                            foreach (string interfaceName in classData.interfaceNames)
                            {
                                txtsel2.Insert(", " + interfaceName);
                            }
                        }
                        else if (classData.interfaceNames.Count != 0)
                        {
                            txtsel2.Insert(" : " + classData.interfaceNames[0]);

                            for (int i = 1; i < classData.interfaceNames.Count; ++i)
                            {
                                txtsel2.Insert(", " + classData.interfaceNames[i]);
                            }
                        }
                        txtsel2.Insert("\n{\npublic:\n\t" + classData.className + "();\n\t~" + classData.className + "();\n\nprivate:\n\n};");
                        doc2.Save(solutionData.directoryPath + "\\Header Files\\" + classData.className + ".h");
                        project.ProjectItems.AddFromFile(solutionData.directoryPath + "\\Header Files\\" + classData.className + ".h");
                    }
                    #endregion
                    #region Enum
                    else if (classData.classType == ClassType.Enum)
                    {
                        EnvDTE.Document doc2 = dte2.ItemOperations.NewFile("General\\Text File", classData.className).Document;
                        TextSelection txtsel2 = (TextSelection)doc2.Selection;
                        txtsel2.Text = "";
                        txtsel2.Insert("#pragma once\n\nenum " + classData.className + "\n{\n\n};");
                        doc2.Save(solutionData.directoryPath + "\\Header Files\\" + classData.className + ".h");
                        project.ProjectItems.AddFromFile(solutionData.directoryPath + "\\Header Files\\" + classData.className + ".h");
                    }
                    #endregion
                }
            }
            #endregion
            #region C# Writing
            else //C# Example
            {
                foreach (ProjectItem pItem in project.ProjectItems)
                {
                    if (pItem.Name == "Form1.cs")
                    {
                        pItem.Remove();
                    }
                }

                foreach (ClassData classData in solutionData.classes)
                {
                    if (classData.classType == ClassType.Enum || classData.classType == ClassType.Class || classData.classType == ClassType.Interface)
                        project.ProjectItems.AddFromTemplate(@"C:\Program Files (x86)\Microsoft Visual Studio " + dte2.Version + @"\Common7\IDE\ItemTemplates\CSharp\Code\1033\Class\Class.vstemplate", classData.className + ".cs");
                    else if (classData.classType == ClassType.Form)
                        project.ProjectItems.AddFromTemplate(@"c:\Program Files (x86)\Microsoft Visual Studio " + dte2.Version + @"\Common7\IDE\ItemTemplates\CSharp\Windows Forms\1033\Form\windowsform.vstemplate", classData.className + ".cs");

                    ProjectItem projectItem = null;
                    foreach (ProjectItem pItem in project.ProjectItems)
                    {
                        if (pItem.Name == classData.className + ".cs")
                        {
                            projectItem = pItem;
                            break;
                        }
                    }

                    projectItem.Save();
                    TextSelection txtsel = (TextSelection)projectItem.Document.Selection;

                    #region Class
                    if (classData.classType == ClassType.Class)
                    {
                        txtsel.GotoLine(9);
                        txtsel.EndOfLine();
                        if (classData.superClassName != "")
                        {
                            txtsel.Insert(" : " + classData.superClassName);

                            foreach (string interfaceName in classData.interfaceNames)
                            {
                                txtsel.Insert(", " + interfaceName);
                            }
                        }
                        else if (classData.interfaceNames.Count != 0)
                        {
                            txtsel.Insert(" : " + classData.interfaceNames[0]);

                            for (int i = 1; i < classData.interfaceNames.Count; ++i)
                            {
                                txtsel.Insert(", " + classData.interfaceNames[i]);
                            }
                        }
                    }
                    #endregion
                    #region Enum
                    else if (classData.classType == ClassType.Enum)
                    {
                        txtsel.GotoLine(9);
                        txtsel.StartOfLine();
                        txtsel.CharRight(false, 4);
                        txtsel.DestructiveInsert("enum");
                        txtsel.Delete();
                    }
                    #endregion
                    #region Interface
                    else if (classData.classType == ClassType.Interface)
                    {
                        txtsel.GotoLine(9);
                        txtsel.StartOfLine();
                        txtsel.CharRight(false, 4);
                        txtsel.Insert("interface");
                        txtsel.Delete(5);
                    }
                    #endregion
                }
            }
            #endregion
        }

        private static void createProject(SolutionData solutionData, Solution2 solution, DTE dte)
        {
            switch (solutionData.projectType)
            {
                case ProjectType.CsConsole:
                    solution.AddFromTemplate(solution.GetProjectTemplate("ConsoleApplication.zip", "CSharp"),
                        solutionData.directoryPath + "\\" + solutionData.projectName, solutionData.projectName);
                    break;

                case ProjectType.CsForms:
                    solution.AddFromTemplate(solution.GetProjectTemplate("WindowsApplication.zip", "CSharp"),
                        solutionData.directoryPath + "\\" + solutionData.projectName, solutionData.projectName);
                    break;

                case ProjectType.CppEmpty:
                    object[] contextParams = { "{66bb5dd8-bf70-4784-be56-2273124f2638}", 
                                               solutionData.projectName,
                                               solutionData.directoryPath + "\\"
                                               };
                    dte.LaunchWizard(@"c:\Program Files (x86)\Microsoft Visual Studio " + dte.Version + @"\VC\vcprojects\emptyproj.vsz", contextParams);
                    break;
            }
        }
    }
}