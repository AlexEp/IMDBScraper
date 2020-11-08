using System;
using System.Collections.Generic;
using System.Text;

namespace IMDBScraper.Shared.DTO
{
    public class Actor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public DateTime Birthday { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public ActorGender Gender { get; set; }
        public bool IsHide { get; set; }

        public Actor()
        {
            Roles = new HashSet<string>();
            Gender = ActorGender.Unknown;
        }
    }

    public class ActorNameComparer : IEqualityComparer<Actor>
    {
        public int GetHashCode(Actor co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.Name.GetHashCode();
        }

        public bool Equals(Actor x1, Actor x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.Name.Trim().ToLower() == x2.Name.Trim().ToLower();
        }
    }
}
