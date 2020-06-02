using System;
using System.Collections.Generic;

namespace Common
{
    public class MenuItem
    {
        public int? SubMenuId { get; set; }
        public string Description { get; set; }
        public List<Action> Actions { get; set; }
    }
}
