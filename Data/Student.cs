using System;
using System.Collections.Generic;

namespace BlazorExam.Data
{
    public partial class Student
    {
        public string GradebookNumber { get; set; }
        public int IdGroup { get; set; }
        public string Name { get; set; }
        public string Secondname { get; set; }
        public string Thirdname { get; set; }

        public virtual Group IdGroupNavigation { get; set; }
    }
}
