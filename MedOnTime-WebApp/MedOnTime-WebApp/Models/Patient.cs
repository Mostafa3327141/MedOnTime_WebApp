using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MedOnTime_WebApp.Models
{
    public class Patient
    {
        public string Id { get; set; }
        public int PatientID { get; set; }
        public int CaretakerID { get; set; } // for caretaker
        
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Please enter your age")]
        public int Age { get; set; }
        public List<Shape> UnSelectedShapes { get; set; }
        public List<int> MedicationIDs { get; set; }
        public List <int> PrescriptionIDs { get; set; }

        public Patient() { }

        public Patient(int patientID, string firstName, string lastName, string email, string phoneNum, int age)
        {
            this.PatientID = patientID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNum = phoneNum;
            this.Age = age;
            this.MedicationIDs = new List<int>();
            this.CaretakerID = 0;
            this.UnSelectedShapes = new List<Shape> { 
                new Shape { ShapeName = "circle", ShapeDisplay = "Circle" },
                new Shape { ShapeName = "oval", ShapeDisplay = "Oval" },
                new Shape { ShapeName = "triangle", ShapeDisplay = "Triangle" },
                new Shape { ShapeName = "heart", ShapeDisplay = "Heart" },
                new Shape { ShapeName = "pentagon", ShapeDisplay = "Pentagon" },
                new Shape { ShapeName = "hexagon", ShapeDisplay = "Hexagon" },
                new Shape { ShapeName = "octagon", ShapeDisplay = "Octagon" },
                new Shape { ShapeName = "rightTri", ShapeDisplay = "Right Triangle" },
                new Shape { ShapeName = "sTri", ShapeDisplay = "Scalene Triangle" },
                new Shape { ShapeName = "square", ShapeDisplay = "Square" },
                new Shape { ShapeName = "rectangle", ShapeDisplay = "Rectangle" },
                new Shape { ShapeName = "parallelogram", ShapeDisplay = "Parallelogram" },
                new Shape { ShapeName = "trapezuim", ShapeDisplay = "Trapezuim" },
                new Shape { ShapeName = "rhombus", ShapeDisplay = "Rhombus" },
                new Shape { ShapeName = "4star", ShapeDisplay = "4 Pointed Star" },
                new Shape { ShapeName = "star", ShapeDisplay = "5 Pointed Star" },
                new Shape { ShapeName = "6star", ShapeDisplay = "6 Pointed Star" }
            };
            this.PrescriptionIDs = new List<int>();
        }
    }
}
