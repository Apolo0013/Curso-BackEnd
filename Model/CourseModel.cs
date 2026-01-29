namespace BackEnd.Model.Course
{
    class CourseAuthor
    {
        public string Name { set; get; } = "";
        public string SrcAvatar { set; get; } = "";
        public string About { set; get; } = "";
    }

    class CourseModel
    {
        public string Id { get; } = "";
        public string Title { private set; get; } = "";
        public CourseAuthor Author { private set; get; } = new();
        public string Summary { private set; get; } = "";
        public string Description { private set; get; } = "";
        public float Price { private set; get; } = 0;
        public string ThumbnailUrl { private set; get; } = "";
        public List<string> LearningOutcomes { private set; get; } = new();
        public List<string> TargetAudience { private set; get; } = new();
        public List<string> Prerequisites { private set; get; } = new();
        public List<string> CompletionBenefits { private set; get; } = new();

        //para altarar o titulo
        public void UpdateTitle(string newtitle)
        {
            Title = newtitle;
        }
    }
}