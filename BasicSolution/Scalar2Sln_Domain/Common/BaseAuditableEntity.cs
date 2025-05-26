using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Domain.Common
{
    //带有审计信息（谁创建、何时修改）的基础实体类
    //审计在软件系统中，不是财务那种“查账”，而是指：
    // “记录谁在什么时候对数据做了什么修改。”
    public class BaseAuditableEntity: BaseEntity
    {
        public DateTimeOffset Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
