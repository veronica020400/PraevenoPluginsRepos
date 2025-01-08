using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PluginPraevenioV
{
    public class creaControllo : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                if (!context.InputParameters.ContainsKey("Target") || !(context.InputParameters["Target"] is Entity))
                    throw new InvalidPluginExecutionException("Il Target non è presente o non è un'entità valida.");

                Entity entity = (Entity)context.InputParameters["Target"];
                tracingService.Trace("Entity: {0}, Id: {1}", entity.LogicalName, entity.Id);

                // Controllo presenza attributi necessari
                if (!entity.Attributes.Contains("cr4f9_dpifisico") || entity["cr4f9_dpifisico"] == null)
                    throw new InvalidPluginExecutionException("Il campo 'cr4f9_dpifisico' non è valorizzato.");

                if (!entity.Attributes.Contains("cr4f9_dataprossimocontrollo") || entity["cr4f9_dataprossimocontrollo"] == null)
                    throw new InvalidPluginExecutionException("Il campo 'cr4f9_dataprossimocontrollo' non è valorizzato.");

                // Recupero valori principali
                Guid consegnaId = entity.Id;
                EntityReference dpiConsegnato = entity.GetAttributeValue<EntityReference>("cr4f9_dpifisico");
                DateTime prossimoControllo = entity.GetAttributeValue<DateTime>("cr4f9_dataprossimocontrollo");

                // Creazione del nuovo record
                Entity controllo = new Entity("cr4f9_elencocontrollidpi");
                controllo["cr4f9_dispositivoconsegnato"] = new EntityReference("cr4f9_associazionedpialavoratore", consegnaId);

                controllo["cr4f9_datascadenzacontrollo"] = prossimoControllo;
                controllo["cr4f9_periodicitadelcontrolloinmesi"] = entity.GetAttributeValue<int>("cr4f9_periodicitacontrollimesi");

                Guid controlloId = service.Create(controllo);
                tracingService.Trace("Creato record 'cr4f9_elencocontrollidpi' con Id: {0}", controlloId);
            }
            catch (Exception ex)
            {
                tracingService.Trace("Errore: {0}", ex.ToString());
                throw;
            }
        }
    }
}
