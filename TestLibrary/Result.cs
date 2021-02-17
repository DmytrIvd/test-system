using System;

namespace TestLibrary
{
    [Serializable]
    public class Result : IEntity
    {
        public Result()
        {
        }

        public int Id { get; set; }
        public DateTime dateOfPassing { get; set; }
        public double PercentageOfRightAnswers { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int TaskId { get; set; }
        public Test Task { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
