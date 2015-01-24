using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Parity.Game.Script
{
    public class RuntimeCode
    {

        #region Generate

        /// <summary>
        /// Generates Assembly from a single file
        /// </summary>
        /// <param name="fileName">.cs file</param>
        /// <returns>Generated assembly</returns>
        public Assembly GenerateFromFile(string fileName)
        {
            return GenerateFromFiles(new string[] { System.IO.File.ReadAllText(fileName) });
        }

        /// <summary>
        /// Generates Assembly from a multiple files
        /// </summary>
        /// <param name="fileName">.cs files</param>
        /// <returns>Generated assembly</returns>
        public Assembly GenerateFromFiles(string[] fileNames)
        {
            return GenerateFromCode((from fileName in fileNames select System.IO.File.ReadAllText(fileName)).ToArray());
        }

        /// <summary>
        /// Generates Assembly from a code strings
        /// </summary>
        /// <param name="codes">String array containing csharp code</param>
        /// <returns>Generated assembly</returns>
        public Assembly GenerateFromCode(string[] codes)
        {
            return Generate(
                codes,
                new string[] {
                    "System.dll",
                    "\\Lib\\Parity.Base.dll",
                    "\\Lib\\Parity.DS.dll",
                    "\\Lib\\Parity.GameDetails.dll",
                    "\\Lib\\Parity.Kernel.dll",
                    "\\Lib\\Parity.Net.dll",
                    "Parity.Game.exe"
                }
                .Select(x => x.StartsWith("\\Lib") ? Environment.CurrentDirectory + x : x)
                .ToArray());
        }

        /// <summary>
        /// Generates Assembly from code string with given referenced assemblies
        /// </summary>
        /// <param name="code">String array containing csharp code</param>
        /// <param name="referencedAssemblies">String array containing filenames of referenced assemblies</param>
        /// <returns>Generated assembly</returns>
        public Assembly Generate(string[] code, string[] referencedAssemblies)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.ReferencedAssemblies.AddRange(referencedAssemblies);
            compilerParameters.GenerateInMemory = true;
            //compilerParameters.GenerateExecutable = false;

            CompilerResults results = provider.CompileAssemblyFromSource(compilerParameters, code);

            if (results.Errors.HasErrors)
                throw new Exception("Could not compile code: " + String.Join(", ", (from CompilerError err in results.Errors select "[" + err.ErrorNumber + ": " + err.ErrorText + "]")));

            return results.CompiledAssembly;
        }

        #endregion

        #region ParityScript

        public string[] LoadList { get; private set; }
        private Assembly ParityAssembly;

        public RuntimeCode()
        {

            this.LoadList =
                new string[]
                {
                    "Script\\Outbox.cs"
                };

        }

        public void Generate()
        {
            this.ParityAssembly = this.GenerateFromFiles(this.LoadList);
        }

        public IOutboxActivation[] GetOutboxActivation()
        {
            return 
                (from Type t in this.ParityAssembly.GetTypes()
                where t.GetInterface(typeof(IOutboxActivation).FullName, false) != null
                select (IOutboxActivation)this.ParityAssembly.CreateInstance(t.FullName))
                .ToArray();
        }

        #endregion

    }
}
