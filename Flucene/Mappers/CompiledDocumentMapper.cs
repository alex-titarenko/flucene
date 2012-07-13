using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using Lucene.Net.Documents;
using Lucene.Net.Orm.Mapping;
using System.Reflection;


namespace Lucene.Net.Orm.Mappers
{
    public class CompiledDocumentMapper : IDocumentMapper
    {
        IDictionary<Type, dynamic> _compiledMappings = new Dictionary<Type, dynamic>();

        #region IDocumentMapper Members

        public Document GetDocument<TModel>(DocumentMapping<TModel> mapping, TModel model, IMappingsService mappingService)
        {
            dynamic compiledMapper;
            if (!_compiledMappings.TryGetValue(typeof(TModel), out compiledMapper))
            {
                compiledMapper = GenerateMapperType(typeof(TModel));
                _compiledMappings.Add(typeof(TModel), compiledMapper);
            }

            return compiledMapper.GetDocument(model);
        }

        public TModel GetModel<TModel>(DocumentMapping<TModel> mapping, Document document, IMappingsService mappingService) where TModel : new()
        {
            dynamic compiledMapper;
            if (!_compiledMappings.TryGetValue(typeof(TModel), out compiledMapper))
            {
                compiledMapper = GenerateMapperType(typeof(TModel));
            }

            return compiledMapper.GetModel(document);
        }

        #endregion

        public object GenerateMapperType(Type modelType)
        {
            CompiledDocumentMapperTemplate template = new CompiledDocumentMapperTemplate();
            template.ModelName = modelType.FullName;
            template.ShortModelName = modelType.Name;
            string source = template.TransformText();

            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            options.OutputAssembly = "TempModule";

            // Adds needed references
            Assembly mainAssembly = GetType().Assembly;

            var references = mainAssembly
                .GetReferencedAssemblies()
                .Select(x => Assembly.ReflectionOnlyLoad(x.FullName).Location)
                .ToArray();

            options.ReferencedAssemblies.AddRange(references);
            options.ReferencedAssemblies.Add(mainAssembly.Location);
            options.ReferencedAssemblies.Add(modelType.Assembly.Location);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            

            CompilerResults results = provider.CompileAssemblyFromSource(options, source);
            if (results.Errors.Count > 0)
            {
                //If a compiler error is generated, we will throw an exception because 
                //the syntax was wrong - again, this is left up to the implementer to verify syntax before
                //calling the function.  The calling code could trap this in a try loop, and notify a user 
                //the command was not understood, for example.
                throw new ArgumentException("Expression cannot be evaluated, please use a valid C# expression");
            }

            return Activator.CreateInstance(results.CompiledAssembly.GetExportedTypes().First());
        }
    }
}
