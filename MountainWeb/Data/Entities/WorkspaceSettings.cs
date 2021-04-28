using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Data.Entities
{
    public class WorkspaceSettings
    {
        [Key]
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
    }
}
