using System.Collections.Generic;

namespace Betterki.Models
{
    public class KlipModel
    {
        public List<string> Klipy { get; set; } = new();
        public Dictionary<string, int> Glosy { get; set; } = new();
    }
}
