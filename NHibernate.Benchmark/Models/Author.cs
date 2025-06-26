using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Benchmark.Models;
public class Author
{
    public virtual int Id { get; set; }
    public virtual string Alias { get; set; }
    public virtual Person Person { get; set; }
    public virtual ISet<Work> Works { get; set; }
}
