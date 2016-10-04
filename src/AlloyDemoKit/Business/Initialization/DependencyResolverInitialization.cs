using System;
using System.Web;
using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using AlloyDemoKit.Business.Rendering;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using StructureMap;
using AlloyDemoKit.Business.Data;
using EPiServer.Core;
using EPiServer.Globalization;

namespace AlloyDemoKit.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DependencyResolverInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.Container));
        }

        private static void ConfigureContainer(ConfigurationExpression container)
        {
            //Swap out the default ContentRenderer for our custom
            container.For<IContentRenderer>().Use<ErrorHandlingContentRenderer>();
            container.For<ContentAreaRenderer>().Use<AlloyContentAreaRenderer>();

            //Implementations for custom interfaces can be registered here.
            container.For<IFileDataImporter>().Use<FileDataImporter>();

            container.For<IUpdateCurrentLanguage>()
                .Singleton()
                .Use<LanguageService>()
                .Setter<IUpdateCurrentLanguage>()
                .Is(x => x.GetInstance<UpdateCurrentLanguage>());
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }

    public class LanguageService : IUpdateCurrentLanguage
    {
        private readonly IUpdateCurrentLanguage _defaultUpdateCurrentLanguage;

        public LanguageService(IUpdateCurrentLanguage defaultUpdateCurrentLanguage)
        {
            _defaultUpdateCurrentLanguage = defaultUpdateCurrentLanguage;
        }

        public void UpdateLanguage(string languageId)
        {
            _defaultUpdateCurrentLanguage.UpdateLanguage("sv");
        }

        public void UpdateReplacementLanguage(IContent currentContent, string requestedLanguage)
        {
            _defaultUpdateCurrentLanguage.UpdateReplacementLanguage(currentContent, "sv");
        }
    }
}
