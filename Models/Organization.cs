using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modules.OrganizationOrganization.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public byte[] ImagePath { get; set; }
    }
}