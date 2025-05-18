using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    public class DocumentEditRecord
    {
        public DateTime EditTime { get; }
        public string EditedBy { get; }

        public DocumentEditRecord(string editedBy)
        {
            EditTime = DateTime.Now;
            EditedBy = editedBy;
        }
        public override string ToString()
        {
            return EditTime.ToString() + " document was edited by " + EditedBy;
        }
    }
}

