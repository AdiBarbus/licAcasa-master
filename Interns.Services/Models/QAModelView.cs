using System.Collections.Generic;
using Interns.Core.Data;
using System.Linq;

namespace Interns.Services.Models
{
    public class QAModelView
    {
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
