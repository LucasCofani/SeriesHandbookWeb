using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesHandbookSPA.StateContainer
{
    public class MainStateContainer
    {
        public event Action OnChange;
        public string PageName { get; set; }
        public async Task SetPageName(string newName)
        {
            this.PageName = newName;
            OnChange?.Invoke();
        }
        
    }
}
