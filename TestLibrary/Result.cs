using System;

namespace TestLibrary
{
    public class Result:IEntity
    {
        public int Id { get; set; }
        public DateTime dateOfPassing { get; set; }
        public double PercentageOfRightAnswers { get; set; }
        public User Sender { get; set; }
        public Test Task { get; set; }
        public Group Group{ get; set; }
    }
}
