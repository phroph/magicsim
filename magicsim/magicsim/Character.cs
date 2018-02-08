using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class Character
    {
        public string ProfileText { get; set; }
        public string CharacterName { get; set; }
        public Character(String profile, String name)
        {
            ProfileText = profile;
            CharacterName = name;
        }
    }
}
