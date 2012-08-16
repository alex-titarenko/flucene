using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Mapping;
using System.Reflection;


namespace Lucene.Net.Odm.Mappers
{
    public class CompiledDocumentMapper : IDocumentMapper
    {
        private IDictionary<Type, dynamic> _compiledMappers = new Dictionary<Type, dynamic>();


        #region IDocumentMapper Members

        public Document GetDocument<TModel>(DocumentMapping<TModel> mapping, TModel model, IMappingsService mappingService, string prefix = null)
        {
            ICompiledMapper<TModel> compiledMapper = GetCompiledMapper(mapping, mappingService);
            return compiledMapper.GetDocument(model);
        }

        public TModel GetModel<TModel>(DocumentMapping<TModel> mapping, Document document, IMappingsService mappingService, string prefix = null) where TModel : new()
        {
            ICompiledMapper<TModel> compiledMapper = GetCompiledMapper(mapping, mappingService);
            return compiledMapper.GetModel(document);
        }

        #endregion


        private ICompiledMapper<TModel> GetCompiledMapper<TModel>(DocumentMapping<TModel> mapping, IMappingsService mappingService)
        {
            dynamic compiledMapper;
            if (!_compiledMappers.TryGetValue(typeof(TModel), out compiledMapper))
            {
                compiledMapper = GenerateMapperType(mapping, mappingService);
            }

            return compiledMapper;
        }

        private object GenerateMapperType<TModel>(DocumentMapping<TModel> mapping, IMappingsService mappingService)
        {
            CompiledDocumentMapperTemplate template = new CompiledDocumentMapperTemplate();
            
            Type modelType = typeof(TModel);
            template.ModelName = modelType.FullName;
            template.ShortModelName = modelType.Name;
            template.DocumentMapping = mapping;

            string source = template.TransformText();

            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            options.OutputAssembly = "TempModule" + modelType.Name;

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
