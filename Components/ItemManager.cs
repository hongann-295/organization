/*
' Copyright (c) 2020 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Modules.OrganizationOrganization.Models;

namespace Modules.OrganizationOrganization.Components
{
    interface IItemManager
    {
        void CreateItem(Item t);
        void DeleteItem(int itemId, int moduleId);
        void DeleteItem(Item t);
        IEnumerable<Item> GetItems(int moduleId);
        Item GetItem(int itemId, int moduleId);
        void UpdateItem(Item t);
        IEnumerable<GetOrganizationAll> GetOrganization();
        Organization GetOrganization(int organizationId);
        DeleteOrganization DeleteOrganizatio(int organizationId);
        //void SaveOrganization(int orgId, string name, string code, string imagePath);
        void SaveOrganization(Organization org);

    }

    class ItemManager : ServiceLocator<IItemManager, ItemManager>, IItemManager
    {
        public void CreateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Insert(t);
            }
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var t = GetItem(itemId, moduleId);
            DeleteItem(t);
        }

        public void DeleteItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Item> GetItems(int moduleId)
        {
            IEnumerable<Item> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Item GetItem(int itemId, int moduleId)
        {
            Item t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.GetById(itemId, moduleId);
            }
            return t;
        }

        public void UpdateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Update(t);
            }
        }

        protected override System.Func<IItemManager> GetFactory()
        {
            return () => new ItemManager();
        }

        public IEnumerable<GetOrganizationAll> GetOrganization()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<GetOrganizationAll>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetOrganization"));
            }
        }

        public Organization GetOrganization(int organizationId)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                IEnumerable<Organization> x = ctx.ExecuteQuery<Organization>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetOrganizationById {0}", organizationId));
                return x.FirstOrDefault();
            }
        }

        public DeleteOrganization DeleteOrganizatio(int organizationId)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                IEnumerable<DeleteOrganization> y = ctx.ExecuteQuery<DeleteOrganization>(System.Data.CommandType.StoredProcedure, String.Format("Sp_DeleteOrganization {0}", organizationId));
                return y.FirstOrDefault();
            }
        }

        public void SaveOrganization(Organization org)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                ctx.ExecuteQuery<Organization>(System.Data.CommandType.StoredProcedure, String.Format("Sp_SaveOrganization  {0}{1}{2}{3}", org.OrganizationId, org.Name, org.Code, org.ImagePath));

            }
        }

        //public void SaveOrganization(int orgId, string name, string code, string imagePath)
        //{
        //    using (IDataContext ctx = DataContext.Instance())
        //    {
        //        ctx.ExecuteQuery<Organization>(System.Data.CommandType.StoredProcedure, String.Format("Sp_SaveOrganization {0}{1}{2}{3}", orgId, name, code, imagePath));

        //    }
        //}
    }
}
