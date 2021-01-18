using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Student2020.ForumDb
{
    [Table("yaf_User")]
    public class User
    {
        [Key]
        [Column("UserID")]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
