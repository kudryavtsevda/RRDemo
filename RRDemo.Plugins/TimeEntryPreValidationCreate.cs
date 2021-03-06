﻿using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace RRDemo.Plugins
{
    public class TimeEntryPreValidationCreate : PluginBase
    {
        public TimeEntryPreValidationCreate(string unsecure, string secure)
           : base(typeof(TimeEntryPreValidationCreate))
        {

        }

        protected override void ExecuteCrmPlugin(LocalPluginContext localContext)
        {
            DateTime start;
            DateTime end;
            Guid resourceId;
            Entity timeEntry;
            var logic = new BusinessLogic(localContext.OrganizationService);

            if (!logic.TryValidateContext(localContext.PluginExecutionContext, out timeEntry, out start, out end, out resourceId))
                return;

            if (start == end)
                throw new InvalidPluginExecutionException($"msdyn_start can't be equal msdyn_end");

            var leftRecords = logic.GetTimeEntriesToCreate(start, end, resourceId);

            if (leftRecords.Count() == 0)
                throw new InvalidPluginExecutionException($"Can't create duplicate time entry");
        }
    }
}