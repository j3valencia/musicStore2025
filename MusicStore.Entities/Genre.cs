using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
    public class Genre: EntityBase
    {
       public string Name { get; set; } = default!; //NO ACEPTA NULOS PERO ACECPTA VACIOS
        
    
    }
}