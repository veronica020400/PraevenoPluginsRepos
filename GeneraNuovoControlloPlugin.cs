using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ServiceModel;

public class GeneraNuovoControlloPlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

        IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

        if (!context.InputParameters.ContainsKey("Target"))
            throw new InvalidPluginExecutionException("No target found");

        Entity entity = (Entity)context.InputParameters["Target"];

        //target entity == cr4f9_elencocontrollidpi



        }
    }
}
