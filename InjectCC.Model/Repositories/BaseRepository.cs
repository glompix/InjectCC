using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InjectCC.Model.EntityFramework;

namespace InjectCC.Model.Repositories
{
    public abstract class BaseRepository
    {
        protected Context _context;
        protected BaseRepository(Context context)
        {
            _context = context;
        }
    }
}
